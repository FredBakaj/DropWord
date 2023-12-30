using DropWord.TgBot.Core.Handler;
using DropWord.TgBot.Core.Handler.Implementation;
using DropWord.TgBot.Core.Src.View;
using DropWord.TgBot.Core.Src.View.Implementation;

namespace DropWord.TgBot.Di.View;

public class ViewBuild
{
    public static void BuildService(IServiceCollection services)
    {
        
        services.AddTransient<IBotViewHandler, BotViewHandler>();
        // View
        services.AddTransient<IBotView, BaseBotView>();
    }
}
