using DropWord.Infrastructure.Common.Enum;

namespace DropWord.Domain.Entities;

public class UserSettingsEntity : BaseAuditableEntity<int>
{
 
    public string InterfaceLanguage { get; set; } = null!;
    public HideLanguageEnum HideLanguageEnum { get; set; }
    public string FirstLanguage { get; set; } = null!;
    public string SecondLanguage { get; set; } = null!;

    public long UserId { get; set; }
    public UserEntity User { get; set; } = null!;
}