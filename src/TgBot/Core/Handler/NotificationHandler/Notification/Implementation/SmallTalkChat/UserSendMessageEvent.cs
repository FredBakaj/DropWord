using MediatR;

namespace DropWord.TgBot.Core.Handler.NotificationHandler.Notification.Implementation.SmallTalkChat;

public class UserSendMessageEvent: INotification
{
    public long UserId { get; set; }
    public string Message { get; set; } = null!;
}
