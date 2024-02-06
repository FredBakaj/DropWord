using DropWord.Application.Common.Interfaces;
using System.Text.Json;
namespace DropWord.Application.UseCase.StateTree.Commands.SetTempData;

public record SetTempDataCommand : IRequest
{
    public long UserId { get; set; }
    public object Data { get; set; } = null!;

}

public class SetTempDataCommandValidator : AbstractValidator<SetTempDataCommand>
{
    public SetTempDataCommandValidator()
    {
    }
}

public class SetTempDataCommandHandler : IRequestHandler<SetTempDataCommand>
{
    private readonly IApplicationDbContext _context;

    public SetTempDataCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(SetTempDataCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .Include(x => x.StateTree)
            .Where(x => x.Id == request.UserId)
            .FirstAsync(cancellationToken);

        user.StateTree.JsonTempData = JsonSerializer.Serialize(request.Data);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
