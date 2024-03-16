using DropWord.TgBot.Core.Src.Controller;
using DropWord.TgBot.Core.Src.Controller.Implementation;

namespace DropWord.TgBot.Di.Controller
{
    public class ControllerBuild
    {
        public static void BuildService(IServiceCollection services)
        {
            services.AddTransient<IBotController, BaseController>();
            services.AddTransient<IBotController, StartController>();
            services.AddTransient<IBotController, SentencesRepetitionByInputController>();
            services.AddTransient<IBotController, RepeatForDayController>();
        }
    }
}
