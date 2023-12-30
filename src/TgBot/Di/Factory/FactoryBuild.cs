using DropWord.TgBot.Core.Factory;
using DropWord.TgBot.Core.Factory.Implementation;
using DropWord.TgBot.Core.Src.Command;
using DropWord.TgBot.Core.Src.Controller;

namespace DropWord.TgBot.Di.Factory;

public class FactoryBuild
{
    public static void BuildService(IServiceCollection services)
    {
        services.AddTransient<IFactory<IBotController>, ControllerFactory>();
        services.AddTransient<IFactory<IBotCommand>, CommandFactory>();
    }
}
