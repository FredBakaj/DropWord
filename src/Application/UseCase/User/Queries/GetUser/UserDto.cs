using DropWord.Domain.Entities;
using DropWord.Domain.Enums;

namespace DropWord.Application.UseCase.User.Queries.GetUser;

public class UserDto
{
    public string Name { get; set; } = null!;
    public UserGenderEnum? Gender { get; set; }


    public UserSettingsDto UserSettings { get; set; } = null!;
    
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<UserEntity, UserDto>();
        }
    }
}
