using DropWord.Application.Common.Interfaces;
using DropWord.Domain.Enums;

namespace DropWord.Application.UseCase.Sentence.Queries.GetNewSentence;

public record GetNewSentenceQuery : IRequest<NewSentenceDto>
{
    public long UserId { get; set; }
}

public class GetNewSentenceQueryValidator : AbstractValidator<GetNewSentenceQuery>
{
    public GetNewSentenceQueryValidator()
    {
    }
}

public class GetNewSentenceQueryHandler : IRequestHandler<GetNewSentenceQuery, NewSentenceDto>
{
    private readonly IApplicationDbContext _context;

    public GetNewSentenceQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<NewSentenceDto> Handle(GetNewSentenceQuery request, CancellationToken cancellationToken)
    {
        var userCollection = _context.Users
            .Include(x => x.UserSettings)
            .Include(x => x.SentencesPairs)
            .Include(x => x.UsingSentencesPairs)
            .Where(x => x.Id == request.UserId);

        var userSettings = await userCollection.Select(x => x.UserSettings).FirstAsync();

        var sentencesPair = await userCollection
            .SelectMany(x => x.SentencesPairs)
            .Where(x => !x.UsingSentencesPairs
                .Any(y => y.SentencesPairId == x.Id)
                && x.FirstLanguage == userSettings.MainLanguage
                && x.SecondLanguage == userSettings.LearnLanguage)
            .FirstAsync();

        var sentences = await _context.SentencesPair
            .Include(x => x.FirstSentence)
            .Include(x => x.SecondSentence)
            .Where(x => x.Id == sentencesPair.Id)
            .FirstAsync();

        var result = new NewSentenceDto()
        {
            SentencePairId = sentences.Id,
            FirstSentence = sentences.FirstSentence.Sentence,
            SecondSentence = sentences.SecondSentence.Sentence,
            SentenceToLearnLabel = SentenceToLearnLabelEnum.First
            
        };
        return result;
    }
}
