namespace DropWord.Domain.Entities;

public class AutoChatBotEntity: BaseAuditableEntity<int>
{
    public string Name { get; set; } = null!;
    public int Age { get; set; }
    public string Country { get; set; } = null!;
    public string Interests { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Language { get; set; } = null!;

    public List<AutoChatDataEntity> AutoChatDates { get; set; } = null!;
}
