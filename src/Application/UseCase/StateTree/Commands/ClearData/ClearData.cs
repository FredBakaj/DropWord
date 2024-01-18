using DropWord.Application.Common.Interfaces;

namespace DropWord.Application.UseCase.StateTree.Commands.ClearData;

public record ClearDataCommand : IRequest
{
    public long UserId { get; set; }
}

public class ClearDataCommandValidator : AbstractValidator<ClearDataCommand>
{
    public ClearDataCommandValidator()
    {
    }
}

public class ClearDataCommandHandler : IRequestHandler<ClearDataCommand>
{
    private readonly IApplicationDbContext _context;

    public ClearDataCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(ClearDataCommand request, CancellationToken cancellationToken)
    {
        var stateTree = await _context.StateTree
            .Where(x => x.UserId == request.UserId)
            .FirstOrDefaultAsync();
        stateTree!.JsonData = string.Empty;
        await _context.SaveChangesAsync(cancellationToken);
    }
}
