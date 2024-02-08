using DropWord.Application.Common.Interfaces;

namespace DropWord.Application.UseCase.Sentence.Queries.GetSentencesPair;

public record GetSentencesPairQuery : IRequest<SentencesPairDto>
{
    public long UserId { get; set; }
    public int SentencesPairId { get; set; }
}

public class GetSentencesPairQueryValidator : AbstractValidator<GetSentencesPairQuery>
{
    public GetSentencesPairQueryValidator()
    {
    }
}

public class GetSentencesPairQueryHandler : IRequestHandler<GetSentencesPairQuery, SentencesPairDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetSentencesPairQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<SentencesPairDto> Handle(GetSentencesPairQuery request, CancellationToken cancellationToken)
    {
        var sentencePair = await _context.SentencesPair
            .Include(x => x.FirstSentence)
            .Include(x => x.SecondSentence)
            .Where(x => x.UserId == request.UserId && x.Id == request.SentencesPairId)
            .ProjectTo<SentencesPairDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();

        return sentencePair!;
    }
}
