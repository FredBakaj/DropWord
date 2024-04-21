using DropWord.Domain.Enums;

namespace DropWord.Application.UseCase.Sentence.Queries.GetNewSentence;

public class NewSentenceDto
{
    public int SentencePairId { get; set; }
    public string FirstSentence { get; set; } = null!;
    public string SecondSentence { get; set; } = null!;
    public string FirstLanguage { get; set; } = null!;
    public string SecondLanguage { get; set; } = null!;
    public SentenceToLearnLabelEnum SentenceToLearnLabel { get; set; }
}
