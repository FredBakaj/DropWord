using DropWord.TgBot.Core.Extension;
using DropWord.TgBot.Core.Field.Controller;
using DropWord.TgBot.Core.Field.View;
using DropWord.TgBot.Core.Handler.BotStateTreeHandler;
using DropWord.TgBot.Core.Handler.BotStateTreeUserHandler;
using DropWord.TgBot.Core.Handler.BotViewHandler;
using DropWord.TgBot.Core.Handler.NotificationHandler.Notification.Implementation.SmallTalkChat;
using DropWord.TgBot.Core.Model;
using MediatR;

namespace DropWord.TgBot.Core.Src.Controller.Implementation;

public class SmallTalkChatController : IBotController
{
    private readonly IBotStateTreeHandler _botStateTreeHandler;
    private readonly IBotViewHandler _botViewHandler;
    private readonly ISender _sender;
    private readonly IPublisher _mediator;
    private readonly IBotStateTreeUserHandler _botStateTreeUserHandler;
    public string Name() => SmallTalkChatField.SmallTalkChatState;

    public SmallTalkChatController(IBotStateTreeHandler botStateTreeHandler, IBotViewHandler botViewHandler,
        ISender sender,
        IPublisher mediator,
        IBotStateTreeUserHandler botStateTreeUserHandler)
    {
        _botStateTreeHandler = botStateTreeHandler;
        _botViewHandler = botViewHandler;
        _sender = sender;
        _mediator = mediator;
        _botStateTreeUserHandler = botStateTreeUserHandler;

        Initialize();
    }


    public async Task Exec(UpdateBDto update)
    {
        await _botStateTreeHandler.ExecuteRoute(update);
    }

    private void Initialize()
    {
        _botStateTreeHandler.AddAction(SmallTalkChatField.SmallTalkChatAction, OnAutoChatAction);
        _botStateTreeHandler.AddKeyboard(SmallTalkChatField.SmallTalkChatAction, SmallTalkChatField.BackKeyboard,
            OnCancelKeyboard);
        _botStateTreeHandler.AddKeyboard(SmallTalkChatField.SmallTalkChatAction,
            SmallTalkChatField.SearchNewUserKeyboard,
            OnSearchNewUserKeyboard);
    }

    private async Task OnAutoChatAction(UpdateBDto updateBDto)
    {
        // await Task.Run(() => Console.WriteLine(updateBDto.GetMessage().Text));
        await _mediator.Publish(
            
            new UserSendMessageEvent()
            {
                UserId = updateBDto.GetUserId(), Message = updateBDto.GetMessage().Text!
            });
        
    }

    private async Task OnCancelKeyboard(UpdateBDto updateBDto)
    {
        await _botStateTreeUserHandler.SetStateAndActionAsync(updateBDto, BaseField.BaseState, BaseField.BaseAction);
        await _botViewHandler.SendAsync(BaseViewField.Menu, updateBDto);
    }

    private async Task OnSearchNewUserKeyboard(UpdateBDto updateBDto)
    {
        await _botViewHandler.SendAsync(SmallTalkChatViewField.SearchNewUserKeyboard, updateBDto);
    }
}
