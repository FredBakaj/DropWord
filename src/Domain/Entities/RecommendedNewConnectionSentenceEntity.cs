namespace DropWord.Domain.Entities;

public class RecommendedNewConnectionSentenceEntity : BaseAuditableEntity<int>
{
    public int RecommendedNewFirstSentenceId { get; set; }
    public RecommendedNewFirstSentenceEntity RecommendedNewFirstSentence { get; set; } = null!;
    
    public int RecommendedNewSecondSentenceId { get; set; }
    public RecommendedNewSecondSentenceEntity RecommendedNewSecondSentence { get; set; } = null!;

    public List<RecommendedNewConnectionWithUserEntity> RecommendedNewConnectionWithUsers { get; set; } = null!;
}
