using Telegram.Bot.Types;
using Weekend.Enums;
using Weekend.Helpers;

namespace Weekend.Workers
{
	public static class TricksProcessor
	{
		public static string ProcessRandomTrick(Message message)
		{
			const Difficulty difficulty = Difficulty.Easy;
			var userName = message.From.Username;
			var addingResult = UserOwnedTricksProcessor.AddUserNewTrickIfPossible(difficulty, userName);
			if (addingResult.Item1)
			{
				UserOwnedTricksParser.WriteToUserOwnedTricksFile(difficulty);
				return InfoMessages.CreateRandomTrickStartMessage(userName, addingResult.Item2.Name, addingResult.Item2.VideoLink);
			}

			return InfoMessages.CreateTrickAlreadyOwnedMessage(userName, addingResult.Item2.Name, addingResult.Item2.VideoLink);
		}

		public static string ProcessHardTrick(Message message)
		{
			const Difficulty difficulty = Difficulty.Hard;
			var userName = message.From.Username;
			var addingResult = UserOwnedTricksProcessor.AddUserNewTrickIfPossible(difficulty, userName);
			if (addingResult.Item1)
			{
				UserOwnedTricksParser.WriteToUserOwnedTricksFile(difficulty);
				return InfoMessages.CreateHardTrickStartMessage(userName, addingResult.Item2.Name, addingResult.Item2.VideoLink);
			}

			return InfoMessages.CreateTrickAlreadyOwnedMessage(userName, addingResult.Item2.Name, addingResult.Item2.VideoLink);
		}
	}
}
