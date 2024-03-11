using DropWord.Domain.Enums;

namespace DropWord.TgBot.Model;

public class SentenceForRepeatApiModel
{
    public int UsingSentencesPairId { get; set; }
    public string FirstSentence { get; set; } = null!;
    public string SecondSentence { get; set; } = null!;
    public SentenceToLearnLabelEnum SentenceToLearnLabel { get; set; }
}
