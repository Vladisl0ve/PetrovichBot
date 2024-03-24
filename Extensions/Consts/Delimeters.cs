namespace PetrovichBot.Extensions.Consts
{
    public static class Delimeters
    {
        public const string BotCommandDelimeter = "/";

        private static List<string> allDelimeters =
        [
            BotCommandDelimeter
        ];

        public static List<string> AllDelimeters { get => allDelimeters; private set => allDelimeters = value; }
    }
}
