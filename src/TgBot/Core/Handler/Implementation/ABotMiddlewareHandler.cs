using DropWord.TgBot.Core.Model;
using DropWord.TgBot.Core.Src.Middleware;

namespace DropWord.TgBot.Core.Handler.Implementation
{
    public abstract class ABotMiddlewareHandler : IBotMiddlewareHandler
    {
        private IBotMiddleware _lastMiddleware = null!;
        private IBotMiddleware _firstMiddleware = null!;
        
        /// <summary>
        /// Добавлення ланцюга обовязків
        /// </summary>
        protected async Task AddMiddleware(IBotMiddleware middlewate)
        {
            if (_lastMiddleware == null)
            {
                _lastMiddleware = middlewate;
                _firstMiddleware = middlewate;
            }
            else
            {
                await _lastMiddleware.SetNext(middlewate);
                _lastMiddleware = middlewate;
            }
        }

        public async Task Run(UpdateBDto update)
        {
            await _firstMiddleware.Next(update);
        }

        /// <summary>
        /// Формує ланцюг обовязків
        /// </summary>
        protected abstract Task Bind();
    }
}
