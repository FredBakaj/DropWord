using DropWord.Domain.Enums;

namespace DropWord.TgBot.Core.StateDto;

public class SentencesRepetitionByInputSDto
{
    public int UsingSentencesPairId { get; set; }
    public string FirstSentence { get; set; } = null!;
    public string SecondSentence { get; set; } = null!;
    public SentenceToLearnLabelEnum SentenceToLearnLabelEnum { get; set; }
}
