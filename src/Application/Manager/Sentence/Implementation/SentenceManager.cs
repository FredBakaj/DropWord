using DropWord.Application.Common.Interfaces;
using DropWord.Application.Manager.Sentence.Implementation.Model;
using DropWord.Domain.Enums;

namespace DropWord.Application.Manager.Sentence.Implementation;

public class SentenceManager : ISentenceManager
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    private int _limitForAddedSentences;

    public SentenceManager(IApplicationDbContext context, IMapper mapper,
        IConfig config)
    {
        _context = context;
        _mapper = mapper;

        _limitForAddedSentences = Convert.ToInt16(config.GetValue("LimitForAddedSentences"));
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

    public async Task<int> GetCountAddedSentencesAsync(long userId, DateTimeOffset startDate, DateTimeOffset endDate)
    {
        var countAddedSentences = await _context.SentencesPair
            .Where(x => x.UserId == userId)
            .CountAsync(x => x.Created >= startDate && x.Created <= endDate);
        return countAddedSentences;
    }

    public async Task<bool> IsLimitAddSentencesExceededAsync(long userId)
    {
        DateTimeOffset today = DateTimeOffset.Now.Date;
        DateTimeOffset todayStart = today.AddHours(0);
        DateTimeOffset todayEnd = today.AddHours(23).AddMinutes(59).AddSeconds(59);
        var countAddedSentences = await GetCountAddedSentencesAsync(userId, todayStart, todayEnd);

        if (countAddedSentences > _limitForAddedSentences)
        {
            return false;
        }

        return true;
    }

    public bool IsNotOneWord(string sentence)
    {
        if (sentence.Contains(" "))
        {
            return true;
        }

        return false;
    }

    public bool IsNotOneWord(List<string> sentences)
    {
        foreach (var sentence in sentences)
        {
            if (!IsNotOneWord(sentence))
            {
                return false;
            }
        }
        return true;
    }

    public SentenceToLearnLabelEnum DetectSentenceToLearnLabel(bool isLearning, LearnSentencesModeEnum learnSentencesModeEnum)
    {
        if (learnSentencesModeEnum == LearnSentencesModeEnum.MainLanguage)
        {
            return SentenceToLearnLabelEnum.Second;
        }
        else if (learnSentencesModeEnum == LearnSentencesModeEnum.LearnLanguage)
        {
            return SentenceToLearnLabelEnum.First;
        }
        else if (learnSentencesModeEnum == LearnSentencesModeEnum.Random)
        {
            List<SentenceToLearnLabelEnum> label = new List<SentenceToLearnLabelEnum>()
            {
                SentenceToLearnLabelEnum.First, SentenceToLearnLabelEnum.Second
            };
            Random random = new Random();

            // Генерируем случайный индекс
            int randomIndex = random.Next(0, label.Count);

            // Получаем элемент списка по случайному индексу
            SentenceToLearnLabelEnum randomElement = label[randomIndex];

            return randomElement;
        }
        else if (learnSentencesModeEnum == LearnSentencesModeEnum.Learned)
        {
            if (isLearning)
            {
                return SentenceToLearnLabelEnum.Second;
            }
            else
            {
                return SentenceToLearnLabelEnum.First;
            }
        }

        throw new ArgumentException("not correct learnSentencesModeEnum value");
    }
}
