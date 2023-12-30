namespace DropWord.Domain.Entities;

public class SentenceEntity : BaseAuditableEntity<int>
{
    public string Sentence { get; set; } = null!;
    public string Language { get; set; }= null!;
    
    public List<SentencesPairEntity> FirstSentencesPairs { get; set; }= null!;
    public List<SentencesPairEntity> SecondSentencesPairs { get; set; }= null!;
}