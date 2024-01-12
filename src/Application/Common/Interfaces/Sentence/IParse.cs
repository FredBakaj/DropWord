using DropWord.Application.Common.Models.Sentence;

namespace DropWord.Application.Common.Interfaces.Sentence;

public interface IParse
{
    Task<IEnumerable<ParseSentenceModel>> ParseAsync(string content);
}
