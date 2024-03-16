using DropWord.Application.Common.Interfaces;
using Newtonsoft.Json;

namespace DropWord.Application.UseCase.StateTree.Commands.SetData;

public record SetDataCommand : IRequest
{
    public long UserId { get; set; }
    public object Data { get; set; } = null!;
}

public class SetDataCommandValidator : AbstractValidator<SetDataCommand>
{
    public SetDataCommandValidator()
    {
    }
}

public class SetDataCommandHandler : IRequestHandler<SetDataCommand>
{
    private readonly IApplicationDbContext _context;

    public SetDataCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(SetDataCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .Include(x => x.StateTree)
            .Where(x => x.Id == request.UserId)
            .FirstAsync(cancellationToken);

        user.StateTree.JsonData = JsonConvert.SerializeObject(request.Data);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
