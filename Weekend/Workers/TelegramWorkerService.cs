using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Weekend.Loggers;

// Disable obsolete warnings
#pragma warning disable 612, 618

namespace Weekend.Workers
{
    public class TelegramWorkerService
    {
        private static ITelegramBotClient _botClient;
        private static Logger _logger;
        private static readonly string BotToken = Environment.GetEnvironmentVariable("token");

        public TelegramWorkerService(Logger logger)
        {
            _botClient = new TelegramBotClient(BotToken);
            _logger = logger;
        }

        public void ExecuteCore()
        {
            while (true)
            {
                ReceiveUpdates();
                // ReceiveMessage();
                Thread.Sleep(int.MaxValue);
            }
        }

        private void ReceiveUpdates()
        {
            _botClient.OnUpdate += Bot_OnUpdate;
            _botClient.StartReceiving();
        }

        private void ReceiveMessage()
        {
            _botClient.OnMessage += Bot_OnMessage;
            _botClient.StartReceiving();
        }

        private async void Bot_OnUpdate(object sender, UpdateEventArgs update)
        {
            if (update.Update?.Message?.Text != null)
            {
                await FindWordsAndAnswer(update);
            }
        }

        private async void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            Console.WriteLine($"Received message = {e.Message}, chat id = {e.Message.Chat.Id}");

            if (e.Message.NewChatMembers != null)
            {
                // await bot.sendMessage(message.chat.id, message.new_chat_member.username + " joined!");
            }

            if (e.Message.Text != null)
            {
                await FindWordsAndAnswer(e);
            }
        }

        private async Task FindWordsAndAnswer(MessageEventArgs e)
        {
            var messageText = e.Message.Text.ToLower();
            if (messageText.Contains("щёлк") || messageText.Contains("щелк") || messageText.Contains("щелч"))
            {
                await SendReply(e, "Флип");
                // maybe add random emodji
            }

            if (messageText.Contains("флип"))
            {
                await SendReply(e, "Щёлк");
                // maybe add random emodji
            }
        }

        private async Task SendReply(MessageEventArgs eventArgs, string replyMsg)
        {
            await _botClient.SendTextMessageAsync(
                chatId: eventArgs.Message.Chat,
                text: replyMsg,
                replyToMessageId: eventArgs.Message.MessageId
            );
        }
    }
}