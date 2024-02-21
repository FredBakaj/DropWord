using DropWord.Application.Common.Interfaces.Sentence;
using DropWord.Application.Common.Models.Sentence;
using DropWord.Domain.Exceptions;
using Microsoft.Extensions.Configuration;

namespace DropWord.Infrastructure.Sentence;

public class Parse : IParse
{
    private readonly int _maxSentenceLength;
    private readonly int _maxCountSentences;

    public Parse(IConfiguration configuration)
    {
        _maxSentenceLength =
            Convert.ToInt32(configuration.GetSection("SentencesSettings")["MaxLengthSentenceForSave"]);
        _maxCountSentences =
            Convert.ToInt32(configuration.GetSection("SentencesSettings")["MaxCountSentencesForSave"]);
    }

    public Task<IEnumerable<ParseSentenceModel>> ParseAsync(string content)
    {
        IEnumerable<ParseSentenceModel> result = null!;
        if (!string.IsNullOrEmpty(content) && content[content.Length - 1] == '.')
        {
            content = content.Substring(0, content.Length - 1);
        }
        if (content.Any(x => x == '.') || content.Any(x => x == '\n'))
        {
            result = ParseText(content);
        }
        else
        {
            result = ParseSentence(content);
        }

        return Task.FromResult(result);
    }

    private IEnumerable<ParseSentenceModel> ParseSentence(string content)
    {
        var result = new List<ParseSentenceModel>() { new ParseSentenceModel() { Sentence = content } };
        return result;
    }

    private IEnumerable<ParseSentenceModel> ParseText(string content)
    {
        var result = new List<ParseSentenceModel>();
        var splitContent = content
            .Split('.')
            .SelectMany(x => x.Split('\n'))
            .Where(x => x.Length > 0)
            .ToArray();
        if (splitContent.Length > _maxCountSentences)
        {
            throw new MaxCountSentencesException("text had so match sentences");
        }

        foreach (var item in splitContent)
        {
            var item_ = item.Trim();
            if (item_.Length > _maxSentenceLength)
            {
                throw new MaxLengthSentenceException("Max length sentences where parse text");
            }

            if (item_.Length == 0)
            {
                throw new InvalidDataException("sentences was not be empty");
            }

            if (!item_.Any(x => x == ' '))
            {
                throw new InvalidDataException("sentences had zero space");
            }

            result.Add(new ParseSentenceModel() { Sentence = item_ });
        }

        return result;
    }
}
