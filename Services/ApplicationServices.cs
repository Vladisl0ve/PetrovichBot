using PetrovichBot.Services.Interfaces;

namespace PetrovichBot.Services
{
    public class ApplicationServices : IApplicationServices
    {
        public BotControlService BotControlService { get; }

        public ApplicationServices(BotControlService botControlService)
        {
            BotControlService = botControlService;
        }
    }
}
