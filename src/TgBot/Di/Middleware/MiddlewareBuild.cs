using DropWord.TgBot.Core.Handler;
using DropWord.TgBot.Core.Handler.Implementation;
using DropWord.TgBot.Core.Src.Middleware;
using DropWord.TgBot.Core.Src.Middleware.Implementation;

namespace DropWord.TgBot.Di.Middleware
{
    public class MiddlewareBuild
    {
        public static void BuildService(IServiceCollection services)
        {
            services.AddTransient<IBotMiddlewareHandler, BotMiddlewareHandler>();

            services.AddTransient<IBotMiddleware, InitializationMiddleware>();
            services.AddTransient<IBotMiddleware, CommandMiddleware>();
            services.AddTransient<IBotMiddleware, StateMiddleware>();
            services.AddTransient<IBotMiddleware, RouteMiddleware>();
        }
    }
}
