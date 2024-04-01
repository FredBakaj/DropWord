using DropWord.TgBot.Core.Model;

namespace DropWord.TgBot.Core.Handler.MiddlewareHandler
{
    /// <summary>
    /// Формування ланцюжка обовязків (патерн)
    /// </summary>
    public interface IBotMiddlewareHandler
    {
        /// <summary>
        /// Запуск ланцюжка обовязків
        /// Буде запущено перший ланцюжок який добавлено в MainConstructor
        /// </summary>
        Task Run(UpdateBDto update);
    }
}
