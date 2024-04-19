using DropWord.TgBot.Core.Extension;
using DropWord.TgBot.Core.Manager.UserFilter;
using DropWord.TgBot.Core.Model;
using Telegram.Bot;

namespace DropWord.TgBot.Core.Src.Middleware.Implementation;

public class SpamBlockerMiddleware : ABotMiddleware
{
    private readonly ISpamQueryManager _spamManager;
    private readonly ITelegramBotClient _client;

    public SpamBlockerMiddleware(ISpamQueryManager spamManager, ITelegramBotClient client)
    {
        _spamManager = spamManager;
        _client = client;
    }
    public override async Task Next(UpdateBDto telegramUpdate)
    {
        _spamManager.AddRecord(new SpamQueryBDto()
            {
                UserId = telegramUpdate.GetUserId(),
                CreateRecord = DateTime.UtcNow
            });

        if (_spamManager.IsNoReachLimit(telegramUpdate.GetUserId()))
        {
            await base.Next(telegramUpdate);
        }
        else
        {
            await _client.SendTextMessageAsync(telegramUpdate.GetUserId(), "spam block");
        }

    }
}
