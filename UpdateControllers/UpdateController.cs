using PetrovichBot.Services;
using PetrovichBot.Services.Interfaces;

namespace PetrovichBot.UpdateControllers
{
    public class UpdateController(IApplicationServices applicationServices)
    {
        public readonly IApplicationServices _appServices = applicationServices;
    }
}
