namespace DropWord.TgBot.Core.ViewDto;

public class InputWithErrorsAndOutOfSentencesToRepeatVDto : BaseVDto
{
    public string RightSentence { get; set; } = null!;
    public string CorrectedSentence { get; set; } = null!;
}
