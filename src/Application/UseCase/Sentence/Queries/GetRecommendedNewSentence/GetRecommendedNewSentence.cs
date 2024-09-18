using DropWord.Application.Common.Interfaces;
using DropWord.Application.Manager.Sentence;
using DropWord.Application.Manager.Sentence.Implementation.Model;
using DropWord.Domain.Constants;
using DropWord.Domain.Entities;
using DropWord.Domain.Enums;

namespace DropWord.Application.UseCase.Sentence.Queries.GetRecommendedNewSentence;

public record GetRecommendedNewSentenceQuery : IRequest<RecommendedNewSentenceDto>
{
    public long UserId { get; set; }
}

public class GetRecommendedNewSentenceQueryValidator : AbstractValidator<GetRecommendedNewSentenceQuery>
{
    public GetRecommendedNewSentenceQueryValidator()
    {
    }
}

public class
    GetRecommendedNewSentenceQueryHandler : IRequestHandler<GetRecommendedNewSentenceQuery, RecommendedNewSentenceDto>
{
    private readonly IApplicationDbContext _context;
    private readonly ISentenceManager _sentenceManager;

    public GetRecommendedNewSentenceQueryHandler(IApplicationDbContext context, ISentenceManager sentenceManager)
    {
        _context = context;
        _sentenceManager = sentenceManager;
    }

    public async Task<RecommendedNewSentenceDto> Handle(GetRecommendedNewSentenceQuery request,
        CancellationToken cancellationToken)
    {
        // var result = new RecommendedNewSentenceDto()
        // {
        //     Id = -1,
        //     FirstLanguage = LanguageConst.English,
        //     SecondLanguage = LanguageConst.Ukrainian,
        //     FirstSentence = "Hello world!",
        //     SecondSentence = "Привіт світ!",
        //     SentenceToLearnLabel = SentenceToLearnLabelEnum.First
        // };
        var result = await GetRecommendedNewSentenceAsync(request.UserId);
        return await Task.FromResult(result);
    }

    private async Task<RecommendedNewSentenceDto> GetRecommendedNewSentenceAsync(long userId)
    {
        UserSettingsEntity userSettings = (await _context.UserSettings
            .Where(x => x.UserId == userId)
            .FirstOrDefaultAsync())!;


        RecommendedNewConnectionSentenceEntity recommendedSentence = (await _context.RecommendedNewConnectionSentence
            .Include(x => x.RecommendedNewConnectionWithUsers)
            .Include(x => x.RecommendedNewFirstSentence)
            .Include(x => x.RecommendedNewSecondSentence)
            .Where(x => x.RecommendedNewConnectionWithUsers.All(y => y.UserId != userId)
                        && x.RecommendedNewFirstSentence.Language == userSettings.MainLanguage
                        && x.RecommendedNewSecondSentence.Language == userSettings.LearnLanguage)
            .FirstOrDefaultAsync())!;
        ;

        var sentenceToLearn = _sentenceManager.DetectSentenceToLearnLabel(false, userSettings.LearnSentencesModeEnum); 

        var result = new RecommendedNewSentenceDto()
        {
            Id = recommendedSentence.Id,
            FirstSentence = recommendedSentence.RecommendedNewFirstSentence.Sentence,
            FirstLanguage = recommendedSentence.RecommendedNewFirstSentence.Language,
            SecondSentence = recommendedSentence.RecommendedNewSecondSentence.Sentence,
            SecondLanguage = recommendedSentence.RecommendedNewSecondSentence.Language,
            SentenceToLearnLabel = sentenceToLearn,
        };
        
        return result;
    }
}
