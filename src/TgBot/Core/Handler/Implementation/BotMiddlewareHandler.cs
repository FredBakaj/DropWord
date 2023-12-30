using DropWord.TgBot.Core.Src.Middleware;
using DropWord.TgBot.Core.Src.Middleware.Implementation;
using DropWord.TgBot.Extension;

namespace DropWord.TgBot.Core.Handler.Implementation
{
    public class BotMiddlewareHandler : ABotMiddlewareHandler
    {
        private readonly IServiceProvider _serviceProvider;

        public BotMiddlewareHandler(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            Task.Run(Bind);
        }

        protected override async Task Bind()
        {
            //Последовательность вызовов AddMiddleware важна!
            //Реализует первичную инициализацию телеграмм аккаунта при запуске бота
            await AddMiddleware(_serviceProvider.GetSpecificService<IBotMiddleware, InitializationMiddleware>());
            // Обработчик команд (пример: /start)
            await AddMiddleware(_serviceProvider.GetSpecificService<IBotMiddleware, CommandMiddleware>());
            // Определяет в каком сейчас стейте находиться пользователь
            await AddMiddleware(_serviceProvider.GetSpecificService<IBotMiddleware, StateMiddleware>());
            // Определяет в какой обект передать обработку сообщения от пользователя
            // в зависимости от состояния
            await AddMiddleware(_serviceProvider.GetSpecificService<IBotMiddleware, RouteMiddleware>());
        }
    }
}
