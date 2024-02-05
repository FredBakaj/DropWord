namespace DropWord.TgBot.Core.ViewDto;

public class IncorrectInputVDto : BaseVDto
{
    public string RightSentence { get; set; } = null!;
    public string NextSentence { get; set; } = null!;
}
