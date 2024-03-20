using DropWord.TgBot.Core.ViewComponent;
using DropWord.TgBot.Core.ViewComponent.Implementation;

namespace DropWord.TgBot.Di.View;

public class ViewComponentBuild
{
    public static void BuildService(IServiceCollection services)
    {
        services.AddTransient<IMainMenuComponent, MainMenuComponent>();
        services.AddTransient<IDynamicButtonCallbackComponent, DynamicButtonCallbackComponent>();
    }
}
