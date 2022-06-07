using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Weekend.Models;
using Weekend.Enums;
using Weekend.Helpers;

namespace Weekend.Workers
{
	public static class UserOwnedTricksParser
	{
		public static List<UserOwnedTricks> RandomUserOwnedTricks { get; private set; } = new List<UserOwnedTricks>();
		public static List<UserOwnedTricks> HardUserOwnedTricks { get; private set; } = new List<UserOwnedTricks>();

		public static void ParseAllUserOwnedTrickFiles()
		{
			foreach (var tricksOwned in PathsConstants.TricksOwned)
			{
				ParseTextFileWithUserOwnedTricks(
					PathResolver.GetFilesPathWithFile(PathsConstants.FilesFolder, tricksOwned.Item1),
					tricksOwned.Item2);
			}

			Console.WriteLine("All user owned trick files are successfully parsed");
		}

		public static void WriteToUserOwnedTricksFile(Difficulty difficulty)
		{
			var updatedTricksList = UserOwnedTricksProcessor.ConvertTricksToArray(difficulty);
			var fileName = PathsConstants.TricksOwned
				.First(trick => trick.Item2 == difficulty).Item1;
			using (StreamWriter sw = new StreamWriter(
				       PathResolver.GetFilesPathWithFile(PathsConstants.FilesFolder, fileName)))
			{
				foreach (string trick in updatedTricksList)
				{
					sw.WriteLine(trick);
				}
			}
		}

		private static void ParseTextFileWithUserOwnedTricks(string fileName, Difficulty difficulty)
		{
			var resultCollection = new List<UserOwnedTricks>();

			using (StreamReader reader = new StreamReader(fileName))
			{
				string line;
				while ((line = reader.ReadLine()) != null)
				{
					var lineWords = line.Trim().Split(' ');
					var userTrick = new UserOwnedTricks(lineWords[0],
						string.Join(' ', lineWords.Skip(1).Take(lineWords.Length - 1)));
					resultCollection.Add(userTrick);
					//Console.WriteLine(userTrick); // For debug
				}
			}

			switch (difficulty)
			{
				case Difficulty.Easy:
				{
					RandomUserOwnedTricks = resultCollection;
					break;
				}
				case Difficulty.Hard:
				{
					HardUserOwnedTricks = resultCollection;
					break;
				}
				default:
				{
					Console.WriteLine("Found tricks of unknown difficulty while parsing!");
					break;
				}
			}
		}

		public static void CleanAllOwnedTricks()
		{
			RandomUserOwnedTricks = new List<UserOwnedTricks>();
			HardUserOwnedTricks = new List<UserOwnedTricks>();
			foreach (var difficulty in (Difficulty[]) Enum.GetValues(typeof(Difficulty)))
			{
				WriteToUserOwnedTricksFile(difficulty);
			}
		}
	}
}