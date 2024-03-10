using DropWord.Application.Common.Interfaces;
using DropWord.Application.Factory.Sentence;
using DropWord.Application.UseCase.Sentence.Queries.GetSentenceForRepeat;
using DropWord.Domain.Enums;

namespace DropWord.Application.Manager.Sentence.Implementation;

public class SentenceManager : ISentenceManager
{
    private readonly IApplicationDbContext _context;
    private readonly ISentencesFactory _sentencesFactory;

    public SentenceManager(IApplicationDbContext context, ISentencesFactory sentencesFactory)
    {
        _context = context;
        _sentencesFactory = sentencesFactory;
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
            if (user.User.UserLearningInfo.CountUseForDaySentences == null ||
                user.User.UserLearningInfo.LastUseForDaySentencesId == null)
            {
                user.User.UserLearningInfo.CountUseForDaySentences = 1;
            }
            else
            {
                user.User.UserLearningInfo.CountUseForDaySentences += 1;
            }

            user.User.UserLearningInfo.LastUseForDaySentencesId = usingSentencesPairId;
            user.UsingSentencesPair.CountUse += 1;
            user.UsingSentencesPair.IsLearning = isLearn;
            user.UsingSentencesPair.UpdateDate = DateTime.Now;

            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<SentenceForRepeatDto> GetSentenceForRepeatAsync(long userId, SentenceForRepeatModeEnum mode)
    {
        var sentencesForRepeat =
            await _sentencesFactory.CreateSentencesForRepeatAsync(mode);
        return await sentencesForRepeat.Exec(userId);
    }
}
