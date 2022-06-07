using Weekend.Enums;

namespace Weekend.Helpers
{
	public static class PathsConstants
	{
		public const string FilesFolder = "Files";

		public const string RandomTricksStorage = "RandomTricks.txt";
		public const string HardTricksStorage = "HardTricks.txt";

		public const string RandomTricksOwned = "RandomTricks_users.txt";
		public const string HardTricksOwned = "HardTricks_users.txt";

		public static (string, Difficulty)[] TricksStorages =
			{(RandomTricksStorage, Difficulty.Easy), (HardTricksStorage, Difficulty.Hard)};

		public static (string, Difficulty)[] TricksOwned =
			{(RandomTricksOwned, Difficulty.Easy), (HardTricksOwned, Difficulty.Hard)};
	}
}