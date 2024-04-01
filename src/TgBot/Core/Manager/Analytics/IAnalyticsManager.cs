namespace DropWord.TgBot.Core.Manager.Analytics;

public interface IAnalyticsManager
{
    Task SendUserActionAsync(long userId, string typeAction, string action, object? data);
}
