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
                tasksList.Add(_botClient.DeleteMessageAsync(user.ChatId, user.InviteMessageId));
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
            if (userToAuthorize != null && callbackQuery.Message.MessageId == userToAuthorize.CaptchaMessageId)
            {
	            if (callbackQuery.Data != userToAuthorize.CaptchaCorrectNumber)
	            {
		            await _botClient.KickChatMemberAsync(
			            userToAuthorize.ChatId, userToAuthorize.UserId, DateTime.Now, true);
		            await _botClient.DeleteMessageAsync(userToAuthorize.ChatId, userToAuthorize.CaptchaMessageId);
		            await _botClient.DeleteMessageAsync(userToAuthorize.ChatId, userToAuthorize.InviteMessageId);
	            }
	            else
	            {
		            await _botClient.AnswerCallbackQueryAsync(
			            callbackQueryId: callbackQuery.Id,
			            text: $"Received {callbackQuery.Data}");

		            await _botClient.SendTextMessageAsync(
			            chatId: callbackQuery.Message.Chat,
			            text: InfoMessages.CreateGreetingNewMemberMsg(userToAuthorize.GetTaggedUserAuthMessageWithNames()));

		            UsersAuthorization.RemoveUserFromAuthorizeProcess(userToAuthorize);
		            await _botClient.DeleteMessageAsync(callbackQuery.Message.Chat.Id, callbackQuery.Message.MessageId);
		            await _botClient.RestrictChatMemberAsync(
			            callbackQuery.Message.Chat.Id, userToAuthorize.UserId, LiberalChatPermissions);
	            }
            }
        }

        public async Task CreateCaptchaForNewMember(Message message)
        {
            var usersToAdd = FilterNonBotUsers(message.NewChatMembers);
            if (usersToAdd.Length == 0) return;

            foreach (var user in usersToAdd)
            {
                await _botClient.RestrictChatMemberAsync(message.Chat.Id, user.Id, StrictChatPermissions);
                var addingUser = UsersAuthorization.AddNewUserToAuthorizeProcess(user, message);
                var inlineKeyboard = TextsWorker.CreateInlineButtonsForReply();

                var randomNumber = new Random().Next(1, 4).ToString();
                var sendMessage = await _botClient.SendTextMessageAsync(chatId: message.Chat.Id,
                    text: InfoMessages.CreateCaptchaMessage(user.GetTaggedUserMessageWithNames(), randomNumber),
                    replyMarkup: inlineKeyboard);
                addingUser.CaptchaMessageId = sendMessage.MessageId;
                addingUser.CaptchaCorrectNumber = randomNumber;
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