using DropWord.TgBot.Core.Handler.ServiceChanelHandler;
using DropWord.TgBot.Core.Service.Channel.SmallTalkChat;
using Telegram.Bot;

namespace DropWord.TgBot.Core.Service.Implementation;

public class GenerateReplyToUserMessageService : BackgroundService
{
    private readonly IServiceChannelReaderHandler _serviceChannelReaderHandler;
    private readonly ITelegramBotClient _botClient;
    private readonly Dictionary<long, (Task Task, CancellationTokenSource CancellationTokenSource)> _taskDictionary;


    public GenerateReplyToUserMessageService(IServiceChannelReaderHandler serviceChannelReaderHandler,
        ITelegramBotClient botClient)
    {
        _serviceChannelReaderHandler = serviceChannelReaderHandler;
        _botClient = botClient;
        _taskDictionary = new Dictionary<long, (Task, CancellationTokenSource)>();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            // тормозит цикл, если небыло сигнала
            GenerateReplyToUserMessageChannel chanelData =
                await _serviceChannelReaderHandler.ReadSignalAsync<GenerateReplyToUserMessageChannel>();
            
            lock (_taskDictionary)
            {
                foreach (var taskItem in _taskDictionary)
                {
                    if (taskItem.Value.Task.IsCanceled
                        || taskItem.Value.Task.IsCompleted
                        || taskItem.Value.Task.IsFaulted
                        || _taskDictionary.ContainsKey(chanelData.UserId))
                    {
                        _taskDictionary[chanelData.UserId].CancellationTokenSource.CancelAsync();
                        _taskDictionary.Remove(taskItem.Key); // Удаляем завершённую задачу из словаря
                    }
                }
            }

            // Создаём CancellationTokenSource для этой задачи
            var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;

            // Создаём и запускаем задачу асинхронной обработки
            var task = Task.Run(() => ProcessReplyToUserMessageAsync(chanelData, cancellationToken),
                cancellationToken);

            // Сохраняем задачу и её CancellationTokenSource в словарь
            lock (_taskDictionary)
            {
                _taskDictionary[chanelData.UserId] = (task, cancellationTokenSource);
            }
            
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
