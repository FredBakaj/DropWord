using DropWord.Application.Manager.Sentence;
using DropWord.Application.Manager.Sentence.Implementation;
using Microsoft.Extensions.DependencyInjection;

namespace DropWord.Application.DI.Manager;

public class ManagerBuild
{
    public static void BuildService(IServiceCollection services)
    {
        services.AddScoped<ISentenceManager, SentenceManager>();
    }
}
