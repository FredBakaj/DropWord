using DropWord.TgBot.Core.Src.Middleware;
using DropWord.TgBot.Core.Src.Middleware.Implementation;
using DropWord.TgBot.Extension;

namespace DropWord.TgBot.Core.Handler.Implementation
{
    public class BotMiddlewareHandler : ABotMiddlewareHandler
    {
        public BotMiddlewareHandler(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        protected override void Bind()
        {
            //Последовательность вызовов AddMiddleware важна!
            //Реализует первичную инициализацию телеграмм аккаунта при запуске бота
            AddMiddleware(_serviceProvider.GetSpecificService<IBotMiddleware, InitializationMiddleware>());
            // Обработчик команд (пример: /start)
            AddMiddleware(_serviceProvider.GetSpecificService<IBotMiddleware, CommandMiddleware>());
            // Определяет в каком сейчас стейте находиться пользователь
            AddMiddleware(_serviceProvider.GetSpecificService<IBotMiddleware, StateMiddleware>());
            // Определяет в какой обект передать обработку сообщения от пользователя
            // в зависимости от состояния
            AddMiddleware(_serviceProvider.GetSpecificService<IBotMiddleware, RouteMiddleware>());
        }
    }
}
