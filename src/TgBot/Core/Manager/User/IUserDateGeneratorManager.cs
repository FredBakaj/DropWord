using DropWord.Domain.Enums;

namespace DropWord.TgBot.Core.Manager.User;

public interface IUserDateGeneratorManager
{
    string GetRandomUserName(UserGenderEnum gender);
}
