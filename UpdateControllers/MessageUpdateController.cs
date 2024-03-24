using PetrovichBot.Services.Interfaces;
using Serilog;
using Telegram.Bot.Types;

namespace PetrovichBot.UpdateControllers
{
    internal class MessageUpdateController : UpdateController
    {
        public MessageUpdateController(IApplicationServices applicationServices, Message message) : base (applicationServices)
        {
            if (message.Chat.Id > 0)
                new PrivateMessageUpdateController(applicationServices, message).ProcessCommand();
            else
                new PublicMessageUpdateController(applicationServices, message).ProcessCommand();
        }

        protected class PrivateMessageUpdateController(IApplicationServices applicationServices, Message message) : BasicMessageUpdateController(applicationServices, message)
        {
            public override async Task<string> ProcessText(long chatId, string msgText)
            {
                var textToSend = await base.ProcessText(chatId, msgText);
                Log.Debug($"Processed private text message from {chatId}");
                await _appServices.BotControlService.SendTextMessageAsync(chatId, textToSend, _message.MessageThreadId, toLog: false);

                return textToSend;
            }
        }
        protected class PublicMessageUpdateController(IApplicationServices applicationServices, Message message) : BasicMessageUpdateController(applicationServices, message)
        {
            public override async Task<string> ProcessText(long chatId, string msgText)
            {
                var textToSend = await base.ProcessText(chatId, msgText);
                Log.Debug($"Processed public text message from {chatId}");
                await _appServices.BotControlService.SendTextMessageAsync(chatId, textToSend, _message.MessageThreadId, toLog: false);

                return textToSend;
            }
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
                        await ProcessText(_message.Chat.Id, _message.Text);
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
            public virtual async Task<string> ProcessText(long chatId, string msgText)
            {
                return "Hello world!";
            }
        }
    }
}
