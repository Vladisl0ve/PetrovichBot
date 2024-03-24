using Serilog;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace PetrovichBot
{
    public class UpdateHandler : IUpdateHandler
    {
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
            throw new NotImplementedException();
        }
    }
}
