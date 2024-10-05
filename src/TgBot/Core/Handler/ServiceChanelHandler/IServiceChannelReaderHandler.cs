namespace DropWord.TgBot.Core.Handler.ServiceChanelHandler;

public interface IServiceChannelReaderHandler
{
    Task<T> ReadSignalAsync<T>() where T : class;
}
