using DropWord.Application.Common.Interfaces;

namespace DropWord.Application.StateTree.Commands.SetStateAndAction;

public record SetStateAndActionCommand : IRequest<object>
{
}

public class SetStateAndActionCommandValidator : AbstractValidator<SetStateAndActionCommand>
{
    public SetStateAndActionCommandValidator()
    {
    }
}

public class SetStateAndActionCommandHandler : IRequestHandler<SetStateAndActionCommand, object>
{
    private readonly IApplicationDbContext _context;

    public SetStateAndActionCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public Task<object> Handle(SetStateAndActionCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
