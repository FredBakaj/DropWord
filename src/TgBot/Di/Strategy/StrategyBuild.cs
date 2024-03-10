using DropWord.Application.Strategy.SentencesForRepeat;
using DropWord.Application.Strategy.SentencesForRepeat.Implementation;

namespace DropWord.TgBot.Di.Strategy;

public class StrategyBuild 
{
    public static void BuildService(IServiceCollection services)
    {
        //Application
        services.AddTransient<ISentencesForRepeatStrategy, SentencesForRepeatStepByQueue>();
        services.AddTransient<ISentencesForRepeatStrategy, SentencesForRepeatOldDataMinCount>();

    }
}
