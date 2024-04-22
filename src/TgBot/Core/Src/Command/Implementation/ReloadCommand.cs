using DropWord.TgBot.Core.Extension;
using DropWord.TgBot.Core.Field;
using DropWord.TgBot.Core.Field.Controller;
using DropWord.TgBot.Core.Handler.BotStateTreeUserHandler;
using DropWord.TgBot.Core.Manager.Info;
using DropWord.TgBot.Core.Model;
using Telegram.Bot;

namespace DropWord.TgBot.Core.Src.Command.Implementation;

public class ReloadCommand : IBotCommand
{
    private readonly ITelegramBotClient _client;
    private readonly IBotStateTreeUserHandler _botStateTreeUserHandler;
    private readonly IInfoManager _infoManager;
    public string GetCommand() => CommandField.Reload;

    public bool IsMoveNext() => true;

    public ReloadCommand(ITelegramBotClient client,
        IBotStateTreeUserHandler botStateTreeUserHandler,
        IInfoManager infoManager)
    {
        _client = client;
        _botStateTreeUserHandler = botStateTreeUserHandler;
        _infoManager = infoManager;
    }

    public async Task Exec(UpdateBDto update)
    {
        await _botStateTreeUserHandler.SetStateAndActionAsync(update, BaseField.BaseState, BaseField.ReloadAction,
            CancellationToken.None);
        await _client.SendTextMessageAsync(update.GetUserId(), "бот перезавантажен)");
        await _infoManager.SendBotCommandToUserAsync();
    }
}
