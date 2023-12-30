using DropWord.TgBot.Core.Src.View;

namespace DropWord.TgBot.Core.Handler.Implementation;

public class BotViewHandler : IBotViewHandler
{
    private readonly IServiceProvider _serviceProvider;
    private readonly Dictionary<string, Func<object[], Task>> ViewItemsMethods;

    public BotViewHandler(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;

        //Заполняет словарь всеми методами которые находяться в классах IViewItem и помечены атрибутом ViewAttribute
        ViewItemsMethods = new Dictionary<string, Func<object[], Task>>();
        var viewItems = _serviceProvider.GetServices<IBotView>();
        foreach (var item in viewItems)
        {
            var itemMethods = Task.Run(() => item.GetMethodsAsync()).Result;
            ViewItemsMethods = ViewItemsMethods.Concat(itemMethods).ToDictionary(pair => pair.Key, pair => pair.Value);
        }
    }


    public async Task SendAsync<T>(string view, T model)
    {
        await ViewItemsMethods[view].Invoke(new object[] { model ?? throw new ArgumentNullException(nameof(model)) });
    }
}
