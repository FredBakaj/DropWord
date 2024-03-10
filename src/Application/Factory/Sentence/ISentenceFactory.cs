using DropWord.Application.Strategy.SentencesForRepeat;
using DropWord.Domain.Enums;

namespace DropWord.Application.Factory.Sentence;

public interface ISentencesFactory
{
    Task<ISentencesForRepeatStrategy> CreateSentencesForRepeatAsync(SentenceForRepeatModeEnum mode);
}
