using DropWord.Domain.Enums;

namespace DropWord.TgBot.Core.ViewDto;

public class RepeatSentenceVDto : BaseVDto
{
    public SentenceToLearnLabelEnum SentenceToLearnLabel { get; set; }
    public string FirstSentence { get; set; } = null!;
    public string SecondSentence { get; set; } = null!;
}
