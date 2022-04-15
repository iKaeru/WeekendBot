namespace Weekend.Helpers
{
    public static class InfoMessages
    {
        public static readonly string InstagramLink = "https://instagram.com/longboardwknd";
        public static readonly string AdminsCommandName = "@romossjax, @iKaeru, @ohmygoshaaa, @gevorgshaginyan, @TatianaSergevna";

        public static string CreateGreetingNewMemberMsg(string userInfo)
        {
            return $"Внимание!!! У нас новый участник: {userInfo}.\n" +
                   $"Привет и добро пожаловать! Подписывайся на нас в инстаграме: {InstagramLink}\n" +
                   "У нас к тебе есть пару вопросов, дабы лучше узнать тебя: \n" +
                   "Как про нас узнал?\nНа чём катаешь?\nКакой у тебя опыт катания на доске?\nИз какого ты города?\n" +
                   $"А так же ты всегда можешь обратиться лично к любому администратору, например к кому-то из них: {AdminsCommandName}";
        }

        public static string CreateCaptchaMessage(string userMessage, string correctNumber)
        {
            return $"Привет, {userMessage}. Рады тебя видеть в нашем чатике. В целях отсева ботов и спамеров у нас введена капча.\n" +
                   $"Просто нажми на цифру \"{correctNumber}\" снизу, у тебя есть две минуты.\n" +
                   "Если не успеешь или что-то пошло не так - пиши в личку админу romossjax и он тебе поможет";
        }

        public static string CreateRandomTrickStartMessage(string userMessage, string trickInfo)
        {
	        return $"{userMessage}, так, дайка мне подумать, чтобы тебе загадать этакое...\nПускай твой трюк на сегодня будет: {trickInfo}!";
        }
    }
}