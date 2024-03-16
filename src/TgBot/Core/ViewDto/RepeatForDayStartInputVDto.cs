namespace DropWord.TgBot.Core.ViewDto;

public class RepeatForDayStartInputVDto
{
    public long UserId { get; set; }
    public string FirstLangEmoji { get; set; } = null!;
    public string SecondLangEmoji { get; set; } = null!;
    public string Sentence { get; set; } = null!;
}
