using DropWord.Infrastructure.Common.Configuration;

namespace TgBot.Core.Config;

public class BotConfig : IConfig
{
    private readonly IConfigurationSection _notificationSection;
    private readonly IConfigurationSection _settingsSection;

    public BotConfig(IConfiguration configuration)
    {
        _notificationSection = configuration.GetSection("NotificationService");
        _settingsSection = configuration.GetSection("CommonSettings");
    }

    public string MsSqlConnectionString => _settingsSection["MsSqlConnectionString"];
    public bool IsWebHook => Convert.ToBoolean(_settingsSection["IsWebHook"]);
    public string WebHookUrl => _settingsSection["WebHookUrl"];
    public string BotToken => _settingsSection["BotToken"];
}