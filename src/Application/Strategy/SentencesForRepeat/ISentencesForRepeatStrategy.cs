using DropWord.Application.UseCase.Sentence.Queries.GetSentenceForRepeat;
using DropWord.Domain.Enums;

namespace DropWord.Application.Strategy.SentencesForRepeat;

public interface ISentencesForRepeatStrategy
{
    SentenceForRepeatModeEnum Mode { get; }
    Task<SentenceForRepeatDto> Exec(long userId);
}
