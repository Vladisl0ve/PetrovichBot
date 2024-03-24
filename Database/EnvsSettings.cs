namespace PetrovichBot.Database
{
    public class EnvsSettings : IEnvsSettings
    {
        public required List<string> Admins              { get; set; }
        public required List<string> ChatsToDevNotify    { get; set; }
        public TimeSpan DevNotifyEvery                   { get; set; }
        public TimeSpan DevExtraNotifyEvery              { get; set; }
        public TimeSpan TriggersEvery                    { get; set; }
        public required string TokenBot                  { get; set; }
        public required string BotstatApiKey             { get; set; }
        public required string ConnectionString          { get; set; }
    }

    public interface IEnvsSettings
    {
        public List<string> Admins           { get; set; }
        public List<string> ChatsToDevNotify { get; set; }
        public TimeSpan DevNotifyEvery       { get; set; }
        public TimeSpan DevExtraNotifyEvery  { get; set; }
        public TimeSpan TriggersEvery        { get; set; }
        public string TokenBot               { get; set; }
        public string BotstatApiKey          { get; set; }
        public string ConnectionString       { get; set; }
    }
}
