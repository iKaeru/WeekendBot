using Telegram.Bot.Types;
using Weekend.Models;

namespace Weekend.Helpers
{
    public static class Extensions
    {
        public static string GetUserMessage(this User user)
        {
            var usernameValue = string.IsNullOrEmpty(user.Username) ? "" : $" (@{user.Username})";
            var lastNameValue = string.IsNullOrEmpty(user.LastName) ? "" : $" {user.LastName}";
            return $"{user.FirstName}{lastNameValue}{usernameValue}";
        }

        public static string GetUserAuthMessage(this AuthorizationInfo userInfo)
        {
            var usernameValue = string.IsNullOrEmpty(userInfo.UserName) ? "" : $" (@{userInfo.UserName})";
            var lastNameValue = string.IsNullOrEmpty(userInfo.LastName) ? "" : $" {userInfo.LastName}";
            return $"{userInfo.FirstName}{lastNameValue}{usernameValue}";
        }
    }
}