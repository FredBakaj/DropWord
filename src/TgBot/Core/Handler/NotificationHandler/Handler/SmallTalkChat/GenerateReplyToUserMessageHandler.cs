using DropWord.TgBot.Core.Field;
using DropWord.TgBot.Core.Handler.NotificationHandler.Notification.SmallTalkChat;
using DropWord.TgBot.Core.Handler.TaskProcessingHandler;
using MediatR;
using Telegram.Bot;

namespace DropWord.TgBot.Core.Handler.NotificationHandler.Handler.SmallTalkChat;

public class GenerateReplyToUserMessageHandler : INotificationHandler<UserSendMessageEvent>
{
    private readonly ITelegramBotClient _botClient;
    private readonly IBackgroundTaskHandler _backgroundTaskHandler;

    public GenerateReplyToUserMessageHandler(
        ITelegramBotClient botClient, IBackgroundTaskHandler backgroundTaskHandler)
    {
        _botClient = botClient;
        _backgroundTaskHandler = backgroundTaskHandler;
    }

    public async Task Handle(UserSendMessageEvent notification, CancellationToken cancellationToken)
    {
        if (await _backgroundTaskHandler.IsProcessRunningAsync(notification.UserId, TaskProcessingField.GenerateReplyToUserMessage))
        {
            await _backgroundTaskHandler.StopProcessAsync(notification.UserId, TaskProcessingField.GenerateReplyToUserMessage);
        }

        await _backgroundTaskHandler.StartProcessAsync(notification.UserId, TaskProcessingField.GenerateReplyToUserMessage,
            ProcessReplyToUserMessageAsync, notification);
    }

    private async Task ProcessReplyToUserMessageAsync(UserSendMessageEvent chanelData,
        CancellationToken cancellationToken)
    {
        await Task.Delay(10000);
        cancellationToken.ThrowIfCancellationRequested();
        await _botClient.SendTextMessageAsync(chanelData.UserId, chanelData.Message);
    }
}
