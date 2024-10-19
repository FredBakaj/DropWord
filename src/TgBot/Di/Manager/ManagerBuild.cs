using DropWord.Application.Common.Interfaces;
using DropWord.Application.Common.Interfaces.Sentence;
using DropWord.Application.Common.Interfaces.SmallTalkChat;
using DropWord.Infrastructure.Config;
using DropWord.Infrastructure.Sentence;
using DropWord.Infrastructure.SmallTalkChat;
using DropWord.TgBot.Core.Manager.Analytics;
using DropWord.TgBot.Core.Manager.Analytics.Implementation;
using DropWord.TgBot.Core.Manager.Info;
using DropWord.TgBot.Core.Manager.Info.Implementation;
using DropWord.TgBot.Core.Manager.RepeatSentence;
using DropWord.TgBot.Core.Manager.RepeatSentence.Implementation;
using DropWord.TgBot.Core.Manager.Settings;
using DropWord.TgBot.Core.Manager.Settings.Implementation;
using DropWord.TgBot.Core.Manager.UserFilter;
using DropWord.TgBot.Core.Manager.UserFilter.Implementation;

namespace DropWord.TgBot.Di.Manager;

public class ManagerBuild
{
    public static void BuildService(IServiceCollection services)
    {
        services.AddScoped<IParse, Parse>();
        services.AddScoped<ITranslate, Translate>();
        services.AddScoped<IConfig, Config>();
        services.AddScoped<IDifferenceSentence, DifferenceSentence>();
        services.AddScoped<IRepeatSentenceManager, RepeatSentenceManager>();
        services.AddScoped<IMenuSettingsManager, MenuSettingsManager>();
        services.AddScoped<IAnalyticsManager, AnalyticsManager>();
        services.AddScoped<IInfoManager, InfoManager>();
        services.AddScoped<ISpamQueryManager, SpamQueryManager>();
        services.AddScoped<IResponseMessageGenerator, ResponseMessageGenerator>();
    }
}
