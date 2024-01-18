using DropWord.Infrastructure.Common.Enum;

namespace DropWord.Application.UseCase.Sentence.Queries.GetSentenceForRepeat;

public class SentenceForRepeatDto
{
    public int UsingSentencesPairId { get; set; }
    public string FirstSentence { get; set; } = null!;
    public string SecondSentence { get; set; } = null!;
    public HideSentenceEnum HideSentenceEnum { get; set; }
}
