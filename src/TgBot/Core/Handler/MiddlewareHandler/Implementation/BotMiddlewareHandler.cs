using DropWord.TgBot.Core.Src.Middleware;
using DropWord.TgBot.Core.Src.Middleware.Implementation;
using DropWord.TgBot.Extension;

namespace DropWord.TgBot.Core.Handler.MiddlewareHandler.Implementation
{
    public class BotMiddlewareHandler : ABotMiddlewareHandler
    {
        public BotMiddlewareHandler(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        protected override void Bind()
        {
            //Последовательность вызовов AddMiddleware важна!
            //Отключение всех чатов кромер OneToOne
            //AddMiddleware(_serviceProvider.GetSpecificService<IBotMiddleware, GroupDisableMiddleware>());
            //Спам блокер 
            //AddMiddleware(_serviceProvider.GetSpecificService<IBotMiddleware, SpamBlockerMiddleware>());
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
