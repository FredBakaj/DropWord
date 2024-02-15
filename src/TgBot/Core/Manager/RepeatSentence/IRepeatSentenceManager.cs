using DropWord.Application.UseCase.Sentence.Queries.GetSentenceForRepeat;
using DropWord.Domain.Enums;
using DropWord.TgBot.Core.Model;
using DropWord.TgBot.Core.StateDto;

namespace DropWord.TgBot.Core.Manager.RepeatSentence;

public interface IRepeatSentenceManager
{
    public Task<bool> IsShowResetCountRepeatSentences(UpdateBDto updateBDto);
    Task<SentenceForRepeatDto> GetSentencesPairAndSaveInDataAsync(UpdateBDto updateBDto);
    string GetNextSentence(SentenceForRepeatDto sentenceForRepeatDto);
    string GetOriginalSentence(SentencesRepetitionByInputSDto sentenceForRepeatDto);
    public Task<int?> GetCountRepetitionSentences(long userId);
    public Task<bool> CanShowResetCountRepeatSentences(UpdateBDto updateBDto);
    public Task SaveShowResetCountRepeatSentencesView(UpdateBDto updateBDto);
    public Task ClearShowResetCountRepeatSentencesView(UpdateBDto updateBDto);
}
