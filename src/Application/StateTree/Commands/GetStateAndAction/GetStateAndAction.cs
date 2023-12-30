using DropWord.Application.Common.Interfaces;

namespace DropWord.Application.StateTree.Commands.GetStateAndAction;

public record GetStateAndActionCommand : IRequest<object>
{
    public long UserId { get; set; }
}

// public class GetStateAndActionCommandValidator : AbstractValidator<GetStateAndActionCommand>
// {
//     public GetStateAndActionCommandValidator()
//     {
//     }
// }

public class GetStateAndActionCommandHandler : IRequestHandler<GetStateAndActionCommand, object>
{
    private readonly IApplicationDbContext _context;

    public GetStateAndActionCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<object> Handle(GetStateAndActionCommand request, CancellationToken cancellationToken)
    {
        var result = await _context.StateTree.Where(x => x.UserId == request.UserId).FirstOrDefaultAsync();
        return result!;
    }
}
