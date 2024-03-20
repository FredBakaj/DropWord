using DropWord.Domain.Constants;
using DropWord.Domain.Enums;

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

    public static string IntToUTC(int timeZone)
    {
        if (timeZone > 0)
        {
            return $"UTC+{timeZone}";
        }
        else if (timeZone < 0)
        {
            return $"UTC{timeZone}";
        }
        else
        {
            return "UTC";
        }
    }

    public static int TimesForDayToCount(SentencesRepeatForDayModeEnum timesForDay)
    {
        var dictTimesForDay = new Dictionary<SentencesRepeatForDayModeEnum, int>()
        {
            {SentencesRepeatForDayModeEnum.TurnOff, 0},
            {SentencesRepeatForDayModeEnum.Times1InDay, 1},
            {SentencesRepeatForDayModeEnum.Times3InDay, 3},
            {SentencesRepeatForDayModeEnum.Times5InDay, 5},
            {SentencesRepeatForDayModeEnum.Times10InDay, 10},
        };
        return dictTimesForDay[timesForDay];
    }
    
    public static string TimesForDayToViewText(SentencesRepeatForDayModeEnum timesForDay)
    {
        var timesForDayDict = new Dictionary<SentencesRepeatForDayModeEnum, string>()
        {
            { SentencesRepeatForDayModeEnum.TurnOff, "❌" },
            {
                SentencesRepeatForDayModeEnum.Times1InDay,
                $"{CustomConvert.TimesForDayToCount(SentencesRepeatForDayModeEnum.Times1InDay)} раз"
            },
            {
                SentencesRepeatForDayModeEnum.Times3InDay,
                $"{CustomConvert.TimesForDayToCount(SentencesRepeatForDayModeEnum.Times3InDay)} рази"
            },
            {
                SentencesRepeatForDayModeEnum.Times5InDay,
                $"{CustomConvert.TimesForDayToCount(SentencesRepeatForDayModeEnum.Times5InDay)} разів"
            },
            {
                SentencesRepeatForDayModeEnum.Times10InDay,
                $"{CustomConvert.TimesForDayToCount(SentencesRepeatForDayModeEnum.Times10InDay)} разів"
            },
        };
        return timesForDayDict[(timesForDay)];
    }
    
}
