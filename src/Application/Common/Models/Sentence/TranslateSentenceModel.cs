namespace DropWord.Application.Common.Models.Sentence;

public class TranslateSentenceModel
{
    public string OriginalSentence { get; set; } = null!;
    public string TranslateSentence { get; set; } = null!;
    public string OriginalLanguage { get; set; } = null!;
    public string TranslateLanguage { get; set; } = null!;
}
