using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Weekend.Loggers;

// prod bot id = 2044568774

// Disable obsolete warnings
// #pragma warning disable 612, 618

// https://github.com/TelegramBots/Telegram.Bot.Examples
namespace Weekend.Workers
{
    public class TelegramWorkerService
    {
        private static ITelegramBotClient _botClient;
        private readonly UsersWorker _usersWorker;
        private static Logger _logger;
        private static readonly string BotToken = Environment.GetEnvironmentVariable("token");
        private static int MaxResponseSkip = 3;
        private static int _currentResponseIndex = 0;

        public TelegramWorkerService(Logger logger)
        {
            _botClient = new TelegramBotClient(BotToken);
            _usersWorker = new UsersWorker(_botClient);
            TextsWorker.Init(_botClient);
            _logger = logger;
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

        private async void Bot_OnUpdate(object sender, UpdateEventArgs update)
        {
            await _usersWorker.KickUsersThatNotAuthorizedInTime();
            switch (update.Update.Type)
            {
                case UpdateType.Message:
                {
                    await BotOnMessageReceived(update.Update.Message);
                    break;
                }
                case UpdateType.CallbackQuery:
                {
                    await _usersWorker.BotOnCallbackQueryReceived(update.Update.CallbackQuery);
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
                await _usersWorker.CreateCaptchaForNewMember(message);
            }

            if (message.Text != null)
            {
                if (message.Chat.Id == message.From?.Id)
                {
                    await TextsWorker.ReplyInPersonalChat(message);
                }
                else if (IsReplyAvailable())
                {
                    await TextsWorker.ReplyInGroupChat(message);
                }
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
    }
}