using DropWord.Domain.Enums;

namespace DropWord.Domain.Entities;

public class AutoChatHistoryEntity: BaseAuditableEntity<int>
{
    public AutoChatSenderEnum SenderEnum { get; set; }
    public AutoChatMessageTypeEnum MessageTypeEnum { get; set; }
    public string Message { get; set; } = null!;
    
    public int AutoChatDataId { get; set; }
    public AutoChatDataEntity AutoChatData { get; set; } = null!;
}
