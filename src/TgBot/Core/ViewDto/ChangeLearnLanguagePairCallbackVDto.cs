namespace DropWord.TgBot.Core.ViewDto;

public class ChangeLearnLanguagePairCallbackVDto : BaseVDto
{
    public string MainLanguage { get; set; } = null!;
    public string LearnLanguage { get; set; } = null!;
    public Dictionary<string,string> LearnLanguageVariants { get; set; } = null!;
}
