using System.ComponentModel.DataAnnotations.Schema;

namespace DropWord.Domain.Entities;

public class SentencesPairEntity : BaseAuditableEntity<int>
{
    public string FirstLanguage { get; set; } = null!;
    public string SecondLanguage { get; set; } = null!;

    public long? UserId { get; set; }
    [ForeignKey("UserId")]
    public UserEntity User { get; set; } = null!;
    public int FirstSentenceId { get; set; }
    public SentenceEntity FirstSentence { get; set; } = null!;
    public int SecondSentenceId { get; set; }
    public SentenceEntity SecondSentence { get; set; } = null!;
    public List<UsingSentencesPairEntity> UsingSentencesPairs { get; set; } = null!;
    public List<UserSentencesCollectionEntity> UserSentencesCollections { get; set; } = new();
}
