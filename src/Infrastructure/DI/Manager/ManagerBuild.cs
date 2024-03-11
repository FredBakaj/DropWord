using DropWord.Application.Common.Interfaces;
using DropWord.Application.Common.Interfaces.Sentence;
using DropWord.Infrastructure.Sentence;
using DropWord.Infrastructure.Utils.RestApiClient;
using DropWord.Infrastructure.Utils.RestApiClient.Implementation;
using Microsoft.Extensions.DependencyInjection;

namespace DropWord.Infrastructure.DI.Manager;

public class ManagerBuild
{
    public static void BuildService(IServiceCollection services)
    {
        services.AddScoped<IParse, Parse>();
        services.AddScoped<ITranslate, Translate>();
        services.AddScoped<IConfig, Config.Config>();
        services.AddScoped<IDifferenceSentence, DifferenceSentence>();
        services.AddScoped<IRestApiClient, RestApiClient>();
        
    }
}
