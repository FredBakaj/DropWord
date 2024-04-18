using DropWord.TgBot.Core.Field.Controller;
using DropWord.TgBot.Core.Field.View;
using DropWord.TgBot.Core.Handler.BotStateTreeHandler;
using DropWord.TgBot.Core.Handler.BotStateTreeUserHandler;
using DropWord.TgBot.Core.Handler.BotViewHandler;
using DropWord.TgBot.Core.Manager.Info;
using DropWord.TgBot.Core.Model;
using DropWord.TgBot.Core.ViewDto;
using MediatR;

namespace DropWord.TgBot.Core.Src.Controller.Implementation;

public class StartController : IBotController
{
    private readonly IBotStateTreeHandler _botStateTreeHandler;
    private readonly IBotStateTreeUserHandler _botStateTreeUserHandler;
    private readonly IBotViewHandler _botViewHandler;
    private readonly ISender _sender;
    private readonly IInfoManager _infoManager;
    public string Name() => StartField.StartState;

    public StartController(
        IBotStateTreeHandler botStateTreeHandler,
        IBotStateTreeUserHandler botStateTreeUserHandler,
        IBotViewHandler botViewHandler,
        ISender sender,
        IInfoManager infoManager)
    {
        _botStateTreeHandler = botStateTreeHandler;
        _botStateTreeUserHandler = botStateTreeUserHandler;
        _botViewHandler = botViewHandler;
        _sender = sender;
        _infoManager = infoManager;

        Initialize();
    }

    private void Initialize()
    {
        _botStateTreeHandler.AddAction(StartField.StartAction, StartActionAsync);
        _botStateTreeHandler.AddAction(StartField.CompleteStartAction, OnCompleteStartActionAsync);
    }

    public async Task Exec(UpdateBDto update)
    {
        await _botStateTreeHandler.ExecuteRoute(update);
    }

    private async Task StartActionAsync(UpdateBDto update)
    {
        await _botViewHandler.SendAsync(StartViewField.Start, update);
        await _botStateTreeUserHandler.SetActionAsync(update, StartField.CompleteStartAction);
    }

    private async Task OnCompleteStartActionAsync(UpdateBDto update)
    {
        await _botStateTreeUserHandler.SetStateAndActionAsync(update, BaseField.BaseState,
            BaseField.BaseAction);
        var viewDto = new FirstShowMenuVDto() { Update = update, TutorialText = _infoManager.TutorialText };
        await _botViewHandler.SendAsync(StartViewField.FirstShowMenu, viewDto);
    }
}
