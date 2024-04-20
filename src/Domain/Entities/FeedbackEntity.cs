namespace DropWord.Domain.Entities;

public class FeedbackEntity: BaseAuditableEntity<int>
{
    public long UserId { get; set; }
    public string Text { get; set; } = null!;
}
