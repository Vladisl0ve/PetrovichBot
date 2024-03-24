using PetrovichBot.Properties;
using System.Text.RegularExpressions;
using Telegram.Bot.Types;

namespace PetrovichBot.Extensions.Consts
{
    public static class Commands
    {
        public const string RandomJokeCommand = "random_joke";
        public const string RandomBezdnaCommand = "random_bezdna";
        public const string TopJokeCommand = "top_joke";
        public const string TopBezdnaCommand = "top_bezdna";
        public const string StartCommand = "start";

        private static List<string> allCommands =
        [
            RandomJokeCommand,
            TopJokeCommand,
            RandomBezdnaCommand,
            TopBezdnaCommand,
            StartCommand,
        ];

        public static List<string> AllCommands { get => allCommands; private set => allCommands = value; }

        public static List<BotCommand> GetCommands()
        {
            List<BotCommand> result = new()
            {
                new BotCommand()
                {
                    Command = RandomJokeCommand,
                    Description = nameof(Resources.RandomJokeButton).UseCulture("ru")
                },
                new BotCommand()
                {
                    Command = TopJokeCommand,
                    Description = nameof(Resources.TopJokeButton).UseCulture("ru")
                },
                new BotCommand()
                {
                    Command = RandomBezdnaCommand,
                    Description = nameof(Resources.RandomBezdnaButton).UseCulture("ru")
                },
                new BotCommand()
                {
                    Command = TopBezdnaCommand,
                    Description = nameof(Resources.TopBezdnaButton).UseCulture("ru")
                },
            };

            return result;
        }
    }
}
