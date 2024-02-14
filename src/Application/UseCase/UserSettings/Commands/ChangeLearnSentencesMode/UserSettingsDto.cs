using DropWord.Domain.Entities;
using DropWord.Domain.Enums;

namespace DropWord.Application.UseCase.UserSettings.Commands.ChangeLearnSentencesMode;

//TODO дублируеться в namespace DropWord.Application.UseCase.User.Queries.GetUser;
public class UserSettingsDto
{
    public string InterfaceLanguage { get; set; } = null!;
    public LearnSentencesModeEnum LearnSentencesModeEnum { get; set; }
    public string MainLanguage { get; set; } = null!;
    public string LearnLanguage { get; set; } = null!;

    public long UserId { get; set; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<UserSettingsEntity, UserSettingsDto>();
        }
    }
}
