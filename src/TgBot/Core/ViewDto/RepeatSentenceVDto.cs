using DropWord.Infrastructure.Common.Enum;

namespace DropWord.TgBot.Core.ViewDto;

public class RepeatSentenceVDto : BaseVDto
{
    public HideSentenceEnum HideSentenceEnum { get; set; }
    public string FirstSentence { get; set; } = null!;
    public string SecondSentence { get; set; } = null!;
}
