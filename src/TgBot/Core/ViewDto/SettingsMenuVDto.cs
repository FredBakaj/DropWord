namespace DropWord.TgBot.Core.ViewDto;

public class SettingsMenuVDto : BaseVDto
{
    public string ChangeModeIcon { get; set; } = null!;
    public string LearnLanguagePairEmoji { get; set; } = null!;
    public string TimeZone { get; set; } = null!;
    public string TimesForDay { get; set; } = null!;
}
