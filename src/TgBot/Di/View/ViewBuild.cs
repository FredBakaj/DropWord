using DropWord.TgBot.Core.Src.View;
using DropWord.TgBot.Core.Src.View.Implementation;

namespace DropWord.TgBot.Di.View;

public class ViewBuild
{
    public static void BuildService(IServiceCollection services)
    {
        services.AddTransient<IBotView, BaseBotView>();
        services.AddTransient<IBotView, StartBotView>();
        services.AddTransient<IBotView, SentencesRepetitionByInputBotView>();
        services.AddTransient<IBotView, SettingsBotView>();
        services.AddTransient<IBotView, RepeatForDayBotView>();
    }
}
