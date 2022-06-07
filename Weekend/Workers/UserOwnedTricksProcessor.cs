using System;
using System.Linq;
using Weekend.Enums;
using Weekend.Models;

namespace Weekend.Workers
{
	public static class UserOwnedTricksProcessor
	{
		public static DateTime LastTimeTricksOwned;

		public static (bool, TrickInfo) AddUserNewTrickIfPossible(Difficulty difficulty, string userName)
		{
			var rand = new Random();
			LastTimeTricksOwned = DateTime.Now;
			switch (difficulty)
			{
				case Difficulty.Easy:
				{
					var userOwnedTrick = UserOwnedTricksParser.RandomUserOwnedTricks
						.FirstOrDefault(tricks => tricks.UserName == userName);
					if (userOwnedTrick is null)
					{
						var newTrick =
							TricksTextFilesParser.RandomTricks[rand.Next(TricksTextFilesParser.RandomTricks.Count)];
						UserOwnedTricksParser.RandomUserOwnedTricks.Add(new UserOwnedTricks(userName,
							newTrick.Name));
						return (true, newTrick);
					}

					var trickName = userOwnedTrick.ReceivedTrickName;
					return (false,
						new TrickInfo(trickName,
							TricksTextFilesParser.RandomTricks.FirstOrDefault(t => t.Name.Equals(trickName))?.VideoLink,
							difficulty));
				}
				case Difficulty.Hard:
				{
					var userOwnedTrick = UserOwnedTricksParser.HardUserOwnedTricks
						.FirstOrDefault(tricks => tricks.UserName == userName);
					if (userOwnedTrick is null)
					{
						var newTrick =
							TricksTextFilesParser.HardTricks[rand.Next(TricksTextFilesParser.HardTricks.Count)];
						UserOwnedTricksParser.HardUserOwnedTricks.Add(new UserOwnedTricks(userName,
							newTrick.Name));
						return (true, newTrick);
					}

					var trickName = userOwnedTrick.ReceivedTrickName;
					return (false,
						new TrickInfo(trickName,
							TricksTextFilesParser.HardTricks.FirstOrDefault(t => t.Name.Equals(trickName))?.VideoLink,
							difficulty));
				}
				default:
				{
					Console.WriteLine(
						$"Found trick of unknown difficulty while creating random trick for user \"{userName}\"");
					return (false, null);
				}
			}
		}

		public static string[] ConvertTricksToArray(Difficulty difficulty)
		{
			string[] result = default;
			switch (difficulty)
			{
				case Difficulty.Easy:
				{
					result = UserOwnedTricksParser.RandomUserOwnedTricks.Select(trick => trick.ToString()).ToArray();
					break;
				}
				case Difficulty.Hard:
				{
					result = UserOwnedTricksParser.HardUserOwnedTricks.Select(trick => trick.ToString()).ToArray();
					break;
				}
				default:
				{
					Console.WriteLine("Found tricks of unknown difficulty while converting!");
					break;
				}
			}

			return result;
		}

		public static bool AreOwnedTricksOutdated()
		{
			if ((DateTime.Now.Day > LastTimeTricksOwned.Day || DateTime.Now.Month > LastTimeTricksOwned.Month))
			{
				return true;
			}

			return false;
		}
	}
}