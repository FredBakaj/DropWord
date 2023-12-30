using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace DropWord.TgBot.Core.Service.Implementation;

public class ConfigureWebhook : IHostedService
{
    private readonly ILogger<ConfigureWebhook> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly IConfiguration _configuration;

    public ConfigureWebhook(
        ILogger<ConfigureWebhook> logger,
        IServiceProvider serviceProvider,
        IConfiguration configuration)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _configuration = configuration;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var botClient = scope.ServiceProvider.GetRequiredService<ITelegramBotClient>();

        // Configure custom endpoint per Telegram API recommendations:
        // https://core.telegram.org/bots/api#setwebhook
        // If you'd like to make sure that the webhook was set by you, you can specify secret data
        // in the parameter secret_token. If specified, the request will contain a header
        // "X-Telegram-Bot-Api-Secret-Token" with the secret token as content.
        var webhookAddress = _configuration.GetSection("CommonSettings")["WebHookUrl"];
        var secretToken = _configuration.GetSection("CommonSettings")["WebHookSecretToken"];
        _logger.LogInformation("Setting webhook: {WebhookAddress}", webhookAddress);
        await botClient.SetWebhookAsync(
            url: webhookAddress!,
            allowedUpdates: Array.Empty<UpdateType>(),
            secretToken: secretToken!,
            cancellationToken: cancellationToken);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var botClient = scope.ServiceProvider.GetRequiredService<ITelegramBotClient>();

        // Remove webhook on app shutdown
        _logger.LogInformation("Removing webhook");
        await botClient.DeleteWebhookAsync(cancellationToken: cancellationToken);
    }
}
