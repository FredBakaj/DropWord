namespace DropWord.TgBot.Model;

public class RepeatForDayApiModel
{
    public long UserId { get; set; }
    public SentenceForRepeatApiModel SentenceForRepeatApi { get; set; } = null!;
}
