using DropWord.Application.Common.Interfaces;

namespace DropWord.Application.UseCase.StateTree.Commands.SetStateAndAction;

public record SetStateAndActionCommand : IRequest
{
    public long UserId { get; set; }
    public string State { get; set; } = null!;
    public string Action { get; set; } = null!;
}

public class SetStateAndActionCommandValidator : AbstractValidator<SetStateAndActionCommand>
{
    public SetStateAndActionCommandValidator()
    {
    }
}

public class SetStateAndActionCommandHandler : IRequestHandler<SetStateAndActionCommand>
{
    private readonly IApplicationDbContext _context;

    public SetStateAndActionCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(SetStateAndActionCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .Include(x => x.StateTree)
            .Where(x => x.Id == request.UserId)
            .FirstAsync(cancellationToken);

        user.StateTree.State = request.State;
        user.StateTree.Action = request.Action;

        await _context.SaveChangesAsync(cancellationToken);
    }
}
