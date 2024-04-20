using DropWord.TgBot.Core.Extension;
using DropWord.TgBot.Core.Manager.UserFilter;
using DropWord.TgBot.Core.Model;
using Telegram.Bot;

namespace DropWord.TgBot.Core.Src.Middleware.Implementation;

public class SpamBlockerMiddleware : ABotMiddleware
{
    private readonly ISpamQueryManager _spamManager;
    private readonly ITelegramBotClient _client;
    private readonly ILogger<SpamBlockerMiddleware> _logger;

    public SpamBlockerMiddleware(ISpamQueryManager spamManager, 
        ITelegramBotClient client, 
        ILogger<SpamBlockerMiddleware> logger)
    {
        _spamManager = spamManager;
        _client = client;
        _logger = logger;
    }
    public override async Task Next(UpdateBDto telegramUpdate)
    {
        await _spamManager.AddRecord(new SpamQueryBDto()
            {
                UserId = telegramUpdate.GetUserId(),
                CreateRecord = DateTime.UtcNow
            });
        
        if (await _spamManager.IsNoReachLimit(telegramUpdate.GetUserId()))
        {
            await base.Next(telegramUpdate);
        }
        else
        {
            await _client.SendTextMessageAsync(telegramUpdate.GetUserId(), "spam block");
        }

    }
}
