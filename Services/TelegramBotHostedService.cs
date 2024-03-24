using Microsoft.Extensions.Hosting;
using Serilog;
using Telegram.Bot;
using Telegram.Bot.Polling;

namespace PetrovichBot.Services
{
    public class TelegramBotHostedService(ITelegramBotClient telegramBotClient,
                                    IUpdateHandler updateHandler) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var botName = (await telegramBotClient.GetMyNameAsync(cancellationToken: stoppingToken)).Name;
#if DEBUG
            Log.Information($"DEBUG: Telegram Bot {botName} started");
#else
            Log.Information($"RELEASE: Telegram Bot {botName} started");
#endif
            telegramBotClient.StartReceiving(
                updateHandler: updateHandler,
                cancellationToken: stoppingToken
                );
            // Keep hosted service alive while receiving messages
            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
    }
}
