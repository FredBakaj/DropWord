namespace DropWord.Domain.Entities;

public class StateTreeEntity : BaseAuditableEntity<int>
{
    public string State { get; set; } = null!;
    public string Action { get; set; } = null!;
    public string JsonData { get; set; } = null!;

    public long UserId { get; set; }
    public UserEntity User { get; set; } = null!;
}