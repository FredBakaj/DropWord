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

    public static int TimesForDayToCount(SentencesRepeatForDayTimesModeEnum timesForDayTimes)
    {
        var dictTimesForDay = new Dictionary<SentencesRepeatForDayTimesModeEnum, int>()
        {
            {SentencesRepeatForDayTimesModeEnum.TurnOff, 0},
            {SentencesRepeatForDayTimesModeEnum.Times1InDay, 1},
            {SentencesRepeatForDayTimesModeEnum.Times3InDay, 3},
            {SentencesRepeatForDayTimesModeEnum.Times5InDay, 5},
            {SentencesRepeatForDayTimesModeEnum.Times10InDay, 10},
        };
        return dictTimesForDay[timesForDayTimes];
    }
    
    public static string TimesForDayToViewText(SentencesRepeatForDayTimesModeEnum timesForDayTimes)
    {
        var timesForDayDict = new Dictionary<SentencesRepeatForDayTimesModeEnum, string>()
        {
            { SentencesRepeatForDayTimesModeEnum.TurnOff, "❌" },
            {
                SentencesRepeatForDayTimesModeEnum.Times1InDay,
                $"{CustomConvert.TimesForDayToCount(SentencesRepeatForDayTimesModeEnum.Times1InDay)} раз"
            },
            {
                SentencesRepeatForDayTimesModeEnum.Times3InDay,
                $"{CustomConvert.TimesForDayToCount(SentencesRepeatForDayTimesModeEnum.Times3InDay)} рази"
            },
            {
                SentencesRepeatForDayTimesModeEnum.Times5InDay,
                $"{CustomConvert.TimesForDayToCount(SentencesRepeatForDayTimesModeEnum.Times5InDay)} разів"
            },
            {
                SentencesRepeatForDayTimesModeEnum.Times10InDay,
                $"{CustomConvert.TimesForDayToCount(SentencesRepeatForDayTimesModeEnum.Times10InDay)} разів"
            },
        };
        return timesForDayDict[(timesForDayTimes)];
    }
    
}
