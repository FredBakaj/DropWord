using DropWord.TgBot.Core.Src.Command;
using DropWord.TgBot.Core.Src.Command.Implementation;

namespace DropWord.TgBot.Di.Command;

public class CommandBuild
{
    public static void BuildService(IServiceCollection services)
    {
        services.AddScoped<IBotCommand, StartCommand>();
    }
}