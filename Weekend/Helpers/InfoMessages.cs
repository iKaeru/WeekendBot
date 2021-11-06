namespace Weekend.Helpers
{
    public static class InfoMessages
    {
        public static readonly string InstagramName = "@longboardwknd";
        public static readonly string AdminsCommandName = "будет добавлено позже";

        public static string CreateGreetingNewMemberMsg(string userInfo)
        {
            return $"Внимание!!! У нас новый участник: {userInfo}.\n" +
                   $"Привет и добро пожаловать! Подписывайся на нас в инстаграме: {InstagramName}\n" +
                   "У нас к тебе есть пару вопросов, дабы лучше узнать тебя: \n" +
                   "Как про нас узнал?\nНа чём катаешь?\nКакой у тебя опыт катания на доске?\nИз какого ты города?\n" +
                   $"А так же ты всегда можешь обратиться лично к любому администратору, их список можно получить по комманте ({AdminsCommandName})";
        }

        public static string GetUserInfo(AuthorizationInfo userInfo)
        {
            return $"{userInfo.FirstName} {userInfo.LastName}\n(@{userInfo.UserName})";
        }
    }
}