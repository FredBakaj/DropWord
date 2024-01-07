using DropWord.TgBot.Core.Handler;
using DropWord.TgBot.Core.Handler.Implementation;

namespace DropWord.TgBot.Di.Handler;

public class HandlerBuild
{
    public static void BuildService(IServiceCollection services)
    {
        services.AddScoped<IBotStateTreeHandler, BotStateTreeHandler>();
        services.AddScoped<IBotMiddlewareHandler, BotMiddlewareHandler>();
        services.AddScoped<IBotViewHandler, BotViewHandler>();
        services.AddScoped<IBotStateTreeUserHandler, BotStateTreeUserHandler>();
    }
}
