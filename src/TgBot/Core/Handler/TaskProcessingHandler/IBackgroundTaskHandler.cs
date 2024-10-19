namespace DropWord.TgBot.Core.Handler.TaskProcessingHandler;

public interface IBackgroundTaskHandler
{
    Task StartProcessAsync<T>(long userId, string nameProcess, Func<T, IServiceScope, CancellationToken, Task> func,
        T model,
        IServiceScope serviceScope)
        where T : class;
    Task<bool> IsProcessRunningAsync(long userId, string nameProcess);
    Task StopProcessAsync(long userId, string nameProcess);
}
