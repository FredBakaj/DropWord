using DropWord.Application.Common.Interfaces;

namespace DropWord.Application.UseCase.StateTree.Commands.ClearTempData;

public record ClearTempDataCommand : IRequest
{
    public long UserId { get; set; }
}

public class ClearTempDataCommandValidator : AbstractValidator<ClearTempDataCommand>
{
    public ClearTempDataCommandValidator()
    {
    }
}

public class ClearTempDataCommandHandler : IRequestHandler<ClearTempDataCommand>
{
    private readonly IApplicationDbContext _context;

    public ClearTempDataCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(ClearTempDataCommand request, CancellationToken cancellationToken)
    {
        var stateTree = await _context.StateTree
            .Where(x => x.UserId == request.UserId)
            .FirstOrDefaultAsync();
        stateTree!.JsonTempData = string.Empty;
        await _context.SaveChangesAsync(cancellationToken);
    }
}
