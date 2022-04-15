using System;

namespace Weekend.Enums
{
	public enum BotCommands
	{
		RandomTricks,
		HardTricks,
		AdminsList
	}
	public static class BotCommandsExtensions
	{
		public static string GetString(this BotCommands command)
		{
			switch (command)
			{
				case BotCommands.RandomTricks:
					return "random_trick";
				case BotCommands.HardTricks:
					return "hard_trick";
				case BotCommands.AdminsList:
					return "admins";
				default:
					Console.WriteLine($"Not found command for given value: {command}");
					return "";
			}
		}
	}
}
