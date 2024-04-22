using DropWord.TgBot.Core.Extension;
using DropWord.TgBot.Core.Model;
using Telegram.Bot.Types.Enums;

namespace DropWord.TgBot.Core.Src.Middleware.Implementation;

public class GroupDisableMiddleware : ABotMiddleware
{
    private readonly ILogger<GroupDisableMiddleware> _logger;

    public GroupDisableMiddleware(ILogger<GroupDisableMiddleware> logger)
    {
        _logger = logger;
    }
    public override async Task Next(UpdateBDto update)
    {
        if (update.GetChatType() == ChatType.Private)
        {
            await base.Next(update);
        }
    }
}
