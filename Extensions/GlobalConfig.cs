using PetrovichBot.Database;

namespace PetrovichBot.Extensions
{
    public class GlobalConfig
    {
        public required IEnvsSettings EnvsSettings { get; set; }
    }
}
