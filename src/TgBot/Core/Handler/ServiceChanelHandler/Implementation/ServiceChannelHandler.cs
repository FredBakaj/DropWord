using System.Threading.Channels;

namespace DropWord.TgBot.Core.Handler.ServiceChanelHandler.Implementation;

public class ServiceChannelHandler : IServiceChannelReaderHandler, IServiceChannelSenderHandler
{
    public Dictionary<Type, Channel<Object>> _channel;

    public ServiceChannelHandler()
    {
        _channel = new Dictionary<Type, Channel<Object>>();
    }

    public async Task<T> ReadSignalAsync<T>() where T : class
    {
        while (!_channel.ContainsKey(typeof(T)))
        {
            await Task.Delay(100); // Wait for new signal
        }

        if (_channel.TryGetValue(typeof(T), out var channel))
        {
            return (await _channel[typeof(T)].Reader.ReadAsync() as T)!;
        }

        throw new InvalidOperationException("Not have value in dictionary channel");
    }

    public async Task SendSignalAsync<T>(T data) where T : class
    {
        if (!_channel.TryGetValue(typeof(T), out var channel))
        {
            _channel[typeof(T)] = Channel.CreateUnbounded<object>();
        }

        await _channel[typeof(T)].Writer.WriteAsync(data);
    }
}
