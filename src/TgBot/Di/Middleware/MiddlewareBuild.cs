using DropWord.TgBot.Core.Src.Middleware;
using DropWord.TgBot.Core.Src.Middleware.Implementation;

namespace DropWord.TgBot.Di.Middleware
{
    public class MiddlewareBuild
    {
        public static void BuildService(IServiceCollection services)
        {
            services.AddScoped<IBotMiddleware, GroupDisableMiddleware>();
            services.AddScoped<IBotMiddleware, SpamBlockerMiddleware>();
            services.AddScoped<IBotMiddleware, InitializationMiddleware>();
            services.AddScoped<IBotMiddleware, CommandMiddleware>();
            services.AddScoped<IBotMiddleware, StateMiddleware>();
            services.AddScoped<IBotMiddleware, RouteMiddleware>();
        }
    }
}
