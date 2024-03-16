namespace DropWord.TgBot.Core.ViewDto;

public class RepeatForDayInputWithErrorsVDto : BaseVDto
{
    public string RightSentence { get; set; } = null!;
    public string CorrectedSentence { get; set; } = null!;
}
