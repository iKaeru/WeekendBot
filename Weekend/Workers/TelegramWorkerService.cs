using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Weekend.Loggers;

// Prod Weekend_Bot. User id: 2044568774
// https://github.com/TelegramBots/Telegram.Bot.Examples

// Disable obsolete warnings
#pragma warning disable 612, 618

namespace Weekend.Workers
{
    public class TelegramWorkerService
    {
        private static ITelegramBotClient _botClient;
        private readonly UsersWorker _usersWorker;
        private static Logger _logger;
        private static int MaxResponseSkip = 3;
        private static int _currentResponseIndex = 0;

        public TelegramWorkerService(ITelegramBotClient botClient, UsersWorker usersWorker, Logger logger)
        {
            _botClient = botClient;
            _usersWorker = usersWorker;
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
            switch (update.Update.Type)
            {
                case UpdateType.Message:
                {
                    if (IsMessageNotOutdated(update))
                    {
                        await BotOnMessageReceived(update.Update.Message);
                    }

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
            if (message?.NewChatMembers != null)
            {
                await _usersWorker.CreateCaptchaForNewMember(message);
            }

            if (message?.Text != null)
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

        private static bool IsMessageNotOutdated(UpdateEventArgs update)
        {
            var timeDiff = DateTime.Now.ToUniversalTime() - update.Update.Message.Date.ToUniversalTime();
            return timeDiff.Minutes <= 1 && timeDiff.Hours == 0 && timeDiff.Days == 0;
        }
    }
}