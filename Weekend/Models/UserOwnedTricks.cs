namespace Weekend.Models
{
	public class UserOwnedTricks
	{
		public string UserName { get; set; }
		public string ReceivedTrickName { get; set; }

		public UserOwnedTricks(string userName, string receivedTrickName)
		{
			UserName = userName;
			ReceivedTrickName = receivedTrickName;
		}

		public override string ToString()
		{
			return $"{UserName} {ReceivedTrickName}";
		}
	}
}
