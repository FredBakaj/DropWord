using DropWord.Infrastructure.Common.Enum;

namespace DropWord.Application.UseCase.Sentence.Queries.GetNewSentence;

public class NewSentenceDto
{
    public string FirstSentence { get; set; } = null!;
    public string SecondSentence { get; set; } = null!;
    public HideSentenceEnum HideSentence { get; set; }
    public int SentencePairId { get; set; }
}
