using System.Text.RegularExpressions;

namespace DropWord.TgBot.Core.Manager.UserFilter.Implementation;

public class ValidationManager: IValidationManager
{
    static Regex cyrillicPattern = new Regex(@"\p{IsCyrillic}");
    public bool IsAnyCyrillic(string text)
    {
        return cyrillicPattern.IsMatch(text);
    }
}
