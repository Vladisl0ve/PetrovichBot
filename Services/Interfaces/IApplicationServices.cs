namespace PetrovichBot.Services.Interfaces
{
    public interface IApplicationServices
    {
        BotControlService BotControlService { get; }
        HttpService       HttpService       { get; }
    }
}
