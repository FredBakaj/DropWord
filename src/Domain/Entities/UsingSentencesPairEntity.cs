namespace DropWord.Domain.Entities;

public class UsingSentencesPairEntity : BaseAuditableEntity<int>
{
    public bool IsLearning { get; set; }
    public int CountUse { get; set; }
    public DateTime UpdateDate { get; set; }

    public long UserId { get; set; }
    public UserEntity User { get; set; } = null!;
    public int? SentencesPairId { get; set; }
    public SentencesPairEntity SentencesPair { get; set; } = null!;
}
