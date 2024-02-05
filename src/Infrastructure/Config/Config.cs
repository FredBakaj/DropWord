using DropWord.Application.Common.Interfaces;
using DropWord.Infrastructure.Config.Exception;
using Microsoft.Extensions.Configuration;

namespace DropWord.Infrastructure.Config;

public class Config : IConfig
{
    private readonly Dictionary<string, string> Configuration = new();

    public Config(IConfiguration configuration)
    {
        var appSection = configuration.GetSection("ApplicationSettings");
        var keys = appSection.GetChildren().Select(x => x.Key);

        foreach (var key in keys)
        {
            Configuration.Add(key, appSection[key]!);
        }
    }

    public string GetValue(string key)
    {
        if (Configuration.ContainsKey(key))
        {
            return Configuration[key];
        }

        throw new ConfigNotHaveKayException($"container with configuration not have a key {key}");
    }
}
