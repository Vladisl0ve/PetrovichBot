using PetrovichBot.Services;
using PetrovichBot.Services.Interfaces;

namespace PetrovichBot.UpdateControllers
{
    public class UpdateController
    {
        public readonly IApplicationServices _appServices;
        public UpdateController(IApplicationServices applicationServices) 
        {
            _appServices = applicationServices;
        }
    }
}
