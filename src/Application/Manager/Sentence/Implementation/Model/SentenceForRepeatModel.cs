using DropWord.Domain.Enums;

namespace DropWord.Application.Manager.Sentence.Implementation.Model;

public class SentenceForRepeatModel
{
    public int UsingSentencesPairId { get; set; }
    public string FirstSentence { get; set; } = null!;
    public string SecondSentence { get; set; } = null!;
    public SentenceToLearnLabelEnum SentenceToLearnLabel { get; set; }
}
