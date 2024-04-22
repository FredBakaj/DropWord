using DropWord.TgBot.Core.Factory;
using DropWord.TgBot.Core.Model;
using DropWord.TgBot.Core.Src.Controller;

namespace DropWord.TgBot.Core.Src.Middleware.Implementation
{
    /// <summary>
    /// Определяет объект для обработки сообщений пользователя
    /// </summary>
    public class RouteMiddleware : ABotMiddleware
    {
        private readonly ILogger<RouteMiddleware> _logger;
        private readonly IFactory<IBotController> _controllerFactory;

        public RouteMiddleware( ILogger<RouteMiddleware> logger, IFactory<IBotController> controllerFactory)
        {
            _logger = logger;
            _controllerFactory = controllerFactory;
        }

        public override async Task Next(UpdateBDto update)
        {
            var state = update.TelegramState!.State;
            var controller = await _controllerFactory.CreateAsync(state);
            await controller.Exec(update);

            await base.Next(update);
        }
    }
}
