using DropWord.Application.Common.Interfaces;
using DropWord.Application.Manager.Sentence.Implementation.Model;
using DropWord.Domain.Enums;
using DropWord.Domain.Exceptions;

namespace DropWord.Application.Strategy.SentencesForRepeat.Implementation;

public class SentencesForRepeatOldDataMinCount : ASentencesForRepeat, ISentencesForRepeatStrategy
{
    private readonly IApplicationDbContext _context;
    public SentenceForRepeatModeEnum Mode => SentenceForRepeatModeEnum.OldDataMinCount;

    public SentencesForRepeatOldDataMinCount(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<SentenceForRepeatModel> Exec(long userId)
    {
        var userSettings = await _context.UserSettings
            .Where(x => x.UserId == userId)
            .FirstOrDefaultAsync();

        var oldUsingSentencePair = await _context.UsingSentencesPair
            .Where(x => x.UserId == userId
                        && x.SentencesPair.FirstLanguage == userSettings!.MainLanguage
                        && x.SentencesPair.SecondLanguage == userSettings!.LearnLanguage)
            .OrderBy(x => x.UpdateDate)
            .ThenBy(x => x.CountUse)
            .FirstOrDefaultAsync();

        if (oldUsingSentencePair == null)
        {
            throw new EmptyOldUsingSentencesPairException("Not found sentences pair for repeat per day");
        }
        
        var sentencesPair = await _context.SentencesPair
            .Include(x => x.FirstSentence)
            .Include(x => x.SecondSentence)
            .Where(x => x.Id == oldUsingSentencePair!.SentencesPairId)
            .FirstOrDefaultAsync();

        return CreateResponse(oldUsingSentencePair!.Id,
            sentencesPair!.FirstSentence.Sentence,
            sentencesPair!.SecondSentence.Sentence,
            userSettings!.LearnSentencesModeEnum,
            oldUsingSentencePair.IsLearning);
    }
}
