using System;
using System.Collections.Generic;
using System.Linq;
using Telegram.Bot.Types;
using Weekend.Models;

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

        public static AuthorizationInfo AddNewUserToAuthorizeProcess(User user, Message message)
        {
            var currentUser = new AuthorizationInfo(user, message);
            UsersToProcess.Add(currentUser);
            return currentUser;
        }

        public static void RemoveUserFromAuthorizeProcess(AuthorizationInfo user)
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
                RemoveUserFromAuthorizeProcess(user);
            }

            return users;
        }
    }
}