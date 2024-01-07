using DropWord.Application.Common.Interfaces;

namespace DropWord.Application.UseCase.StateTree.Commands.SetAction;

public record SetActionCommand : IRequest
{
    public long UserId { get; set; }
    public string Action { get; set; } = null!;
}

public class SetActionCommandValidator : AbstractValidator<SetActionCommand>
{
    public SetActionCommandValidator()
    {
    }
}

public class SetActionCommandHandler : IRequestHandler<SetActionCommand>
{
    private readonly IApplicationDbContext _context;

    public SetActionCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(SetActionCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .Include(x => x.StateTree)
            .Where(x => x.Id == request.UserId)
            .FirstAsync(cancellationToken);

        user.StateTree.Action = request.Action;

        await _context.SaveChangesAsync(cancellationToken);
    }
}
