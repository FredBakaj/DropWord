using DropWord.Domain.Enums;

namespace DropWord.Application.UseCase.Sentence.Queries.GetNewSentence;

public class NewSentenceDto
{
    public string FirstSentence { get; set; } = null!;
    public string SecondSentence { get; set; } = null!;
    public int SentencePairId { get; set; }
    public SentenceToLearnLabelEnum SentenceToLearnLabel { get; set; }
}
