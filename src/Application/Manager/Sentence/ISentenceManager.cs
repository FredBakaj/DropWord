namespace DropWord.Application.Manager.Sentence;

public interface ISentenceManager
{
    Task RepeatSentenceAsync(long userId, bool isLearn, int usingSentencesPairId, CancellationToken cancellationToken);
}
