using DropWord.TgBot.Core.Handler;
using DropWord.TgBot.Core.Handler.Implementation;
using DropWord.TgBot.Core.Src.Controller;
using DropWord.TgBot.Core.Src.Controller.Implementation;

namespace DropWord.TgBot.Di.Controller
{
    public class ControllerBuild
    {
        public static void BuildService(IServiceCollection services)
        {
            services.AddTransient<IBotStateTreeHandler, BotStateTreeHandler>();
            services.AddTransient<IBotController, BaseController>();
        }
    }
}
