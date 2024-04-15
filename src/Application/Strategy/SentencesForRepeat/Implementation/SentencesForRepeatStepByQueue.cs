using DropWord.Application.Common.Interfaces;
using DropWord.Application.Manager.Sentence.Implementation.Model;
using DropWord.Domain.Enums;
using DropWord.Domain.Exceptions;

namespace DropWord.Application.Strategy.SentencesForRepeat.Implementation;

public class SentencesForRepeatStepByQueue : ASentencesForRepeat, ISentencesForRepeatStrategy
{
    private readonly IApplicationDbContext _context;
    public SentenceForRepeatModeEnum Mode => SentenceForRepeatModeEnum.StepByQueue;

    public SentencesForRepeatStepByQueue(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<SentenceForRepeatModel> Exec(long userId)
    {
        var user = await _context.Users
            .Include(x => x.UserSettings)
            .Include(x => x.UserLearningInfo)
            .FirstAsync(x => x.Id == userId);

        var sentencesPairQuery = _context.SentencesPair
            .Include(x => x.FirstSentence)
            .Include(x => x.SecondSentence);

        //Получение ид последнего слова которое повторял пользователь
        var lastUseForDaySentenceId = user.UserLearningInfo.LastUseForDaySentencesId;
        
        var resultUsingPair = await _context.UsingSentencesPair
            .Include(x => x.SentencesPair)
            .ThenInclude(x => x.FirstSentence)
            .Include(x => x.SentencesPair)
            .ThenInclude(x => x.SecondSentence)
            .Where(x => x.UserId == userId
                        && x.SentencesPair.FirstLanguage == user.UserSettings.MainLanguage
                        && x.SentencesPair.SecondLanguage == user.UserSettings.LearnLanguage
                        && lastUseForDaySentenceId != null ? x.Id < lastUseForDaySentenceId : true)
            .OrderByDescending(x => x.Id)
            .FirstOrDefaultAsync();

        if (resultUsingPair == null && lastUseForDaySentenceId != null)
        {
            throw new OutOfSentencesToRepeatException("Finished all the sentences for repetition");
            
        }

        if(resultUsingPair == null)
        {
            throw new EmptyCollectionOfSentencesToRepeatException("Not have a sentence");
        }

        
        
        return CreateResponse(resultUsingPair.Id, resultUsingPair.SentencesPair.FirstSentence.Sentence,
            resultUsingPair.SentencesPair.SecondSentence.Sentence, user.UserSettings.LearnSentencesModeEnum,
            resultUsingPair.IsLearning);
    }
}
