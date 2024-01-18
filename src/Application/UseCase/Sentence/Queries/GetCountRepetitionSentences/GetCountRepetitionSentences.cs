using DropWord.Application.Common.Interfaces;

namespace DropWord.Application.UseCase.Sentence.Queries.GetCountRepetitionSentences;

public record GetCountRepetitionSentencesQuery : IRequest<CountRepetitionSentencesDto>
{
    public long UserId { get; set; }
}

public class GetCountRepetitionSentencesQueryValidator : AbstractValidator<GetCountRepetitionSentencesQuery>
{
    public GetCountRepetitionSentencesQueryValidator()
    {
    }
}

public class GetCountRepetitionSentencesQueryHandler : IRequestHandler<GetCountRepetitionSentencesQuery, CountRepetitionSentencesDto>
{
    private readonly IApplicationDbContext _context;

    public GetCountRepetitionSentencesQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CountRepetitionSentencesDto> Handle(GetCountRepetitionSentencesQuery request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .Include(x => x.UserLearningInfo)
            .FirstOrDefaultAsync();
        var result = new CountRepetitionSentencesDto() { Count = user!.UserLearningInfo.CountUseForDaySentences };
        return result;
    }
}
