using PetrovichBot.Database;
using PetrovichBot.Extensions.Consts;
using PetrovichBot.Services.Interfaces;
using Telegram.Bot.Types;

namespace PetrovichBot.Controllers
{
    public class SetCommandController
    {
        private readonly IApplicationServices _appServices;
        private readonly IEnvsSettings _envs;
        private readonly long _userId;
        private readonly long _chatId;

        public SetCommandController(IApplicationServices services, IEnvsSettings envs, long userId, long chatId)
        {
            _userId = userId;
            _chatId = chatId;

            _envs = envs;
            _appServices = services;
        }

        public async void UpdateCommands() =>  
                        await _appServices.BotControlService.SetMyCommandsAsync(Commands.GetCommands(),
                                                scope: new BotCommandScopeChat() { ChatId = _chatId });            
        
    }
}
