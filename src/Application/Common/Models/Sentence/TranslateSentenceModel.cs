namespace DropWord.Application.Common.Models.Sentence;

public class TranslateSentenceModel
{
    public string FirstSentence { get; set; } = null!;
    public string SecondSentence { get; set; } = null!;
    public string FirstLanguage { get; set; } = null!;
    public string SecondLanguage { get; set; } = null!;
}
