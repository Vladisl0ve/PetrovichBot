using PetrovichBot.Database;
using Serilog;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace PetrovichBot.Services
{
    public class BotControlService
    {
        private ITelegramBotClient _botClient;
        private IEnvsSettings _envs;
        public BotControlService(ITelegramBotClient bot,
                                 IEnvsSettings envs)
        {
            _botClient = bot;
            _envs = envs;
        }

        public async Task DeleteMessageAsync(long chatId, int msgId, bool toLog = true)
        {
            try
            {
                if (toLog)
                    Log.Information($"Deleting message {msgId} from chat {chatId}");

                Log.Verbose($"Deleting message {msgId} from chat {chatId}");
                await _botClient.DeleteMessageAsync(chatId, msgId);
            }
            catch (Exception ex)
            {
                Log.Error($"MSG: {ex.Message}, InnerExeption: {ex.InnerException?.Message}");
            }
        }

        public async Task<Message?> SendTextMessageAsync(long chatId,
                                               string text,
                                               int? msgThreadId = null,
                                               IReplyMarkup inlineMarkup = default,
                                               CancellationToken cancellationToken = default,
                                               ParseMode? parseMode = null,
                                               bool toLog = true,
                                               int? replyToMsgId = null)
        {
            string logInfo;
            if (chatId < 0)
                logInfo = $"chat id: {chatId}";
            else
                logInfo = $"user id: {chatId}";

            try
            {
                if (toLog)
                    Log.Information($"Message sent to {logInfo}, sent: {text.Replace("\r\n", " ")}");
                Log.Verbose($"Message sent to {logInfo}: {text.Replace("\r\n", " ")}");
                return await _botClient.SendTextMessageAsync(chatId: chatId,
                                     text: text,
                                     messageThreadId: msgThreadId,
                                     replyMarkup: inlineMarkup,
                                     cancellationToken: cancellationToken,
                                     parseMode: parseMode,
                                     replyToMessageId: replyToMsgId);
            }
            catch (ApiRequestException ex)
            {
                Log.Warning($"{ex.Message} : {logInfo}");
                return null;
            }
            catch (Exception ex)
            {
                Log.Error($"MSG: {ex.Message}, InnerExeption: {ex.InnerException?.Message}, USER/CHAT: {logInfo}");
                return null;
            }
        }

        public async Task<Message?> SendStickerAsync(long chatId,
                                           string stickerId,
                                           int? msgThreadId = null,
                                           IReplyMarkup replyMarkup = null,
                                           CancellationToken cancellationToken = default,
                                           bool toLog = true)
        {
            string logInfo;
            if (chatId < 0)
                logInfo = $"chat id: {chatId}";
            else
                logInfo = $"user id: {chatId}";

            try
            {
                if (toLog)
                    Log.Information($"Sticker sent for {logInfo}");

                Log.Verbose($"Sticker sent for {logInfo}");

                return await _botClient.SendStickerAsync(chatId: chatId,
                                                  sticker: new InputFileId(stickerId),
                                                  replyMarkup: replyMarkup,
                                                  messageThreadId: msgThreadId,
                                                  cancellationToken: cancellationToken);
            }
            catch (ApiRequestException ex)
            {
                Log.Warning($"{ex.Message} : {logInfo}");
                return default;
            }
            catch (Exception ex)
            {
                Log.Error($"MSG: {ex.Message}, InnerExeption: {ex.InnerException?.Message}, USER/CHAT: {logInfo}");
                return default;
            }
        }

        public async Task EditMessageTextAsync(long chatId, int messageId, string text, InlineKeyboardMarkup replyMarkup = default, CancellationToken cancellationToken = default, ParseMode? parseMode = null, bool toLog = true)
        {
            string logInfo;
            if (chatId < 0)
                logInfo = $"chat id: {chatId}";
            else
                logInfo = $"user id: {chatId}";

            try
            {
                if (toLog)
                    Log.Information($"Message edited for {logInfo}");

                Log.Verbose($"Message edited for {logInfo}: {text.Replace("\r\n", " ")}");

                await _botClient.EditMessageTextAsync(chatId,
                                               messageId,
                                               text,
                                               replyMarkup: replyMarkup,
                                               cancellationToken: cancellationToken,
                                               parseMode: parseMode);
            }
            catch (ApiRequestException ex)
            {
                if (ex.ErrorCode != 400)
                    Log.Error($"MSG: {ex.Message}, InnerExeption: {ex.InnerException?.Message}, USER/CHAT: {logInfo}");
            }
            catch (Exception ex)
            {
                Log.Error($"MSG: {ex.Message}, InnerExeption: {ex.InnerException?.Message}, USER/CHAT: {logInfo}");
            }
        }


        public async Task SendChatActionAsync(ChatId chatId, ChatAction chatAction, CancellationToken cancellationToken = default)
        {
            try
            {
                await _botClient.SendChatActionAsync(chatId, chatAction, cancellationToken: cancellationToken);
            }
            catch (ApiRequestException ex)
            {
                Log.Error($"{ex.ErrorCode}: {ex.Message}");
            }
            catch (Exception ex)
            {
                Log.Error($"{ex.Message}");
            }
        }
    }
}
