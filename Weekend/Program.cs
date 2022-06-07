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
			TricksTextFilesParser.ParseAllTrickFiles();
			UserOwnedTricksParser.ParseAllUserOwnedTrickFiles();
			StickersStorage.Initialize();
            var usersWorker = new UsersWorker(BotClient);
            Console.WriteLine("Trying to get bot name");
            var botName = await BotClient.GetMeAsync();
            Console.WriteLine($"Bot name received: {botName}");
            var worker = new TelegramWorkerService(BotClient, botName, usersWorker, new Logger());
            Task taskA = new Task(async () => await SheduledWorker.EnableBackgroundScheduler(usersWorker));
            taskA.Start();
            worker.ExecuteCore();
            taskA.Wait();
        }
    }
}