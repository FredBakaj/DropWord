using DropWord.Infrastructure.Common.Enum;

namespace DropWord.Domain.Entities;

public class UserSettingsEntity : BaseAuditableEntity<int>
{
 
    public string InterfaceLanguage { get; set; } = null!;
    public HideSentenceEnum HideSentenceEnum { get; set; }
    public string MainLanguage { get; set; } = null!;
    public string LearnLanguage { get; set; } = null!;

    public long UserId { get; set; }
    public UserEntity User { get; set; } = null!;
}