using DropWord.Application.Common.Interfaces;
using Newtonsoft.Json;

namespace DropWord.Application.UseCase.StateTree.Queries.GetData;

public record GetDataQuery : IRequest<string>
{
    public long UserId { get; set; }
}

public class GetDataQueryValidator : AbstractValidator<GetDataQuery>
{
    public GetDataQueryValidator()
    {
    }
}

public class GetDataQueryHandler : IRequestHandler<GetDataQuery, string>
{
    private readonly IApplicationDbContext _context;

    public GetDataQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<string> Handle(GetDataQuery request, CancellationToken cancellationToken)
    {
        var result = await _context.StateTree.Where(x => x.UserId == request.UserId).FirstAsync(cancellationToken);
        return result.JsonData;
    }
}
