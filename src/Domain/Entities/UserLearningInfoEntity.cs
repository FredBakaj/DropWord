namespace DropWord.Domain.Entities;

public class UserLearningInfoEntity : BaseAuditableEntity<int>
{
    public int? LastUseForDaySentencesId { get; set; }
    public int? CountUseForDaySentences { get; set; }

    public long UserId { get; set; }
    public UserEntity User { get; set; } = null!;
}