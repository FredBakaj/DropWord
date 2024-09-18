namespace DropWord.Domain.Entities;

public class RecommendedNewConnectionWithUserEntity : BaseAuditableEntity<int>
{
    public long UserId { get; set; }
    public UserEntity User { get; set; } = null!;

    public int RecommendedNewConnectionSentenceId { get; set; }
    public RecommendedNewConnectionSentenceEntity RecommendedNewConnectionSentence { get; set; } = null!;
}
