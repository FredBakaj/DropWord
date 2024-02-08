using DropWord.Application.UseCase.User.Queries.GetUser;
using DropWord.TgBot.Core.Extension;
using DropWord.TgBot.Core.Field;
using DropWord.TgBot.Core.Field.Controller;
using DropWord.TgBot.Core.Handler;
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
        var user = await _sender.Send(new GetUserQuery() { UserId = telegramUpdate.GetUserId() });

        if (user.UserSettings.LearnLanguage == String.Empty || user.UserSettings.MainLanguage == String.Empty)
        {
            await _botStateTreeUserHandler.SetStateAndActionAsync(telegramUpdate, StartField.StartState,
                StartField.StartAction);
        }
        else
        {
            await _botStateTreeUserHandler.SetStateAndActionAsync(telegramUpdate, BaseField.BaseState,
                BaseField.ReloadAction);
        }
    }
}
