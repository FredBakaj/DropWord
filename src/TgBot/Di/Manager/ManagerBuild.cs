using DropWord.Application.Common.Interfaces;
using DropWord.Application.Common.Interfaces.Sentence;
using DropWord.Application.Manager.Sentence;
using DropWord.Application.Manager.Sentence.Implementation;
using DropWord.Infrastructure.Config;
using DropWord.Infrastructure.Sentence;
using DropWord.TgBot.Core.Manager.RepeatSentence;
using DropWord.TgBot.Core.Manager.RepeatSentence.Implementation;

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
        services.AddScoped<ISentenceManager, SentenceManager>();
        
        
    }
}