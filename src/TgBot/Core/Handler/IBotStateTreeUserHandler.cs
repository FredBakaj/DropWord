using DropWord.TgBot.Core.Model;

namespace DropWord.TgBot.Core.Handler;

public interface IBotStateTreeUserHandler
{
    Task SetStateAndActionAsync(UpdateBDto update, string state, string action, CancellationToken cancellationToken = default);
    Task SetDataAndActionAsync<T>(UpdateBDto update, string action, T data, CancellationToken cancellationToken = default) where T : class;
    Task SetActionAsync(UpdateBDto update, string action, CancellationToken cancellationToken = default);
    Task<T?> GetDataAsync<T>(UpdateBDto update, CancellationToken cancellationToken = default) where T : class;
    Task ClearDataAsync(UpdateBDto update, CancellationToken cancellationToken = default);
    Task<StateTreeBDto> GetStateAndActionAsync(UpdateBDto update, CancellationToken cancellationToken = default);
}
