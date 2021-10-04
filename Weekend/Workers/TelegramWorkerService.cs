using System;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Args;
using Weekend.Loggers;

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
                ReceiveMessage();
                Thread.Sleep(int.MaxValue);
            }
        }

        private void ReceiveMessage()
        {
            _botClient.OnMessage += Bot_OnMessage;
            _botClient.StartReceiving();
        }

        private static async void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            if (e.Message.Text != null)
            {
            }
        }
    }
}