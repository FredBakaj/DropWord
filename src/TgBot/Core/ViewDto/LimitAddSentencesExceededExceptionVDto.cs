namespace DropWord.TgBot.Core.ViewDto;

public class LimitAddSentencesExceededExceptionVDto : BaseVDto
{
    public int MaxCountSentence { get; set; }
}
