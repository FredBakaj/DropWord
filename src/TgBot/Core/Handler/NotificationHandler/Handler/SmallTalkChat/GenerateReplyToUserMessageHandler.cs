using DropWord.TgBot.Core.Field;
using DropWord.TgBot.Core.Handler.NotificationHandler.Notification;
using DropWord.TgBot.Core.Handler.NotificationHandler.Notification.Implementation.SmallTalkChat;
using DropWord.TgBot.Core.Handler.TaskProcessingHandler;
using MediatR;
using Telegram.Bot;

namespace DropWord.TgBot.Core.Handler.NotificationHandler.Handler.SmallTalkChat;

/// <summary>
/// чтобы вызвался хендлер нужно в контреллере подтянуть IPublisher, и вызвать у него метод Publish
/// передав модель события
/// Пример
/// await _mediator.Publish(
///     
///     new UserSendMessageEvent()
///     {
///         UserId = updateBDto.GetUserId(), Message = updateBDto.GetMessage().Text!
///     });
/// </summary>

public class GenerateReplyToUserMessageHandler : INotificationHandler<UserSendMessageEvent>, IHasPriority
{
    private readonly ITelegramBotClient _botClient;
    private readonly IBackgroundTaskHandler _backgroundTaskHandler;

    public GenerateReplyToUserMessageHandler(
        ITelegramBotClient botClient, IBackgroundTaskHandler backgroundTaskHandler)
    {
        _botClient = botClient;
        _backgroundTaskHandler = backgroundTaskHandler;
    }

    public Task Handle(UserSendMessageEvent notification, CancellationToken cancellationToken)
    {
        // if (await _backgroundTaskHandler.IsProcessRunningAsync(notification.UserId, TaskProcessingField.GenerateReplyToUserMessage))
        // {
        //     await _backgroundTaskHandler.StopProcessAsync(notification.UserId, TaskProcessingField.GenerateReplyToUserMessage);
        // }
        //
        // await _backgroundTaskHandler.StartProcessAsync(notification.UserId, TaskProcessingField.GenerateReplyToUserMessage,
        //     ProcessReplyToUserMessageAsync, notification, TODO);
        
        return Task.CompletedTask; 
    }

    private async Task ProcessReplyToUserMessageAsync(UserSendMessageEvent chanelData,
        CancellationToken cancellationToken)
    {
        await Task.Delay(10000);
        cancellationToken.ThrowIfCancellationRequested();
        await _botClient.SendMessage(chanelData.UserId, chanelData.Message);
    }
    
    public int Priority => 1;
}
