using PetrovichBot.Services.Interfaces;
using PetrovichBot.UpdateControllers;
using Serilog;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace PetrovichBot
{
    public class UpdateHandler : IUpdateHandler
    {
        private readonly IApplicationServices _appServices;
        public UpdateHandler(IApplicationServices applicationServices) 
        {
            _appServices = applicationServices;
        }

        public async Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException
                  => $"Telegram API Error: [{apiRequestException.ErrorCode}]{Environment.NewLine}{apiRequestException.Message}",
                RequestException TGRequestException
                  => $"TG RequestException Error: [{TGRequestException.HttpStatusCode}]{Environment.NewLine}{TGRequestException.Message}",
                _ => exception.ToString()
            };


            Log.Fatal(exception, $"UH => {ErrorMessage}");
            int msToWait = 5000;
            Log.Warning($"Waiting {msToWait / 1000}s...");
            await Task.Delay(msToWait, cancellationToken);
        }

        public Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            switch (update.Type)
            {
                case Telegram.Bot.Types.Enums.UpdateType.Message:
                    new MessageUpdateController(_appServices, update.Message);
                    break;
                case Telegram.Bot.Types.Enums.UpdateType.InlineQuery:
                    break;
                case Telegram.Bot.Types.Enums.UpdateType.ChosenInlineResult:
                    break;
                case Telegram.Bot.Types.Enums.UpdateType.CallbackQuery:
                    break;
                case Telegram.Bot.Types.Enums.UpdateType.EditedMessage:
                    break;
                case Telegram.Bot.Types.Enums.UpdateType.ChannelPost:
                    break;
                case Telegram.Bot.Types.Enums.UpdateType.EditedChannelPost:
                    break;
                case Telegram.Bot.Types.Enums.UpdateType.ShippingQuery:
                    break;
                case Telegram.Bot.Types.Enums.UpdateType.PreCheckoutQuery:
                    break;
                case Telegram.Bot.Types.Enums.UpdateType.Poll:
                    break;
                case Telegram.Bot.Types.Enums.UpdateType.PollAnswer:
                    break;
                case Telegram.Bot.Types.Enums.UpdateType.MyChatMember:
                    break;
                case Telegram.Bot.Types.Enums.UpdateType.ChatMember:
                    break;
                case Telegram.Bot.Types.Enums.UpdateType.ChatJoinRequest:
                    break;
                case Telegram.Bot.Types.Enums.UpdateType.Unknown:
                default:
                    break;

            }
            return Task.CompletedTask;
        }
    }
}
