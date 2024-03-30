using DropWord.Application.Common.Interfaces;
using DropWord.Application.Factory.Sentence;
using DropWord.Application.Manager.Sentence.Implementation.Model;
using DropWord.Domain.Enums;

namespace DropWord.Application.Manager.Sentence.Implementation;

public class SentenceManager : ISentenceManager
{
    private readonly IApplicationDbContext _context;
    private readonly ISentencesFactory _sentencesFactory;
    private readonly IMapper _mapper;

    public SentenceManager(IApplicationDbContext context, ISentencesFactory sentencesFactory, IMapper mapper)
    {
        _context = context;
        _sentencesFactory = sentencesFactory;
        _mapper = mapper;
    }

    public async Task RepeatSentenceAsync(long userId, bool isLearn, int usingSentencesPairId,
        CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .Where(x => x.Id == userId)
            .Include(x => x.UserLearningInfo)
            .Include(x => x.UsingSentencesPairs)
            .Select(x => new
            {
                User = x,
                UsingSentencesPair = x.UsingSentencesPairs
                    .Where(y => y.Id == usingSentencesPairId)
                    .FirstOrDefault()
            })
            .FirstOrDefaultAsync();

        if (user!.User != null && user!.UsingSentencesPair != null)
        {
            
            user.UsingSentencesPair.CountUse += 1;
            user.UsingSentencesPair.IsLearning = isLearn;
            user.UsingSentencesPair.UpdateDate = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<SentenceForRepeatModel> GetSentenceForRepeatAsync(long userId, SentenceForRepeatModeEnum mode)
    {
        var sentencesForRepeat =
            await _sentencesFactory.CreateSentencesForRepeatAsync(mode);
        return  await sentencesForRepeat.Exec(userId);
    }

    public async Task ChangeLastUseForDaySentenceAsync(long userId, int usingSentencesPairId,
        CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .Where(x => x.Id == userId)
            .Include(x => x.UserLearningInfo)
            .FirstOrDefaultAsync();
        
        if (user!.UserLearningInfo.CountUseForDaySentences == null ||
            user.UserLearningInfo.LastUseForDaySentencesId == null)
        {
            user.UserLearningInfo.CountUseForDaySentences = 1;
        }
        else
        {
            user.UserLearningInfo.CountUseForDaySentences += 1;
        }
        
        user!.UserLearningInfo.LastUseForDaySentencesId = usingSentencesPairId;
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<SentencesPairModel> GetSentencesPairAsync(long userId, int usingSentencesPairId)
    {
        var sentencePair = await _context.UsingSentencesPair
            .Include(x => x.SentencesPair)
            .ThenInclude(x => x.FirstSentence)
            .Include(x => x.SentencesPair)
            .ThenInclude(x => x.SecondSentence)
            .Where(x => x.UserId == userId && x.Id == usingSentencesPairId)
            .Select(x => x.SentencesPair)
            .ProjectTo<SentencesPairModel>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();
        return sentencePair!;
    }

    public string GetSentenceLearnFromPair(SentencesPairModel sentencesPair, SentenceToLearnLabelEnum learnLabel)
    {
        var sentenceLearn = string.Empty;
        if (learnLabel == SentenceToLearnLabelEnum.First)
        {
            sentenceLearn = sentencesPair!.FirstSentence.Sentence;
        }
        else if (learnLabel == SentenceToLearnLabelEnum.Second)
        {
            sentenceLearn = sentencesPair!.SecondSentence.Sentence;
        }    
        return sentenceLearn;
    }
    
    public bool IsValidSentenceForAdd(string sentence)
    {
        bool sentenceValid = (
                sentence.Contains("~")
                || sentence.Contains("*")
                || sentence.Contains("_")
                );
        return !sentenceValid;
    }

    public bool IsValidSentenceForAdd(List<string> sentences)
    {
        foreach (var sentence in sentences)
        {
            if (!IsValidSentenceForAdd(sentence))
            {
                return false;
            }
        }

        return true;
    }
}
