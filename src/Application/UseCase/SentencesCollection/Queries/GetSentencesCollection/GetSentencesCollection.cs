using DropWord.Application.Common.Interfaces;

namespace DropWord.Application.UseCase.SentencesCollection.Queries.GetSentencesCollection;

public record GetSentencesCollectionQuery : IRequest<SentencesCollectionDto>
{
    public int SentencesCollectionId { get; set; }
}

public class GetSentencesCollectionQueryValidator : AbstractValidator<GetSentencesCollectionQuery>
{
    public GetSentencesCollectionQueryValidator()
    {
    }
}

public class GetSentencesCollectionQueryHandler : IRequestHandler<GetSentencesCollectionQuery, SentencesCollectionDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetSentencesCollectionQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<SentencesCollectionDto> Handle(GetSentencesCollectionQuery request, CancellationToken cancellationToken)
    {
        var result = await _context.UserSentencesCollection
            .Where(x => x.Id == request.SentencesCollectionId)
            .ProjectTo<SentencesCollectionDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();

        return result!;
    }
}
