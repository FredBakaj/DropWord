using DropWord.Application.Common.Models.Sentence;

namespace DropWord.Application.Common.Interfaces.Sentence;

public interface ITranslate
{
    Task<TranslateSentenceModel> TranslateAsync(string sentence, string originalLanguage,
        string translateLanguage);

    Task<IEnumerable<TranslateSentenceModel>> TranslateListAsync(IEnumerable<string> sentences, string originalLanguage,
        string translateLanguage);

    Task<DetectLanguageModel> DetectLanguageAsync(string sentence);
    Task<IEnumerable<DetectLanguageModel>> DetectLanguageListAsync(IEnumerable<string> sentence);
}
