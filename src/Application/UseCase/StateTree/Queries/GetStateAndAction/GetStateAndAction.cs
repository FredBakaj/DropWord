using DropWord.Application.Common.Interfaces;

namespace DropWord.Application.UseCase.StateTree.Queries.GetStateAndAction;

public record GetStateAndActionQuery : IRequest<StateAndActionDto>
{
    public long UserId { get; set; }
}

public class GetStateAndActionQueryValidator : AbstractValidator<GetStateAndActionQuery>
{
    public GetStateAndActionQueryValidator()
    {
    }
}

public class GetStateAndActionQueryHandler : IRequestHandler<GetStateAndActionQuery, StateAndActionDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetStateAndActionQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<StateAndActionDto> Handle(GetStateAndActionQuery request, CancellationToken cancellationToken)
    {
        var result = await _context.StateTree
            .Where(x => x.UserId == request.UserId)
            .ProjectTo<StateAndActionDto>(_mapper.ConfigurationProvider)
            .FirstAsync(cancellationToken);
        return result!;
    }
}
