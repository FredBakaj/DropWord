namespace DropWord.TgBot.Core.Handler.TaskProcessingHandler;

public interface IBackgroundTaskHandler
{
    Task StartProcessAsync<T>(long userId, string nameProcess, Func<T, CancellationToken, Task>  func, T model)
        where T : class;
    Task<bool> IsProcessRunningAsync(long userId, string nameProcess);
    Task StopProcessAsync(long userId, string nameProcess);
}
