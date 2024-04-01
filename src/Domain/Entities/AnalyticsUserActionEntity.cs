namespace DropWord.Domain.Entities;

public class AnalyticsUserActionEntity : BaseAuditableEntity<int>
{
    public long UserId { get; set; }
    public string TypeAction { get; set; } = null!;
    public string Action { get; set; } = null!;
    public string? Data { get; set; } = null!;
}
