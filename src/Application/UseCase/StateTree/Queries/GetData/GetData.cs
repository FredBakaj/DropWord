using DropWord.Application.Common.Interfaces;
using Newtonsoft.Json;

namespace DropWord.Application.UseCase.StateTree.Queries.GetData;

public record GetDataQuery : IRequest<object>
{
    public long UserId { get; set; }
}

public class GetDataQueryValidator : AbstractValidator<GetDataQuery>
{
    public GetDataQueryValidator()
    {
    }
}

public class GetDataQueryHandler : IRequestHandler<GetDataQuery, object>
{
    private readonly IApplicationDbContext _context;

    public GetDataQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<object> Handle(GetDataQuery request, CancellationToken cancellationToken)
    {
        var result = await _context.StateTree.Where(x => x.UserId == request.UserId).FirstAsync(cancellationToken);

        return JsonConvert.DeserializeObject<object>(result.JsonData)!;
    }
}
