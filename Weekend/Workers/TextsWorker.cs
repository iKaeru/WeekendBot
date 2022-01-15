using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Weekend.Helpers;

namespace Weekend.Workers
{
    public static class TextsWorker
    {
        private static ITelegramBotClient _botClient;

        public static void Init(ITelegramBotClient botClient)
        {
            _botClient = botClient;
        }

        public static InlineKeyboardMarkup CreateInlineButtonsForReply()
        {
            return new InlineKeyboardMarkup(new[]
            {
                new[]
                {
	                InlineKeyboardButton.WithCallbackData("Нажми 1", "1"),
	                InlineKeyboardButton.WithCallbackData("Нажми 2", "2"),
	                InlineKeyboardButton.WithCallbackData("Нажми 3", "3"),
                }
            });
        }

        public static async Task ReplyInPersonalChat(Message message)
        {
            var messageText = message.Text.ToLower();
            if (messageText.Contains("ping") || messageText.Contains("пинг"))
            {
                await SendReply(message, $"Я живой {Emojis.GetRandomHexadecimalEmoji()}!");
            }
        }

        public static async Task ReplyInGroupChat(Message message)
        {
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

        private static async Task SendReply(Message message, string replyMsg)
        {
            await _botClient.SendTextMessageAsync(
                chatId: message.Chat,
                text: replyMsg,
                replyToMessageId: message.MessageId
            );
        }
    }
}