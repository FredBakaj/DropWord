using DropWord.Application.UseCase.Sentence.Queries.GetSentenceForRepeat;
using DropWord.Domain.Enums;

namespace DropWord.Application.Manager.Sentence;

public interface ISentenceManager
{
    Task RepeatSentenceAsync(long userId, bool isLearn, int usingSentencesPairId, CancellationToken cancellationToken);

    Task<SentenceForRepeatDto> GetSentenceForRepeatAsync(long userId, SentenceForRepeatModeEnum mode);
}
