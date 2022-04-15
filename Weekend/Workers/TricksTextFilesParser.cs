using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Weekend.Models;
using Weekend.Enums;

namespace Weekend.Workers
{
	public static class TricksTextFilesParser
	{
		private static List<TrickInfo> _randomTricks = new List<TrickInfo>();
		private static List<TrickInfo> _hardTricks = new List<TrickInfo>();

		public static List<TrickInfo> RandomTricks => _randomTricks;
		public static List<TrickInfo> HardTricks => _hardTricks;

		public static void ParseAllTrickFiles()
		{
			ParseTextFileWithTricks(Path.Combine(Environment.CurrentDirectory, @"Files\", "RandomTricks.txt"), Difficulty.Easy);
			ParseTextFileWithTricks(Path.Combine(Environment.CurrentDirectory, @"Files\", "HardTricks.txt"), Difficulty.Hard);
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

			switch (difficulty)
			{
				case Difficulty.Easy:
				{
					_randomTricks = resultCollection;
					break;
				}
				case Difficulty.Hard:
				{
					_hardTricks = resultCollection;
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