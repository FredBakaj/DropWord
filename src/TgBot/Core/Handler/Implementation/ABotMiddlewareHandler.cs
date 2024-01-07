using DropWord.TgBot.Core.Model;
using DropWord.TgBot.Core.Src.Middleware;

namespace DropWord.TgBot.Core.Handler.Implementation
{
    public abstract class ABotMiddlewareHandler : IBotMiddlewareHandler
    {
        private IBotMiddleware _lastMiddleware = null!;
        private IBotMiddleware _firstMiddleware = null!;

        private ILogger<ABotMiddlewareHandler> _logger;
        protected readonly IServiceProvider _serviceProvider;

        public ABotMiddlewareHandler(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _logger = serviceProvider.GetService<ILogger<ABotMiddlewareHandler>>()!;

            Bind();
        }

        /// <summary>
        /// Добавлення ланцюга обовязків
        /// </summary>
        protected void AddMiddleware(IBotMiddleware middlewate)
        {
            if (_lastMiddleware == null)
            {
                _lastMiddleware = middlewate;
                _firstMiddleware = middlewate;
            }
            else
            {
                _lastMiddleware.SetNext(middlewate);
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
        protected abstract void Bind();
    }
}
