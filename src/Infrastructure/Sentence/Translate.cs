using DropWord.Application.Common.Interfaces.Sentence;
using DropWord.Application.Common.Models.Sentence;
using DropWord.Domain.Constants;
using DropWord.Infrastructure.Sentence.Dto;
using DropWord.Infrastructure.Sentence.Field.RestApiRequest;
using DropWord.Infrastructure.Utils.RestApiClient.Implementation;
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
            OriginalSentence = sentence,
            TranslateSentence = translateSentence,
            OriginalLanguage = originalLanguage,
            TranslateLanguage = translateLanguage
        };
        return result;
    }

    public async Task<IEnumerable<TranslateSentenceModel>> TranslateListAsync(IEnumerable<string> sentences,
        string originalLanguage, string translateLanguage)
    {
        var originalText = String.Empty;
        var sentencesList = sentences.ToList();
        var spliter = "~~~";
        foreach (var item in sentencesList.SkipLast(1))
        {
            originalText += $"{item} {spliter} ";
        }

        originalText += sentencesList.Last();

        string translateText = await TranslatorAsync(originalText, originalLanguage, translateLanguage);
        var splitText = translateText.Split(spliter).Select(str => str.Trim()).ToList();
        List<TranslateSentenceModel> result = new List<TranslateSentenceModel>();

        for (int i = 0; i < splitText.Count; i++)
        {
            result.Add(new TranslateSentenceModel()
            {
                OriginalSentence = sentencesList[i],
                TranslateSentence = splitText[i],
                OriginalLanguage = originalLanguage,
                TranslateLanguage = translateLanguage
            });
        }

        return result;
    }

    public Task<DetectLanguageModel> DetectLanguageAsync(string sentence)
    {
        var detectLanguage = ConvertLanguageCode(DetectLang(sentence)!);
        var result = new DetectLanguageModel() { Language = detectLanguage, Sentence = sentence };
        return Task.FromResult(result);
    }

    public Task<IEnumerable<DetectLanguageModel>> DetectLanguageListAsync(IEnumerable<string> sentence)
    {
        var result = new List<DetectLanguageModel>();
        foreach (var item in sentence)
        {
            var detectLanguage = ConvertLanguageCode(DetectLang(item));
            var resultItem = new DetectLanguageModel() { Language = detectLanguage, Sentence = item };
            result.Add(resultItem);
        }

        return Task.FromResult<IEnumerable<DetectLanguageModel>>(result);
    }

    private string DetectLang(string sentence)
    {
        var langDetector = new LanguageDetector();
        langDetector.AddLanguages(
            "ukr", 
            //"fra", 
            "eng" 
            //"deu", 
            //"pol"
            );
        return langDetector.Detect(sentence);
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

    public string ConvertLanguageCode(string code)
    {
        Dictionary<string, string> codes = new Dictionary<string, string>()
        {
            { "ukr", LanguageConst.Ukrainian },
            //{ "fra", LanguageConst.French },
            { "eng", LanguageConst.English },
            //{ "deu", LanguageConst.German },
            //{ "pol", LanguageConst.Polish },
        };
        return codes[code];
    }
}
