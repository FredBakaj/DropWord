using DropWord.Application.Factory.Sentence;
using DropWord.Application.Factory.Sentence.Implementation;
using Microsoft.Extensions.DependencyInjection;

namespace DropWord.Application.DI.Factory;

public class FactoryBuild
{
    public static void BuildService(IServiceCollection services)
    {
        services.AddTransient<ISentencesFactory, SentencesFactory>();
    }
}
