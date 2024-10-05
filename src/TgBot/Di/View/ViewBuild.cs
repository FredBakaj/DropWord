using DropWord.TgBot.Core.Src.View;
using DropWord.TgBot.Core.Src.View.Implementation;

namespace DropWord.TgBot.Di.View;

public class ViewBuild
{
    public static void BuildService(IServiceCollection services)
    {
        services.AddScoped<IBotView, BaseBotView>();
        services.AddScoped<IBotView, StartBotView>();
        services.AddScoped<IBotView, SentencesRepetitionByInputBotView>();
        services.AddScoped<IBotView, SettingsBotView>();
        services.AddScoped<IBotView, RepeatForDayBotView>();
        services.AddScoped<IBotView, SmallTalkChatBotView>();
    }
}
