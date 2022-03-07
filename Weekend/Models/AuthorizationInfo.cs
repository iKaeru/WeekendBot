using System;
using Telegram.Bot.Types;

namespace Weekend.Models
{
	public class AuthorizationInfo
	{
		public long UserId { get; private set; }
		public long ChatId { get; private set; }
		public string UserName { get; private set; }
		public string FirstName { get; private set; }
		public string LastName { get; private set; }
		public DateTime AddedAt { get; private set; }
		public int CaptchaMessageId { get; set; }
		public int InviteMessageId { get; set; }
		public string CaptchaCorrectNumber { get; set; }

		public AuthorizationInfo(User user, Message message)
		{
			UserId = user.Id;
			UserName = user.Username;
			FirstName = user.FirstName;
			LastName = user.LastName;
			AddedAt = DateTime.Now;
			ChatId = message.Chat.Id;
			InviteMessageId = message.MessageId;
		}
	}
}
