using DropWord.TgBot.Core.Model;

namespace DropWord.TgBot.Core.Src.Middleware
{
    /// <summary>
    /// Реалізація патерна ланцюжка обовязків
    /// </summary>
    public interface IBotMiddleware
    {
        /// <summary>
        /// Виклик наступного елемента ланцюжка обовязків
        /// </summary>
        public Task Next(UpdateBDto update);
        public Task SetNext(IBotMiddleware next);
    }
}
