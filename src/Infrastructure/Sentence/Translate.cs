using DropWord.Application.Common.Interfaces.Sentence;
using DropWord.Application.Common.Models.Sentence;
using DropWord.Infrastructure.Sentence.Dto;
using DropWord.Infrastructure.Sentence.Field.RestApiRequest;
using DropWord.Infrastructure.Utils;
using LanguageDetection;

namespace DropWord.Infrastructure.Sentence;

public class Translate : ITranslate
{
    public async Task<TranslateSentenceModel> TranslateAsync(string sentence, string originalLanguage,
        string translateLanguage)
    {
        string translateSentence = await TranslatorAsync(sentence, originalLanguage, translateLanguage);
        var result = new TranslateSentenceModel()
        {
            FirstSentence = sentence,
            SecondSentence = translateSentence,
            FirstLanguage = originalLanguage,
            SecondLanguage = translateLanguage
        };
        return result;
    }

    public async Task<IEnumerable<TranslateSentenceModel>> TranslateListAsync(IEnumerable<string> sentences,
        string originalLanguage, string translateLanguage)
    {
        var originalText = String.Empty;
        var sentencesList = sentences.ToList();
        foreach (var item in sentencesList)
        {
            originalText += $"{item} #$% ";
        }

        originalText = originalText.Substring(0, originalText.Length - 3);
        string translateText = await TranslatorAsync(originalText, originalLanguage, translateLanguage);
        string[] splitText = translateText.Split("#$%");
        List<TranslateSentenceModel> result = new List<TranslateSentenceModel>();

        for (int i = 0; i < splitText.Length; i++)
        {
            result.Add(new TranslateSentenceModel()
            {
                FirstSentence = sentencesList[i],
                SecondSentence = splitText[i],
                FirstLanguage = originalLanguage,
                SecondLanguage = translateLanguage
            });
        }

        return result;
    }


    public Task<DetectLanguageModel> DetectLanguageAsync(string sentence)
    {
        var LangDetector = new LanguageDetector();
        LangDetector.AddAllLanguages();
        var detectLanguage = LangDetector.Detect(sentence)!;
        var result = new DetectLanguageModel() { Language = detectLanguage, Sentence = sentence };
        return Task.FromResult(result);
    }

    public Task<IEnumerable<DetectLanguageModel>> DetectLanguageListAsync(IEnumerable<string> sentence)
    {
        var LangDetector = new LanguageDetector();
        LangDetector.AddAllLanguages();
        var result = new List<DetectLanguageModel>();
        foreach (var item in sentence)
        {
            var detectLanguage = LangDetector.Detect(item)!;
            var resultItem = new DetectLanguageModel() { Language = detectLanguage, Sentence = item };
            result.Add(resultItem);
        }

        return Task.FromResult<IEnumerable<DetectLanguageModel>>(result);
    }

    private async Task<String> TranslatorAsync(string input, string languageFrom, string languageTo)
    {
        string apiUrl = TranslateField.TranslateApiUrl;
        var restApiClient = new RestApiClient();
        var headers = new Dictionary<string, string>()
        {
            { "X-RapidAPI-Key", TranslateField.XRapidAPIKeyHeader },
            { "X-RapidAPI-Host", TranslateField.XRapidAPIHostHeader }
        };
        var body = new { q = input, source = languageFrom, target = languageTo };
        var apiResponse = await restApiClient.PostAsync<TranslateDataDTO>(apiUrl, headers, body);

        return apiResponse.Data.Translations.TranslatedText;
    }
}
