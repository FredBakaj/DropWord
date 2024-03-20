namespace DropWord.TgBot.Core.ViewDto;

public class ChangeTimeZoneCallbackVDto : BaseVDto
{
    public string CurrentTimeZone { get; set; } = null!;
    public Dictionary<string, string> TimeZoneVariants { get; set; } = null!;
}
