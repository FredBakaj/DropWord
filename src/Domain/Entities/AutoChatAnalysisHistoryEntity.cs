namespace DropWord.Domain.Entities;

public class AutoChatAnalysisHistoryEntity: BaseAuditableEntity<int>
{
    public string TextAnalysis { get; set; } = null!;
    
    public int AutoChatDataId { get; set; }
    
    public int LastAnalyzeAutoChatHistoryId { get; set;}
    
    public long UserId { get; set; }

    public UserEntity User { get; set; } = null!;
}
