using System;
using System.Collections.Generic;
using System.Linq;
using Telegram.Bot.Types;

namespace Weekend.Helpers
{
    public static class UsersAuthorization
    {
        private static readonly List<AuthorizationInfo> UsersToProcess = new List<AuthorizationInfo>();
        private static readonly TimeSpan AuthAvailableInterval = new TimeSpan(0, 2, 0);

        public static AuthorizationInfo FindUserToAuthorize(long userId)
        {
            return UsersToProcess.Find(user => user.UserId == userId);
        }

        public static List<AuthorizationInfo> AddNewUsersToAuthorizeProcess(Message message)
        {
            var addedUsers = new List<AuthorizationInfo>(message.NewChatMembers.Length);
            foreach (var user in message.NewChatMembers)
            {
                var currentUser = new AuthorizationInfo(user, message);
                addedUsers.Add(currentUser);
                UsersToProcess.Add(currentUser);
            }

            return addedUsers;
        }

        public static void RemoveAuthorizedUser(AuthorizationInfo user)
        {
            UsersToProcess.Remove(user);
        }

        public static List<AuthorizationInfo> FindAndDeleteOutdatedAuthUsers()
        {
            var currentTime = DateTime.Now;
            var users = UsersToProcess
                .Where(user => currentTime.Subtract(user.AddedAt) > AuthAvailableInterval)
                .ToList();
            
            foreach (var user in users)
            {
                RemoveAuthorizedUser(user);
            }
            
            return users;
        }
    }

    public class AuthorizationInfo
    {
        public long UserId { get; private set; }
        public long ChatId { get; private set; }
        public string UserName { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public DateTime AddedAt { get; private set; }
        public int CaptchaMessageId { get; set; }

        public AuthorizationInfo(User user, Message message)
        {
            UserId = user.Id;
            UserName = user.Username;
            FirstName = user.FirstName;
            LastName = user.LastName;
            AddedAt = DateTime.Now;
            ChatId = message.Chat.Id;
        }
    }
}