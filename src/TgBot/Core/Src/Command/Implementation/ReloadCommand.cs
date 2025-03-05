using DropWord.TgBot.Core.Extension;
using DropWord.TgBot.Core.Field;
using DropWord.TgBot.Core.Field.Controller;
using DropWord.TgBot.Core.Handler.BotStateTreeUserHandler;
using DropWord.TgBot.Core.Handler.TaskProcessingHandler;
using DropWord.TgBot.Core.Manager.Info;
using DropWord.TgBot.Core.Model;
using Telegram.Bot;

namespace DropWord.TgBot.Core.Src.Command.Implementation;

public class ReloadCommand : IBotCommand
{
    private readonly ITelegramBotClient _client;
    private readonly IBotStateTreeUserHandler _botStateTreeUserHandler;
    private readonly IInfoManager _infoManager;
    private readonly IBackgroundTaskHandler _backgroundTaskHandler;
    public string GetCommand() => CommandField.Reload;

    public bool IsMoveNext() => true;

    public ReloadCommand(ITelegramBotClient client,
        IBotStateTreeUserHandler botStateTreeUserHandler,
        IInfoManager infoManager,
        IBackgroundTaskHandler backgroundTaskHandler)
    {
        _client = client;
        _botStateTreeUserHandler = botStateTreeUserHandler;
        _infoManager = infoManager;
        _backgroundTaskHandler = backgroundTaskHandler;
    }

    public async Task Exec(UpdateBDto update)
    {
        // TODO Вынести в отдельный сервис, для закрытия который будет подписываться на события
        if (await _backgroundTaskHandler.IsProcessRunningAsync(update.GetUserId(),
                TaskProcessingField.SearchNewUserMessage))
            await _backgroundTaskHandler.StopProcessAsync(update.GetUserId(),
                TaskProcessingField.SearchNewUserMessage);
        
        if (await _backgroundTaskHandler.IsProcessRunningAsync(update.GetUserId(),
                TaskProcessingField.GenerateReplyToUserMessage))
            await _backgroundTaskHandler.StopProcessAsync(update.GetUserId(),
                TaskProcessingField.GenerateReplyToUserMessage);

        await _botStateTreeUserHandler.SetStateAndActionAsync(update, BaseField.BaseState, BaseField.ReloadAction,
            CancellationToken.None);
        await _client.SendMessage(update.GetUserId(), "Бот перезавантажений 🏗");
        await _infoManager.SendBotCommandToUserAsync();
    }
}
