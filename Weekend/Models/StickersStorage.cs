using System.Collections.Generic;
using System.Linq;
using Telegram.Bot.Types;

namespace Weekend.Models
{
	static class StickersStorage
	{
		private static Dictionary<ChatSticker, StickerInfo> _stickersIdsAndCount;
		private static int _maxResponseSkip = 3;

		// Receive id via: message?.Sticker?.FileId
		public static void Initialize()
		{
			_stickersIdsAndCount = new Dictionary<ChatSticker, StickerInfo>
			{
				{
					ChatSticker.ParkGorkogo,
					new StickerInfo("CAACAgIAAx0CXXbSjQACA6RiJnEMh5xlOS33ILMfHw4WsmHIzAACvg8AAoH0KUvSAcQWzFmJviME",
						_maxResponseSkip,
						new List<string> {"пг", "горьког"})
				},
				{
					ChatSticker.Reddeck,
					new StickerInfo("CAACAgIAAx0CXXbSjQACA6ViJnE8KaH8DTg4kC6ozMkg3yyELwACDhEAAkeo2EphnFODe5B9qCME",
						_maxResponseSkip,
						new List<string> {"реддек", "ред дек"})
				},
				{
					ChatSticker.Hodynka,
					new StickerInfo("CAACAgIAAx0CXXbSjQACA6ZiJnGPMH4UejAVprtJG_gkdRgeGgACNBIAAsaKeEhBylg54-01TyME",
						_maxResponseSkip,
						new List<string> {"ходынк", "ходынск", "цска"})
				},
				{
					ChatSticker.Grisha,
					new StickerInfo("CAACAgIAAx0CXXbSjQACA6NiJnDDISX64BFz9LqGJzLbKZmCgwAC-g4AAvDIkUjLbU5d_7ppgCME",
						_maxResponseSkip, new List<string> {"гриш", "иисус", "грег", "григор", "рожков"})
				},
				{
					ChatSticker.Pepper,
					new StickerInfo("CAACAgIAAxkBAANQYiZjF6dbYWcf0gsRYA6vn08FRQcAAj8YAAJ--9hKCjZTgitmcyAjBA",
						_maxResponseSkip,
						new List<string>
						{
							"timur", "тимур", "totoev", "тотоев", "pepper", "peper", "пеппер", "пепер", "пепир", "пипер"
						})
				},
				{
					ChatSticker.Katana,
					new StickerInfo("CAACAgIAAxkBAANOYiZjCkIlG0w6I_B1DYXOCDIzbOoAAksRAAIMLtlKu8DLH6-eDegjBA",
						_maxResponseSkip, new List<string> {"katan", "катан"})
				},
				{
					ChatSticker.Pivot,
					new StickerInfo("CAACAgIAAxkBAANhYiaRwQyQuC8iv6qt2aaeQEzEa0oAAg8TAAJjIZFIwpLRw3h5yJojBA",
						_maxResponseSkip, new List<string> {"пивот"})
				},
				{
					ChatSticker.Canal,
					new StickerInfo("CAACAgIAAxkBAANiYiaSJARsOLM5MHyYK69_cKOrAAE3AAIpEwACPW7QSkDdBSvDFNvMIwQ",
						_maxResponseSkip, new List<string> {"канав"})
				},
				{
					ChatSticker.Amen,
					new StickerInfo("CAACAgIAAxkBAANjYiaScgFUZHrG5X3_8kLtm2eYK7AAArUPAALNLylLKV7q_66ho6QjBA",
						_maxResponseSkip, new List<string> {"аминь"})
				},
				{
					ChatSticker.Busy,
					new StickerInfo("CAACAgIAAxkBAANkYiaSsYp9HpILXSFm5lQQBCP8YacAAtANAAIuqyhLOxsWuEyeGLkjBA",
						_maxResponseSkip, new List<string> {"занят", "перезвоню"})
				},
				{
					ChatSticker.Kitty,
					new StickerInfo("CAACAgIAAxkBAANlYiaS4Knc8wozfdissE3f9Aelo9kAAj0SAAI4lSlLbNJDOko7wDAjBA",
						_maxResponseSkip, new List<string> {"киск"})
				}
			};
		}

		public static bool IsResponseAvailable(ChatSticker key)
		{
			IncreaseStickerCounter(key);

			if (_stickersIdsAndCount[key].CurrentCounter < _maxResponseSkip)
				return false;

			ClearStickerCounter(key);
			return true;
		}

		public static bool IsStickerTriggered(ChatSticker key, Message message)
		{
			return _stickersIdsAndCount[key].TriggerPhrases.Any(message.Text.Contains);
		}

		public static string GetStickerId(ChatSticker key)
		{
			return _stickersIdsAndCount[key].FileId;
		}

		private static void IncreaseStickerCounter(ChatSticker key)
		{
			_stickersIdsAndCount[key].CurrentCounter++;
		}

		private static void ClearStickerCounter(ChatSticker key)
		{
			_stickersIdsAndCount[key].CurrentCounter = 0;
		}
	}
}