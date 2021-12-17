using System;
using System.Threading.Tasks;
using Telegram.Bot;

namespace Weekend.Workers
{
    public static class SheduledWorker
    {
        private static double _timerInterval = new TimeSpan(0, 0, 10).TotalMilliseconds;
        private static double _lastSavedTime = GetUnixCurrentTimestamp();

        public static async Task EnableBackgroundScheduler(UsersWorker usersWorker)
        {
            while (true)
            {
                var nowInMilliseconds = GetUnixCurrentTimestamp();
                if (nowInMilliseconds - _lastSavedTime > _timerInterval)
                {
                    await usersWorker.KickUsersThatNotAuthorizedInTime();
                    _lastSavedTime = nowInMilliseconds;
                }

                await Task.Delay((int) _timerInterval);
            }
        }

        private static long GetUnixTimestamp(DateTime date)
        {
            DateTime zero = new DateTime(1970, 1, 1);
            TimeSpan span = date.Subtract(zero);

            return (long) span.TotalMilliseconds;
        }

        private static long GetUnixCurrentTimestamp()
        {
            return GetUnixTimestamp(DateTime.UtcNow);
        }
    }
}