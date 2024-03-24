using PetrovichBot.Services.Interfaces;

namespace PetrovichBot.Services
{
    public class ApplicationServices : IApplicationServices
    {
        public BotControlService BotControlService { get; }
        public HttpService HttpService { get; }

        public ApplicationServices(BotControlService botControlService, HttpService httpService)
        {
            BotControlService = botControlService;
            HttpService = httpService;
        }
    }
}
