using DropWord.TgBot.Core.Service.Implementation;

namespace DropWord.TgBot.Di.Serivce;

public class ServiceBuild
{
    public static void BuildService(IServiceCollection services)
    {
        services.AddHostedService<GenerateReplyToUserMessageService>();
    }
}
