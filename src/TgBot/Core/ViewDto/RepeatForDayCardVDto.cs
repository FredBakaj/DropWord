namespace DropWord.TgBot.Core.ViewDto;

public class RepeatForDayCardVDto
{
public long UserId {get; set; }
public string FirstSentence {get; set; } = null!;
public string FirstLangEmoji {get; set; } = null!;
public string SecondLangEmoji {get; set; } = null!;
public string SecondSentence { get; set; } = null!;
}
