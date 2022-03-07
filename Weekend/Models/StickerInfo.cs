using System.Collections.Generic;

namespace Weekend.Models
{
	public class StickerInfo
	{
		public int CurrentCounter;
		public readonly List<string> TriggerPhrases;
		public string FileId { get; }

		public StickerInfo(string fileId, int currentCounter, List<string> triggerPhrases)
		{
			FileId = fileId;
			CurrentCounter = currentCounter;
			TriggerPhrases = triggerPhrases;
		}
	}
}