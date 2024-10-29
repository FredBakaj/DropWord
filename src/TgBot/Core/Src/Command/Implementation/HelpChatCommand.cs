using DropWord.TgBot.Core.Extension;
using DropWord.TgBot.Core.Field;
using DropWord.TgBot.Core.Manager.Info;
using DropWord.TgBot.Core.Model;
using Telegram.Bot;

namespace DropWord.TgBot.Core.Src.Command.Implementation;

public class HelpChatCommand : IBotCommand
{
    private readonly ITelegramBotClient _client;
    private readonly IInfoManager _infoManager;
    public string GetCommand() => CommandField.HelpChat;
    public bool IsMoveNext() => false;

    public HelpChatCommand(ITelegramBotClient client, IInfoManager infoManager)
    {
        _client = client;
        _infoManager = infoManager;
    }

    public async Task Exec(UpdateBDto update)
    {
        await _client.SendTextMessageMarkdown2Async(update.GetUserId(), _infoManager.HelpChatText);
    }
}
