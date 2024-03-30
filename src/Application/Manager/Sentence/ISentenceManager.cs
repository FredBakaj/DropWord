using DropWord.Application.Manager.Sentence.Implementation.Model;
using DropWord.Domain.Enums;

namespace DropWord.Application.Manager.Sentence;

public interface ISentenceManager
{
    Task RepeatSentenceAsync(long userId, bool isLearn, int usingSentencesPairId, CancellationToken cancellationToken);

    Task<SentenceForRepeatModel> GetSentenceForRepeatAsync(long userId, SentenceForRepeatModeEnum mode);

    Task ChangeLastUseForDaySentenceAsync(long userId, int usingSentencesPairId,
        CancellationToken cancellationToken);

    Task<SentencesPairModel> GetSentencesPairAsync(long userId, int usingSentencesPairId);

    string GetSentenceLearnFromPair(SentencesPairModel sentencesPair, SentenceToLearnLabelEnum learnLabel);

    /// <summary>
    /// Проверка валидно ли слово для добавления
    /// </summary>
    /// <param name="sentence"></param>
    /// <returns></returns>
    bool IsValidSentenceForAdd(string sentence);
    bool IsValidSentenceForAdd(List<string> sentences);
}
