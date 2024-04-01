using DropWord.TgBot.Core.Extension;
using DropWord.TgBot.Core.Field.Analytics;
using DropWord.TgBot.Core.Manager.Analytics;
using DropWord.TgBot.Core.Model;

namespace DropWord.TgBot.Core.Handler.BotStateTreeHandler.Implementation;

public class BotStateTreeHandlerAnalyticDecorator : IBotStateTreeHandler
{
    private readonly BotStateTreeHandler _botStateTreeHandler;
    private readonly IAnalyticsManager _analyticsManager;

    public BotStateTreeHandlerAnalyticDecorator(BotStateTreeHandler botStateTreeHandler,
        IAnalyticsManager analyticsManager)
    {
        _botStateTreeHandler = botStateTreeHandler;
        _analyticsManager = analyticsManager;
    }

    public async Task ExecuteRoute(UpdateBDto updateBDto)
    {
        await _botStateTreeHandler.ExecuteRoute(updateBDto);
    }

    public void AddAction(string action, Func<UpdateBDto, Task> func)
    {
        Func<UpdateBDto, Task> funcDecor = new Func<UpdateBDto, Task>(async updateBDto =>
        {
            await _analyticsManager.SendUserActionAsync(updateBDto.GetUserId(), TypeActionField.Action, action,
                updateBDto.GetMessage().Text);
            
            await func(updateBDto);
        });

        _botStateTreeHandler.AddAction(action, funcDecor);
    }

    public void AddCallback(string action, string callback, Func<UpdateBDto, Task> func)
    {
        Func<UpdateBDto, Task> funcDecor = new Func<UpdateBDto, Task>(async updateBDto =>
        {
            await _analyticsManager.SendUserActionAsync(updateBDto.GetUserId(), TypeActionField.Callback, callback,
                updateBDto.CallbackData);
            
            await func(updateBDto);
        });

        _botStateTreeHandler.AddCallback(action, callback, funcDecor);
    }

    public void AddKeyboard(string action, string keyboard, Func<UpdateBDto, Task> func)
    {
        Func<UpdateBDto, Task> funcDecor = new Func<UpdateBDto, Task>(async updateBDto =>
        {
            await _analyticsManager.SendUserActionAsync(updateBDto.GetUserId(), TypeActionField.Keyboard, keyboard,
                null);
            
            await func(updateBDto);
        });

        _botStateTreeHandler.AddKeyboard(action, keyboard, funcDecor);
    }
}
