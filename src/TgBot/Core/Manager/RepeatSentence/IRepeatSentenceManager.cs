using DropWord.Application.UseCase.Sentence.Queries.GetSentenceForRepeat;
using DropWord.TgBot.Core.Model;
using DropWord.TgBot.Core.StateDto;

namespace DropWord.TgBot.Core.Manager.RepeatSentence;

public interface IRepeatSentenceManager
{
    Task<bool> TryShowRepeatResetToStartAsync(UpdateBDto updateBDto, string resetCountFieldView);
    Task<SentenceForRepeatDto> GetSentencesPairAndSaveInDataAsync(UpdateBDto updateBDto);
    string GetNextSentence(SentenceForRepeatDto sentenceForRepeatDto);
    string GetOriginalSentence(SentencesRepetitionByInputSDto sentenceForRepeatDto);
}
