using DropWord.Domain.Constants;

namespace DropWord.TgBot.Core.Utils;

public static class CustomConvert
{
    public static string LanguageToEmoji(string language)
    {
        var dictEmoji = new Dictionary<string,string>()
        {
            {LanguageConst.Ukrainian, "🇺🇦"},
            {LanguageConst.English, "🇺🇸"},
            {LanguageConst.German, "🇩🇪"},
            {LanguageConst.Polish, "🇵🇱"},
            {LanguageConst.French, "🇫🇷"},
        };
        return dictEmoji[language.ToLower()];
    }
}
