using DropWord.TgBot.Core.Field;
using DropWord.TgBot.Core.Field.Controller;
using DropWord.TgBot.Core.Handler.BotStateTreeUserHandler;
using DropWord.TgBot.Core.Model;
using MediatR;

namespace DropWord.TgBot.Core.Src.Command.Implementation;

public class StartCommand : IBotCommand
{
    private readonly ISender _sender;
    private readonly IBotStateTreeUserHandler _botStateTreeUserHandler;
    public string GetCommand() => CommandField.Start;
    public bool IsMoveNext() => true;

    public StartCommand(ISender sender, IBotStateTreeUserHandler botStateTreeUserHandler)
    {
        _sender = sender;
        _botStateTreeUserHandler = botStateTreeUserHandler;
    }

    public async Task Exec(UpdateBDto telegramUpdate)
    {
        await _botStateTreeUserHandler.SetStateAndActionAsync(telegramUpdate, StartField.StartState,
            StartField.StartAction);
    }
}
