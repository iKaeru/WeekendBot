using System;
using System.IO;

namespace Weekend.Helpers
{
	public static class PathResolver
	{
		public static string GetFilesPathWithFile(string folderName, string fileName)
		{
			var currentDir = Environment.CurrentDirectory;
			var pathSeparator = Path.DirectorySeparatorChar.ToString();
			//Path.PathSeparator = (char) pathSeparator;
			if (currentDir.EndsWith("netcoreapp3.1"))
			{
				var parent = Directory.GetParent(currentDir);
				var result = parent.Parent.Parent; // Go to project folder from bin
				return Path.Combine(result.ToString(), folderName, fileName);
			}

			return Path.Combine(currentDir, folderName, fileName);
		}
	}
}