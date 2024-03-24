using PetrovichBot.Database;
using PetrovichBot.Extensions.Consts;
using PetrovichBot.Services;
using PetrovichBot.Services.Interfaces;
using Serilog;
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
        }
        protected class PublicMessageUpdateController(IApplicationServices applicationServices, Message message) : BasicMessageUpdateController(applicationServices, message)
        {
        }
        protected class BasicMessageUpdateController
        {
            public readonly IApplicationServices _appServices;
            public readonly Message _message;
            public BasicMessageUpdateController(IApplicationServices applicationServices, Message message)
            {
                _appServices = applicationServices;
                _message = message;
            }
            public async void ProcessCommand()
            {
                switch (_message.Type)
                {
                    case Telegram.Bot.Types.Enums.MessageType.Text:
                        ProcessText(_message.Chat.Id, _message.Text);
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
                string msgText = rawMsgText.ToLower().Replace(Delimeters.BotCommandDelimeter, "");

                if (Commands.RandomJokeCommand == msgText)
                {
                    await SendRandomJoke();
                }
                else if (Commands.TopJokeCommand == msgText)
                {
                    await SendTopJoke();
                }
            }

            public virtual async Task<string?> SendTopJoke()
            {
                var textToSend = await _appServices.HttpService.GetTopJoke();
                if (textToSend == default)
                {
                    Log.Fatal($"Error on getting TOP joke: [ID: {_message.Chat.Id}| {_message.Text}]");
                    return default;
                }
                Log.Debug($"Processed getting TOP joke for {_message.Chat.Id}");
                await _appServices.BotControlService.SendTextMessageAsync(_message.Chat.Id, textToSend, _message.MessageThreadId, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html, toLog: false);
                return textToSend;

            }
            public virtual async Task<string?> SendRandomJoke()
            {
                var textToSend = await _appServices.HttpService.GetRandomJoke();
                if (textToSend == default)
                {
                    Log.Fatal($"Error on getting random joke: [ID: {_message.Chat.Id}| {_message.Text}]");
                    return default;
                }
                Log.Debug($"Processed getting random joke for {_message.Chat.Id}");
                await _appServices.BotControlService.SendTextMessageAsync(_message.Chat.Id, textToSend, _message.MessageThreadId, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html, toLog: false);
                return textToSend;

            }
        }
    }
}
