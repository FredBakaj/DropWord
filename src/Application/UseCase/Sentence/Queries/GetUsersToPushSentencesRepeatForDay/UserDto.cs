using DropWord.Domain.Entities;

namespace DropWord.Application.UseCase.Sentence.Queries.GetUsersToPushSentencesRepeatForDay;

public class UserDto
{
    public long Id { get; set; }
    public UserSettingsDto UserSettings { get; set; } = null!;
    
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<UserEntity, UserDto>();
        }
    }
}
