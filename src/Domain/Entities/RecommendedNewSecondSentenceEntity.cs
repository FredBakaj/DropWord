namespace DropWord.Domain.Entities;

public class RecommendedNewSecondSentenceEntity : BaseAuditableEntity<int>
{
    public string Sentence { get; set; } = null!;
    public string Language { get; set; } = null!;
    
    public List<RecommendedNewConnectionSentenceEntity> RecommendedNewConnectionSentences { get; set; }= null!;
}
