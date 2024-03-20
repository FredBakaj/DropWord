namespace DropWord.TgBot.Core.ViewDto;

public class ChangeTimesForDayCallbackVDto : BaseVDto
{
    public string CurrentTimesForDay { get; set; } = null!;
    public Dictionary<string, string> TimesForDayVariants { get; set; } = null!;
}
