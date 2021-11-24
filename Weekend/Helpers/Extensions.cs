using Telegram.Bot.Types;

namespace Weekend.Helpers
{
    public static class Extensions
    {
        public static string GetUserMessage(this User user)
        {
            var usernameValue = string.IsNullOrEmpty(user.Username) ? "" : $"(@{user.Username})";
            return $"{user.FirstName} {user.LastName} {usernameValue}";
        }

        public static string GetUserAuthMessage(this AuthorizationInfo userInfo)
        {
            var usernameValue = string.IsNullOrEmpty(userInfo.UserName) ? "" : $"(@{userInfo.UserName})";
            return $"{userInfo.FirstName} {userInfo.LastName} {usernameValue}";
        }
    }
}