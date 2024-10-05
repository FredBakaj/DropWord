namespace DropWord.TgBot.Core.Handler.ServiceChanelHandler;

public interface IServiceChannelSenderHandler
{

    Task SendSignalAsync<T>(T data) where T : class;

}
