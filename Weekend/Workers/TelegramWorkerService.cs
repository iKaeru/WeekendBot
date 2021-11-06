using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Weekend.Helpers;
using Weekend.Loggers;

// prod bot id = 2044568774

// Disable obsolete warnings
#pragma warning disable 612, 618

// https://github.com/TelegramBots/Telegram.Bot.Examples
namespace Weekend.Workers
{
    public class TelegramWorkerService
    {
        private static ITelegramBotClient _botClient;
        private static Logger _logger;
        private static readonly string BotToken = Environment.GetEnvironmentVariable("token");
        private static readonly ChatPermissions StrictChatPermissions = new ChatPermissions();
        private static readonly ChatPermissions LiberalChatPermissions = new ChatPermissions();
        private static int MaxResponseSkip = 3;
        private static int _currentResponseIndex = 0;

        public TelegramWorkerService(Logger logger)
        {
            _botClient = new TelegramBotClient(BotToken);
            _logger = logger;
            StrictChatPermissions.CanSendMessages = false;
            EnableAllChatPermissions();
        }

        public void ExecuteCore()
        {
            while (true)
            {
                ReceiveUpdates();
                Thread.Sleep(int.MaxValue);
            }
        }

        private void ReceiveUpdates()
        {
            _botClient.OnUpdate += Bot_OnUpdate;
            _botClient.StartReceiving();
        }

        private async Task KickUsersThatNotAuthorizedInTime()
        {
            var users = UsersAuthorization.FindAndDeleteOutdatedAuthUsers();
            var tasksList = new List<Task>();
            foreach (var user in users)
            {
                tasksList.Add(_botClient.KickChatMemberAsync(
                    user.ChatId, user.UserId, DateTime.Now, true));
                tasksList.Add(_botClient.DeleteMessageAsync(user.ChatId, user.CaptchaMessageId));
            }

            await Task.WhenAll(tasksList);
        }

        private async void Bot_OnUpdate(object sender, UpdateEventArgs update)
        {
            await KickUsersThatNotAuthorizedInTime();
            switch (update.Update.Type)
            {
                case UpdateType.Message:
                {
                    await BotOnMessageReceived(update.Update.Message);
                    break;
                }
                case UpdateType.CallbackQuery:
                {
                    await BotOnCallbackQueryReceived(update.Update.CallbackQuery);
                    break;
                }
                default:
                {
                    Console.WriteLine("Unknown handler");
                    break;
                }
            }
        }

        private async Task BotOnMessageReceived(Message message)
        {
            if (message.NewChatMembers != null)
            {
                await CreateCaptchaForNewMember(message);
            }

            if (message.Chat.Id == message.From?.Id)
            {
                await ProcessPersonalMessage(message);
            }
            else
            {
                await ProcessGroupMessage(message);
            }
        }

        // Process Inline Keyboard callback data
        private async Task BotOnCallbackQueryReceived(CallbackQuery callbackQuery)
        {
            var userToAuthorize = UsersAuthorization.FindUserToAuthorize(callbackQuery.From.Id);

            if (callbackQuery.Data == "ne_bot" && userToAuthorize != null)
            {
                await _botClient.AnswerCallbackQueryAsync(
                    callbackQueryId: callbackQuery.Id,
                    text: $"Received {callbackQuery.Data}");

                await _botClient.SendTextMessageAsync(
                    chatId: callbackQuery.Message.Chat,
                    text: InfoMessages.CreateGreetingNewMemberMsg(InfoMessages.GetUserInfo(userToAuthorize)));

                UsersAuthorization.RemoveAuthorizedUser(userToAuthorize);
                await _botClient.DeleteMessageAsync(callbackQuery.Message.Chat.Id, callbackQuery.Message.MessageId);
                await _botClient.RestrictChatMemberAsync(
                    callbackQuery.Message.Chat.Id, userToAuthorize.UserId, LiberalChatPermissions);
            }
        }

        private async Task CreateCaptchaForNewMember(Message message)
        {
            var usersToAdd = FilterNonBotUsers(message.NewChatMembers);
            if (usersToAdd.Length == 0) return;

            foreach (var user in usersToAdd)
            {
                await _botClient.RestrictChatMemberAsync(message.Chat.Id, user.Id, StrictChatPermissions);
            }

            var users = UsersAuthorization.AddNewUsersToAuthorizeProcess(message);
            var inlineKeyboard = CreateInlineButtonsForReply();
            var sendMessage = await _botClient.SendTextMessageAsync(chatId: message.Chat.Id,
                text: "Докажи что ты не бот",
                replyMarkup: inlineKeyboard);

            foreach (var user in users)
            {
                user.CaptchaMessageId = sendMessage.MessageId;
            }
        }

        private User[] FilterNonBotUsers(User[] newMembers)
        {
            return newMembers.Where(user => !user.IsBot).ToArray();
        }

        private InlineKeyboardMarkup CreateInlineButtonsForReply()
        {
            return new InlineKeyboardMarkup(new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData("Я не бот", "ne_bot"),
                }
            });
        }

        private async Task ProcessPersonalMessage(Message message)
        {
        }

        private async Task ProcessGroupMessage(Message message)
        {
            if (message?.Text != null)
            {
                await FindTextAndReply(message);
            }
        }

        private async Task FindTextAndReply(Message message)
        {
            if (!IsReplyAvailable())
                return;

            var messageText = message.Text.ToLower();
            if (messageText.Contains("щёлк") || messageText.Contains("щелк") || messageText.Contains("щелч")
                || messageText.Contains("щёлч") || messageText.Contains("счелк") || messageText.Contains("счёлк"))
            {
                await SendReply(message, $"Флип {Emojis.GetRandomHexadecimalEmoji()}");
                return;
            }

            if (messageText.Contains("флип"))
            {
                await SendReply(message, $"Щёлк {Emojis.GetRandomHexadecimalEmoji()}");
                return;
            }
        }

        private static bool IsReplyAvailable()
        {
            _currentResponseIndex++;
            if (_currentResponseIndex < MaxResponseSkip)
                return false;

            _currentResponseIndex = 0;
            return true;
        }

        private async Task SendReply(Message message, string replyMsg)
        {
            await _botClient.SendTextMessageAsync(
                chatId: message.Chat,
                text: replyMsg,
                replyToMessageId: message.MessageId
            );
        }

        private void EnableAllChatPermissions()
        {
            LiberalChatPermissions.CanInviteUsers = true;
            LiberalChatPermissions.CanSendMessages = true;
            LiberalChatPermissions.CanSendMediaMessages = true;
            LiberalChatPermissions.CanSendOtherMessages = true;
            LiberalChatPermissions.CanAddWebPagePreviews = true;
        }
    }
}