namespace DropWord.TgBot.Core.Handler.TaskProcessingHandler.Implementation;

public class BackgroundTaskHandler : IBackgroundTaskHandler
{
    private readonly Dictionary<string, Dictionary<long, (Task Task, CancellationTokenSource CancellationTokenSource)>>
        _taskDictionary;

    public BackgroundTaskHandler()
    {
        _taskDictionary =
            new Dictionary<string, Dictionary<long, (Task Task, CancellationTokenSource CancellationTokenSource)>>();
    }

    public Task StartProcessAsync<T>(long userId, string nameProcess,
        Func<T, IServiceScope, CancellationToken, Task> func, T model,
        IServiceScope serviceScope) where T : class
    {
        var cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = cancellationTokenSource.Token;

        // Создаём и запускаем задачу асинхронной обработки
        var task = Task.Run(() => func(model, serviceScope, cancellationToken),
            cancellationToken);

        // Сохраняем задачу и её CancellationTokenSource в словарь
        lock (_taskDictionary)
        {
            if (!_taskDictionary.ContainsKey(nameProcess))
            {
                _taskDictionary.Add(nameProcess,
                    new Dictionary<long, (Task Task, CancellationTokenSource CancellationTokenSource)>());
            }

            _taskDictionary[nameProcess][userId] = (task, cancellationTokenSource);
        }

        return Task.CompletedTask;
    }

    public async Task<bool> IsProcessRunningAsync(long userId, string nameProcess)
    {
        if (_taskDictionary.ContainsKey(nameProcess) && _taskDictionary[nameProcess].ContainsKey(userId))
        {
            var (task, cancellationTokenSource) = _taskDictionary[nameProcess][userId];
            if (!cancellationTokenSource.IsCancellationRequested && !task.IsCanceled && !task.IsCompleted &&
                !task.IsFaulted)
                return await Task.FromResult(true);
        }

        return await Task.FromResult(false);
    }

    public Task StopProcessAsync(long userId, string nameProcess)
    {
        if (!_taskDictionary.ContainsKey(nameProcess))
        {
            throw new NullReferenceException("not have a process in the dictionary");
        }

        if (!_taskDictionary[nameProcess].ContainsKey(userId))
        {
            throw new NullReferenceException("not have a user id in the process");
        }

        lock (_taskDictionary[nameProcess])
        {
            var (task, cancellationTokenSource) = _taskDictionary[nameProcess][userId];
            cancellationTokenSource.CancelAsync();
            _taskDictionary[nameProcess].Remove(userId);
        }

        return Task.CompletedTask;
    }
}
