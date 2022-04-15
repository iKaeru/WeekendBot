using Weekend.Enums;

namespace Weekend.Models
{
	public class TrickInfo
	{
		public Difficulty Difficulty;
		public string Name;
		public string VideoLink;

		public TrickInfo(string name, string videoLink, Difficulty difficulty)
		{
			Name = name;
			VideoLink = videoLink;
			Difficulty = difficulty;
		}

		public override string ToString()
		{
			return $"\"{Name}\" ({Difficulty}): {VideoLink}";
		}
	}
}