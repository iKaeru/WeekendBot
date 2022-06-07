using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Weekend.Models;
using Weekend.Enums;
using Weekend.Helpers;

namespace Weekend.Workers
{
	public static class TricksTextFilesParser
	{
		public static List<TrickInfo> RandomTricks { get; private set; } = new List<TrickInfo>();
		public static List<TrickInfo> HardTricks { get; private set; } = new List<TrickInfo>();

		public static void ParseAllTrickFiles()
		{
			foreach (var tricksStorage in PathsConstants.TricksStorages)
			{
				ParseTextFileWithTricks(
					PathResolver.GetFilesPathWithFile(PathsConstants.FilesFolder, tricksStorage.Item1),
					tricksStorage.Item2);
			}

			Console.WriteLine("All trick files are successfully parsed");
		}

		private static void ParseTextFileWithTricks(string fileName, Difficulty difficulty)
		{
			var resultCollection = new List<TrickInfo>();

			using (StreamReader reader = new StreamReader(fileName))
			{
				string line;
				while ((line = reader.ReadLine()) != null)
				{
					var lineWords = line.Trim().Split(' ');
					var trick = new TrickInfo(string.Join(' ', lineWords.Take(lineWords.Length - 1)),
						lineWords[^1], difficulty);
					resultCollection.Add(trick);
					//Console.WriteLine(trick); // For debug
				}
			}

			Console.WriteLine($"Saved {resultCollection.Count} tricks of {difficulty} difficulty");
			switch (difficulty)
			{
				case Difficulty.Easy:
				{
					RandomTricks = resultCollection;
					break;
				}
				case Difficulty.Hard:
				{
					HardTricks = resultCollection;
					break;
				}
				default:
				{
					Console.WriteLine("Found tricks of unknown difficulty!");
					break;
				}
			}
		}
	}
}