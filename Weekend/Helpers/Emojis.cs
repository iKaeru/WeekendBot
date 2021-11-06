using System;

namespace Weekend.Helpers
{
    // https://apps.timwhitlock.info/emoji/tables/unicode
    public static class Emojis
    {
        // https://apps.timwhitlock.info/unicode/inspect/hex/1F600-1F64F
        private static string[] emojiValues =
        {
            "\U0001F600", "\U0001F601", "\U0001F602", "\U0001F603", "\U0001F604", "\U0001F605", "\U0001F606",
            "\U0001F607", "\U0001F608", "\U0001F609", "\U0001F60A", "\U0001F60B", "\U0001F60C", "\U0001F60D",
            "\U0001F60E", "\U0001F60F", "\U0001F610", "\U0001F611", "\U0001F612", "\U0001F613", "\U0001F614",
            "\U0001F615", "\U0001F616", "\U0001F617", "\U0001F618", "\U0001F619", "\U0001F61A", "\U0001F61B",
            "\U0001F61C", "\U0001F61D", "\U0001F61E", "\U0001F61F", "\U0001F620", "\U0001F621", "\U0001F622",
            "\U0001F623", "\U0001F624", "\U0001F625", "\U0001F626", "\U0001F627", "\U0001F628", "\U0001F629",
            "\U0001F62A", "\U0001F62B", "\U0001F62C", "\U0001F62D", "\U0001F62E", "\U0001F62F", "\U0001F630",
            "\U0001F631", "\U0001F632", "\U0001F633", "\U0001F634", "\U0001F635", "\U0001F636", "\U0001F637",
            "\U0001F638", "\U0001F639", "\U0001F63A", "\U0001F63B", "\U0001F63C", "\U0001F63D", "\U0001F63E",
            "\U0001F63F", "\U0001F640", "\U0001F641", "\U0001F642", "\U0001F643", "\U0001F644", "\U0001F645",
            "\U0001F646", "\U0001F647", "\U0001F648", "\U0001F649", "\U0001F64A", "\U0001F64B", "\U0001F64C",
            "\U0001F64D", "\U0001F64E", "\U0001F64F",
        };

        public static string GetRandomHexadecimalEmoji()
        {
            var rand = new Random();
            return emojiValues[rand.Next(emojiValues.Length)];
        }
    }
}