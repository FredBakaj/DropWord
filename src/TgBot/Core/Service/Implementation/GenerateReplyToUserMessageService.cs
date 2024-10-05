using DropWord.TgBot.Core.Handler.ServiceChanelHandler;
using DropWord.TgBot.Core.Handler.TaskProcessingHandler;
using DropWord.TgBot.Core.Service.Channel.SmallTalkChat;
using Telegram.Bot;

namespace DropWord.TgBot.Core.Service.Implementation;

public class GenerateReplyToUserMessageService : BackgroundService
{
    private readonly IServiceChannelReaderHandler _serviceChannelReaderHandler;
    private readonly ITelegramBotClient _botClient;
    private readonly IBackgroundTaskHandler _backgroundTaskHandler;


    public GenerateReplyToUserMessageService(IServiceChannelReaderHandler serviceChannelReaderHandler,
        ITelegramBotClient botClient, IBackgroundTaskHandler backgroundTaskHandler)
    {
        _serviceChannelReaderHandler = serviceChannelReaderHandler;
        _botClient = botClient;
        _backgroundTaskHandler = backgroundTaskHandler;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            // тормозит цикл, если небыло сигнала
            GenerateReplyToUserMessageChannel chanelData =
                await _serviceChannelReaderHandler.ReadSignalAsync<GenerateReplyToUserMessageChannel>();

            if (await _backgroundTaskHandler.IsProcessRunningAsync(chanelData.UserId, "GenerateReplyToUserMessage"))
            {
                await _backgroundTaskHandler.StopProcessAsync(chanelData.UserId, "GenerateReplyToUserMessage");
            }

            await _backgroundTaskHandler.StartProcessAsync(chanelData.UserId, "GenerateReplyToUserMessage",
                ProcessReplyToUserMessageAsync, chanelData);

            await Task.Delay(100, stoppingToken); // Ждём перед следующей итерацией
        }
    }

    private async Task ProcessReplyToUserMessageAsync(GenerateReplyToUserMessageChannel chanelData,
        CancellationToken cancellationToken)
    {
        await Task.Delay(10000);
        cancellationToken.ThrowIfCancellationRequested();
        await _botClient.SendTextMessageAsync(chanelData.UserId, chanelData.Message);
    }
}
