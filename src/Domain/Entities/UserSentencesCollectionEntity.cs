namespace DropWord.Domain.Entities;

public class UserSentencesCollectionEntity : BaseAuditableEntity<int>
{
    public string Description { get; set; } = null!;
    public string FirstLanguage { get; set; } = null!;
    public string SecondLanguage { get; set; } = null!;

    public long UserId { get; set; }
    public UserEntity User { get; set; } = null!;
    public List<SentencesPairEntity> SentencesPairs { get; set; } = new();
}