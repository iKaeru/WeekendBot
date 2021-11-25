using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Weekend.Helpers;

namespace Weekend.Workers
{
    public class UsersWorker
    {
        private static ITelegramBotClient _botClient;
        private static readonly ChatPermissions StrictChatPermissions = new ChatPermissions();
        private static readonly ChatPermissions LiberalChatPermissions = new ChatPermissions();

        public UsersWorker(ITelegramBotClient botClient)
        {
            _botClient = botClient;
            StrictChatPermissions.CanSendMessages = false;
            CreateAllChatPermissionsVariable();
        }

        public async Task KickUsersThatNotAuthorizedInTime()
        {
            var users = UsersAuthorization.FindAndDeleteOutdatedAuthUsers();
            var tasksList = new List<Task>();
            foreach (var user in users)
            {
                tasksList.Add(_botClient.KickChatMemberAsync(
                    user.ChatId, user.UserId, DateTime.Now, true));
                tasksList.Add(_botClient.DeleteMessageAsync(user.ChatId, user.CaptchaMessageId));
            }

            try
            {
                await Task.WhenAll(tasksList);
            }
            catch (ApiRequestException)
            {
                // If message was already deleted - OK!
            }
        }

        // Process Inline Keyboard callback data
        public async Task BotOnCallbackQueryReceived(CallbackQuery callbackQuery)
        {
            var userToAuthorize = UsersAuthorization.FindUserToAuthorize(callbackQuery.From.Id);

            if (callbackQuery.Data == "ne_bot" && userToAuthorize != null)
            {
                await _botClient.AnswerCallbackQueryAsync(
                    callbackQueryId: callbackQuery.Id,
                    text: $"Received {callbackQuery.Data}");

                await _botClient.SendTextMessageAsync(
                    chatId: callbackQuery.Message.Chat,
                    text: InfoMessages.CreateGreetingNewMemberMsg(userToAuthorize.GetUserAuthMessage()));

                UsersAuthorization.RemoveAuthorizedUser(userToAuthorize);
                await _botClient.DeleteMessageAsync(callbackQuery.Message.Chat.Id, callbackQuery.Message.MessageId);
                await _botClient.RestrictChatMemberAsync(
                    callbackQuery.Message.Chat.Id, userToAuthorize.UserId, LiberalChatPermissions);
            }
        }

        public async Task CreateCaptchaForNewMember(Message message)
        {
            var usersToAdd = FilterNonBotUsers(message.NewChatMembers);
            if (usersToAdd.Length == 0) return;

            foreach (var user in usersToAdd)
            {
                await _botClient.RestrictChatMemberAsync(message.Chat.Id, user.Id, StrictChatPermissions);
            }

            var users = UsersAuthorization.AddNewUsersToAuthorizeProcess(message);
            var inlineKeyboard = TextsWorker.CreateInlineButtonsForReply();
            var newUser = usersToAdd[0];

            var sendMessage = await _botClient.SendTextMessageAsync(chatId: message.Chat.Id,
                text: InfoMessages.CreateCaptchaMessage(newUser.GetUserMessage()),
                replyMarkup: inlineKeyboard);

            foreach (var user in users)
            {
                user.CaptchaMessageId = sendMessage.MessageId;
            }
        }

        #region privateMethods

        private User[] FilterNonBotUsers(User[] newMembers)
        {
            return newMembers.Where(user => !user.IsBot).ToArray();
        }

        private void CreateAllChatPermissionsVariable()
        {
            LiberalChatPermissions.CanInviteUsers = true;
            LiberalChatPermissions.CanSendMessages = true;
            LiberalChatPermissions.CanSendMediaMessages = true;
            LiberalChatPermissions.CanSendOtherMessages = true;
            LiberalChatPermissions.CanAddWebPagePreviews = true;
        }

        #endregion
    }
}