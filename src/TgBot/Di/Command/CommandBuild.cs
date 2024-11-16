using DropWord.TgBot.Core.Src.Command;
using DropWord.TgBot.Core.Src.Command.Implementation;

namespace DropWord.TgBot.Di.Command;

public class CommandBuild
{
    public static void BuildService(IServiceCollection services)
    {
        services.AddScoped<IBotCommand, StartCommand>();
        services.AddScoped<IBotCommand, ReloadCommand>();
        services.AddScoped<IBotCommand, HelpCommand>();
        services.AddScoped<IBotCommand, HelpChatCommand>();
        services.AddScoped<IBotCommand, HelpNewSentencesCommand>();
        services.AddScoped<IBotCommand, HelpRepeatSentencesCommand>();
        services.AddScoped<IBotCommand, HelpSettingsCommand>();
        services.AddScoped<IBotCommand, CountUsersCommand>();
    }
}
