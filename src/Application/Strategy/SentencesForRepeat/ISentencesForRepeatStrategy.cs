using DropWord.Application.Manager.Sentence.Implementation.Model;
using DropWord.Domain.Enums;

namespace DropWord.Application.Strategy.SentencesForRepeat;

public interface ISentencesForRepeatStrategy
{
    SentenceForRepeatModeEnum Mode { get; }
    Task<SentenceForRepeatModel> Exec(long userId);
}
