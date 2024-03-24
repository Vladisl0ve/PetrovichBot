using PetrovichBot.Properties;
using System.Text.RegularExpressions;
using Telegram.Bot.Types;

namespace PetrovichBot.Extensions.Consts
{
    public static class Commands
    {
        public const string RandomJokeCommand = "random_joke";
        public const string TopJokeCommand = "top_joke";
        public const string StartCommand = "start";

        private static List<string> allCommands =
        [
            RandomJokeCommand,
            TopJokeCommand,
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
            };

            return result;
        }
    }
}
