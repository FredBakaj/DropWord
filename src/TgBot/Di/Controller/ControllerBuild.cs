using DropWord.TgBot.Core.Src.Controller;
using DropWord.TgBot.Core.Src.Controller.Implementation;

namespace DropWord.TgBot.Di.Controller
{
    public class ControllerBuild
    {
        public static void BuildService(IServiceCollection services)
        {
            services.AddScoped<IBotController, BaseController>();
            services.AddScoped<IBotController, StartController>();
            services.AddScoped<IBotController, SentencesRepetitionByInputController>();
            services.AddScoped<IBotController, RepeatForDayController>();
            services.AddScoped<IBotController, SmallTalkChatController>();
        }
    }
}
