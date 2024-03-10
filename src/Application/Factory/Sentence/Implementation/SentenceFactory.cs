using DropWord.Application.Strategy.SentencesForRepeat;
using DropWord.Domain.Enums;
using DropWord.Domain.Exceptions;

namespace DropWord.Application.Factory.Sentence.Implementation;

public class SentencesFactory : ISentencesFactory
{
    private readonly IEnumerable<ISentencesForRepeatStrategy> _sentencesForRepeatStrategies;

    public SentencesFactory(IEnumerable<ISentencesForRepeatStrategy> sentencesForRepeatStrategies)
    {
        _sentencesForRepeatStrategies = sentencesForRepeatStrategies;
    }

    public async Task<ISentencesForRepeatStrategy> CreateSentencesForRepeatAsync(SentenceForRepeatModeEnum mode)
    {
        var result =  _sentencesForRepeatStrategies
            .FirstOrDefault(x => x.Mode == mode);
        if (result == null)
        {
            throw new EmptySentencesForRepeatException($"Collection not have a select strategy {nameof(mode)}");
        }
        return await Task.FromResult(result);
    }
}
