using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace DropWord.TgBot.Core.Service;

public class ResetWebhookService : BackgroundService
{
    private readonly ILogger<ResetWebhookService> _logger;
    private readonly ITelegramBotClient _botClient;
    private readonly bool _isWebhook;
    private readonly string _webhookAddress;
    private readonly string _webHookSecretToken;
    private readonly int _timeCheckWebhook;

    public ResetWebhookService(ILogger<ResetWebhookService> logger, IConfiguration configuration,
        ITelegramBotClient botClient)
    {
        _logger = logger;
        _botClient = botClient;

        _isWebhook = Convert.ToBoolean(configuration.GetSection("CommonSettings")["IsWebHook"]);
        _webhookAddress = configuration.GetSection("CommonSettings")["WebHookUrl"]!;
        _webHookSecretToken = configuration.GetSection("CommonSettings")["WebHookSecretToken"]!;
        _timeCheckWebhook = Convert.ToInt32(configuration.GetSection("ApplicationSettings")["TimeCheckWebhook"]!);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (_isWebhook)
        {
            _logger.LogInformation("ResetWebhook service is starting...");
        }

        while (_isWebhook)
        {
            try
            {
                var webhookInfo = await _botClient.GetWebhookInfoAsync();
                if (string.IsNullOrEmpty(webhookInfo.Url))
                {
                    _logger.LogInformation("reset webhook: {WebhookAddress}", _webhookAddress);
                    await _botClient.SetWebhookAsync(
                        url: _webhookAddress,
                        allowedUpdates: Array.Empty<UpdateType>(),
                        secretToken: _webHookSecretToken,
                        cancellationToken: CancellationToken.None);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "[ResetWebhookService] " + e.Message);
            }

            await Task.Delay(TimeSpan.FromMinutes(_timeCheckWebhook));
        }
    }
}
