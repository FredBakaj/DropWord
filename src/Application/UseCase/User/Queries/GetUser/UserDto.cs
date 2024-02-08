using DropWord.Domain.Entities;

namespace DropWord.Application.UseCase.User.Queries.GetUser;

public class UserDto
{
    public UserSettingsDto UserSettings { get; set; } = null!;
    
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<UserEntity, UserDto>();
        }
    }
}
