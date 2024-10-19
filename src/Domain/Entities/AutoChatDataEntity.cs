namespace DropWord.Domain.Entities;

public class AutoChatDataEntity: BaseAuditableEntity<int>
{
    public bool IsClosed { get; set; }

    public long UserId { get; set; }
    public UserEntity User { get; set; } = null!;

    public int AutoChatBotId { get; set; }
    public AutoChatBotEntity AutoChatBot { get; set; } = null!;
    
    public List<AutoChatHistoryEntity> AutoChatHistories { get; set; } = null!;
    
}
