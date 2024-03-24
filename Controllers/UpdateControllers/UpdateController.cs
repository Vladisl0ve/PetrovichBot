using PetrovichBot.Database;
using PetrovichBot.Services.Interfaces;

namespace PetrovichBot.Controllers.UpdateControllers
{
    public class UpdateController(IApplicationServices applicationServices, IEnvsSettings envsSettings)
    {
        public readonly IApplicationServices _appServices = applicationServices;
        public readonly IEnvsSettings _envs = envsSettings;
    }
}
