using DropWord.TgBot.Core.Model;

namespace DropWord.TgBot.Core.Src.Middleware.Implementation;

/// <summary>
/// Базовый класс для создания миделвееров для обработки запроса пользователелем телеграмма
/// </summary>
public abstract class ABotMiddleware : IBotMiddleware
{
    protected IBotMiddleware _nextMiddleware = null!;

    public virtual async Task Next(UpdateBDto update)
    {
        if (_nextMiddleware != null)
        {
            await _nextMiddleware.Next(update);
        }
    }

    public virtual async Task SetNext(IBotMiddleware next)
    {
        await Task.Run(() => _nextMiddleware = next);
    }
}
