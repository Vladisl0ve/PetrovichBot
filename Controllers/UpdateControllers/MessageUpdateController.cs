using PetrovichBot.Database;
using PetrovichBot.Extensions;
using PetrovichBot.Extensions.Consts;
using PetrovichBot.Properties;
using PetrovichBot.Services;
using PetrovichBot.Services.Interfaces;
using Serilog;
using System.Globalization;
using Telegram.Bot.Types;

namespace PetrovichBot.Controllers.UpdateControllers
{
    internal class MessageUpdateController : UpdateController
    {
        private readonly Message _message;

        public MessageUpdateController(IApplicationServices applicationServices, IEnvsSettings envsSettings, Message message) : base(applicationServices, envsSettings)
        {
            _message = message;
        }

        internal void ProcessMessage()
        {
            if (_message.Chat.Id > 0)
                new PrivateMessageUpdateController(_appServices, _message).ProcessCommand();
            else
                new PublicMessageUpdateController(_appServices, _message).ProcessCommand();

            new SetCommandController(_appServices, _envs, _message.From.Id, _message.Chat.Id).UpdateCommands();
        }

        protected class PrivateMessageUpdateController(IApplicationServices applicationServices, Message message) : BasicMessageUpdateController(applicationServices, message)
        {
            public override async Task<string?> SendTopJoke()
            {
                var textToSend = await _appServices.HttpService.GetTopJoke();
                if (textToSend == default)
                {
                    Log.Fatal($"Error on getting (private) TOP joke: [ID: {Message.Chat.Id}| {Message.Text}]");
                    return default;
                }
                Log.Debug($"Processed getting (private) TOP joke for {Message.Chat.Id}");
                await _appServices.BotControlService.SendTextMessageAsync(Message.Chat.Id,
                                                                          textToSend,
                                                                          Message.MessageThreadId,
                                                                          inlineMarkup: ExtensionMethods.MenuKeyboardMarkup(UserCulture),
                                                                          parseMode: Telegram.Bot.Types.Enums.ParseMode.Html,
                                                                          toLog: false);
                return textToSend;
            }

            public override async Task<string?> SendRandomJoke()
            {
                var textToSend = await _appServices.HttpService.GetRandomJoke();
                if (textToSend == default)
                {
                    Log.Fatal($"Error on getting random joke: [ID: {Message.Chat.Id}| {Message.Text}]");
                    return default;
                }
                Log.Debug($"Processed getting random joke for {Message.Chat.Id}");
                await _appServices.BotControlService.SendTextMessageAsync(Message.Chat.Id,
                                                                          textToSend,
                                                                          Message.MessageThreadId,
                                                                          inlineMarkup: ExtensionMethods.MenuKeyboardMarkup(UserCulture),
                                                                          parseMode: Telegram.Bot.Types.Enums.ParseMode.Html,
                                                                          toLog: false);
                return textToSend;
            }

            public override async Task SendStartMessage()
            {
                var textToSend = nameof(Resources.StartMessage).UseCulture(UserCulture);

                Log.Debug($"Processed START MSG (private) for {Message.Chat.Id}");
                await _appServices.BotControlService.SendTextMessageAsync(Message.Chat.Id,
                                                                          textToSend,
                                                                          Message.MessageThreadId,
                                                                          inlineMarkup: ExtensionMethods.MenuKeyboardMarkup(UserCulture),
                                                                          parseMode: Telegram.Bot.Types.Enums.ParseMode.Html,
                                                                          toLog: false);
            }
        }
        protected class PublicMessageUpdateController(IApplicationServices applicationServices, Message message) : BasicMessageUpdateController(applicationServices, message)
        {
        }
        protected class BasicMessageUpdateController
        {
            public readonly IApplicationServices _appServices;
            public readonly Message Message;
            public readonly CultureInfo UserCulture;

            public BasicMessageUpdateController(IApplicationServices applicationServices, Message message)
            {
                _appServices = applicationServices;
                Message = message;

                UserCulture = new CultureInfo("ru");
            }
            public async void ProcessCommand()
            {
                switch (Message.Type)
                {
                    case Telegram.Bot.Types.Enums.MessageType.Text:
                        ProcessText(Message.Chat.Id, Message.Text);
                        break;
                    case Telegram.Bot.Types.Enums.MessageType.Photo:
                        break;
                    case Telegram.Bot.Types.Enums.MessageType.Audio:
                        break;
                    case Telegram.Bot.Types.Enums.MessageType.Video:
                        break;
                    case Telegram.Bot.Types.Enums.MessageType.Voice:
                        break;
                    case Telegram.Bot.Types.Enums.MessageType.Document:
                        break;
                    case Telegram.Bot.Types.Enums.MessageType.Sticker:
                        break;
                    case Telegram.Bot.Types.Enums.MessageType.Location:
                        break;
                    case Telegram.Bot.Types.Enums.MessageType.Contact:
                        break;
                    case Telegram.Bot.Types.Enums.MessageType.Venue:
                        break;
                    case Telegram.Bot.Types.Enums.MessageType.Game:
                        break;
                    case Telegram.Bot.Types.Enums.MessageType.VideoNote:
                        break;
                    case Telegram.Bot.Types.Enums.MessageType.Invoice:
                        break;
                    case Telegram.Bot.Types.Enums.MessageType.SuccessfulPayment:
                        break;
                    case Telegram.Bot.Types.Enums.MessageType.WebsiteConnected:
                        break;
                    case Telegram.Bot.Types.Enums.MessageType.ChatMembersAdded:
                        break;
                    case Telegram.Bot.Types.Enums.MessageType.ChatMemberLeft:
                        break;
                    case Telegram.Bot.Types.Enums.MessageType.ChatTitleChanged:
                        break;
                    case Telegram.Bot.Types.Enums.MessageType.ChatPhotoChanged:
                        break;
                    case Telegram.Bot.Types.Enums.MessageType.MessagePinned:
                        break;
                    case Telegram.Bot.Types.Enums.MessageType.ChatPhotoDeleted:
                        break;
                    case Telegram.Bot.Types.Enums.MessageType.GroupCreated:
                        break;
                    case Telegram.Bot.Types.Enums.MessageType.SupergroupCreated:
                        break;
                    case Telegram.Bot.Types.Enums.MessageType.ChannelCreated:
                        break;
                    case Telegram.Bot.Types.Enums.MessageType.MigratedToSupergroup:
                        break;
                    case Telegram.Bot.Types.Enums.MessageType.MigratedFromGroup:
                        break;
                    case Telegram.Bot.Types.Enums.MessageType.Poll:
                        break;
                    case Telegram.Bot.Types.Enums.MessageType.Dice:
                        break;
                    case Telegram.Bot.Types.Enums.MessageType.MessageAutoDeleteTimerChanged:
                        break;
                    case Telegram.Bot.Types.Enums.MessageType.ProximityAlertTriggered:
                        break;
                    case Telegram.Bot.Types.Enums.MessageType.WebAppData:
                        break;
                    case Telegram.Bot.Types.Enums.MessageType.VideoChatScheduled:
                        break;
                    case Telegram.Bot.Types.Enums.MessageType.VideoChatStarted:
                        break;
                    case Telegram.Bot.Types.Enums.MessageType.VideoChatEnded:
                        break;
                    case Telegram.Bot.Types.Enums.MessageType.VideoChatParticipantsInvited:
                        break;
                    case Telegram.Bot.Types.Enums.MessageType.Animation:
                        break;
                    case Telegram.Bot.Types.Enums.MessageType.ForumTopicCreated:
                        break;
                    case Telegram.Bot.Types.Enums.MessageType.ForumTopicClosed:
                        break;
                    case Telegram.Bot.Types.Enums.MessageType.ForumTopicReopened:
                        break;
                    case Telegram.Bot.Types.Enums.MessageType.ForumTopicEdited:
                        break;
                    case Telegram.Bot.Types.Enums.MessageType.GeneralForumTopicHidden:
                        break;
                    case Telegram.Bot.Types.Enums.MessageType.GeneralForumTopicUnhidden:
                        break;
                    case Telegram.Bot.Types.Enums.MessageType.WriteAccessAllowed:
                        break;
                    case Telegram.Bot.Types.Enums.MessageType.UserShared:
                        break;
                    case Telegram.Bot.Types.Enums.MessageType.ChatShared:
                        break;

                    case Telegram.Bot.Types.Enums.MessageType.Unknown:
                    default:
                        break;
                }
            }
            public virtual async void ProcessText(long chatId, string rawMsgText)
            {
                if (string.IsNullOrEmpty(rawMsgText))
                    return;

                string msgText = rawMsgText.ToLower().Replace(Delimeters.BotCommandDelimeter, "");

                if (Commands.RandomJokeCommand == msgText || nameof(Resources.RandomJokeButton).UseCulture(UserCulture) == rawMsgText)
                {
                    await SendRandomJoke();
                }
                else if (Commands.TopJokeCommand == msgText || nameof(Resources.TopJokeButton).UseCulture(UserCulture) == rawMsgText)
                {
                    await SendTopJoke();
                }
                else if (Commands.RandomBezdnaCommand == msgText || nameof(Resources.RandomBezdnaButton).UseCulture(UserCulture) == rawMsgText)
                {
                    await SendRandomBezdna();
                }
                else if (msgText.Contains(Commands.StartCommand))
                {
                    await SendStartMessage();
                }
            }

            public virtual async Task SendStartMessage()
            {
                var textToSend = nameof(Resources.StartMessage).UseCulture(UserCulture);

                Log.Debug($"Processed START MSG for {Message.Chat.Id}");
                await _appServices.BotControlService.SendTextMessageAsync(Message.Chat.Id,
                                                                          textToSend,
                                                                          Message.MessageThreadId,
                                                                          parseMode: Telegram.Bot.Types.Enums.ParseMode.Html,
                                                                          toLog: false);
            }

            public virtual async Task SendTopJoke()
            {
                var textToSend = await _appServices.HttpService.GetTopJoke();
                if (textToSend == default)
                {
                    Log.Fatal($"Error on getting TOP joke: [ID: {Message.Chat.Id}| {Message.Text}]");
                    return;
                }
                Log.Debug($"Processed getting TOP joke for {Message.Chat.Id}");
                await _appServices.BotControlService.SendTextMessageAsync(Message.Chat.Id,
                                                                          textToSend,
                                                                          Message.MessageThreadId,
                                                                          parseMode: Telegram.Bot.Types.Enums.ParseMode.Html,
                                                                          toLog: false);
            }

            public virtual async Task<string?> SendRandomJoke()
            {
                var textToSend = await _appServices.HttpService.GetRandomJoke();
                if (textToSend == default)
                {
                    Log.Fatal($"Error on getting random joke: [ID: {Message.Chat.Id}| {Message.Text}]");
                    return default;
                }
                Log.Debug($"Processed getting random joke for {Message.Chat.Id}");
                await _appServices.BotControlService.SendTextMessageAsync(Message.Chat.Id, textToSend, Message.MessageThreadId, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html, toLog: false);
                return textToSend;
            }

            public virtual async Task<string?> SendRandomBezdna()
            {
                var textToSend = await _appServices.HttpService.GetRandomBezdna();
                if (textToSend == default)
                {
                    Log.Fatal($"Error on getting random bezdna: [ID: {Message.Chat.Id}| {Message.Text}]");
                    return default;
                }
                Log.Debug($"Processed getting random bezdna for {Message.Chat.Id}");
                await _appServices.BotControlService.SendTextMessageAsync(Message.Chat.Id, textToSend, Message.MessageThreadId, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html, toLog: false);
                return textToSend;
            }
        }
    }
}
