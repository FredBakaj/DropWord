using DropWord.TgBot.Core.Handler.BotStateTreeHandler;
using DropWord.TgBot.Core.Handler.BotStateTreeHandler.Implementation;
using DropWord.TgBot.Core.Handler.BotStateTreeUserHandler;
using DropWord.TgBot.Core.Handler.BotStateTreeUserHandler.Implementation;
using DropWord.TgBot.Core.Handler.BotViewHandler;
using DropWord.TgBot.Core.Handler.BotViewHandler.Implementation;
using DropWord.TgBot.Core.Handler.MiddlewareHandler;
using DropWord.TgBot.Core.Handler.MiddlewareHandler.Implementation;

namespace DropWord.TgBot.Di.Handler;

public class HandlerBuild
{
    public static void BuildService(IServiceCollection services)
    {
        services.AddScoped<BotStateTreeHandler>();
        services.AddScoped<IBotStateTreeHandler, BotStateTreeHandlerAnalyticDecorator>();
        
        services.AddScoped<IBotMiddlewareHandler, BotMiddlewareHandler>();
        services.AddScoped<IBotViewHandler, BotViewHandler>();
        services.AddScoped<IBotStateTreeUserHandler, BotStateTreeUserHandler>();
    }
}
