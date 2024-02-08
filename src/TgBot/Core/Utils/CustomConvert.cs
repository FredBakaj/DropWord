namespace DropWord.TgBot.Core.Utils;

public static class CustomConvert
{
    public static string LanguageToEmoji(string language)
    {
        var dictEmoji = new Dictionary<string,string>()
        {
            {"uk", "🇺🇦"},
            {"en", "🇺🇸"},
        };
        return dictEmoji[language.ToLower()];
    }
}
