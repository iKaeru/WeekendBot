using System;

namespace Weekend.Enums
{
	public enum BotCommands
	{
		RandomTricks,
		HardTricks,
		AdminsList,
		RemoveTrick,
		ClearAllTricks
	}
	public static class BotCommandsExtensions
	{
		/// Commands list:
		/// random_trick - Get random easy trick
		/// hard_trick - Get random hard trick
		/// remove_trick - [admin] Remove given trick
		/// clear_all_tricks - [admin] Remove all given tricks
		/// admins - List of all admins
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
				case BotCommands.RemoveTrick:
					return "remove_trick";
				case BotCommands.ClearAllTricks:
					return "clear_all_tricks";
				default:
					Console.WriteLine($"Not found command for given value: {command}");
					return "";
			}
		}
	}
}
