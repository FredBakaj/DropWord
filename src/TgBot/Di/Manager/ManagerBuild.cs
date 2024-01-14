using DropWord.Application.Common.Interfaces.Sentence;
using DropWord.Infrastructure.Sentence;

namespace DropWord.TgBot.Di.Manager;

public class ManagerBuild
{
    public static void BuildService(IServiceCollection services)
    {
        services.AddTransient<IParse, Parse>();
        services.AddTransient<ITranslate, Translate>();
        
    }
}