using DropWord.Domain.Enums;

namespace DropWord.Application.UseCase.Sentence.Queries.GetRecommendedNewSentence;

public class RecommendedNewSentenceDto
{
    public int Id { get; set; }
    public SentenceToLearnLabelEnum SentenceToLearnLabel { get; set; }
    public string FirstSentence { get; set; } = null!;
    public string SecondSentence { get; set; } = null!;
    public string FirstLanguage { get; set; } = null!;
    public string SecondLanguage { get; set; } = null!;
}
