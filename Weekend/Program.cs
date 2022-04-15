using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Weekend.Loggers;
using Weekend.Models;
using Weekend.Workers;

namespace Weekend
{
    class Program
    {
        private static readonly string BotToken = Environment.GetEnvironmentVariable("token");
        private static readonly ITelegramBotClient BotClient = new TelegramBotClient(BotToken);

        static async Task Main(string[] args)
        {
			TextFilesParser.ParseAllTrickFiles();
			StickersStorage.Initialize();
            var usersWorker = new UsersWorker(BotClient);
            var worker = new TelegramWorkerService(BotClient, usersWorker, new Logger());
            Task taskA = new Task(async () => await SheduledWorker.EnableBackgroundScheduler(usersWorker));
            taskA.Start();
            worker.ExecuteCore();
            taskA.Wait();
        }
    }
}