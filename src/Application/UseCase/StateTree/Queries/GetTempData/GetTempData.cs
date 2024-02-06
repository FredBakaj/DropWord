using DropWord.Application.Common.Interfaces;

namespace DropWord.Application.UseCase.StateTree.Queries.GetTempData;

public record GetTempDataQuery : IRequest<string>
{
    public long UserId { get; set; }
}

public class GetTempDataQueryValidator : AbstractValidator<GetTempDataQuery>
{
    public GetTempDataQueryValidator()
    {
    }
}

public class GetTempDataQueryHandler : IRequestHandler<GetTempDataQuery, string>
{
    private readonly IApplicationDbContext _context;

    public GetTempDataQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<string> Handle(GetTempDataQuery request, CancellationToken cancellationToken)
    {
        var result = await _context.StateTree.Where(x => x.UserId == request.UserId).FirstAsync(cancellationToken);
        return result.JsonTempData;
    }
}
