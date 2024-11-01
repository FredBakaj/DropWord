using System.ComponentModel;
using DropWord.Domain.Enums;

namespace DropWord.TgBot.Core.Manager.User.Implementation;

public class UserDateGeneratorManager: IUserDateGeneratorManager
{
    public string[] ManNames = new[]
    {
        "Ethan", "Oliver", "Mason", "Liam", "Benjamin", "Lucas", "Henry", "James", "Alexander", "William",
        "Sebastian", "Elijah", "Daniel", "Matthew", "Samuel", "Nathan", "Jacob", "Leo", "Jack", "Caleb",
    };

    public string[] WomanNames = new[]
    {
        "Emma", "Olivia", "Ava", "Sophia", "Isabella", "Mia", "Emily", "Abigail", "Madison", "Ella", "Grace",
        "Chloe", "Olivia", "Sophia", "Amelia", "Evelyn", "Victoria", "Lily", "Sarah", "Addison",
    };
    
    public string GetRandomUserName(UserGenderEnum gender)
    {
        Random rnd = new Random();
        switch (gender)
        {
            case UserGenderEnum.Man:
                return ManNames[rnd.Next(ManNames.Length)];
                
            case UserGenderEnum.Woman:
                return WomanNames[rnd.Next(WomanNames.Length)];
        }

        throw new InvalidEnumArgumentException($"not processing this gender {gender.ToString()}");
    }
}
