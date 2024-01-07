using DropWord.Application.Common.Interfaces;
using Newtonsoft.Json;

namespace DropWord.Application.UseCase.StateTree.Commands.SetDataAndAction;

public record SetDataAndActionCommand : IRequest
{
    public long UserId { get; set; }
    public object Data { get; set; } = null!;
    public string Action { get; set; } = null!;
}

public class SetDataAndActionCommandHandler : IRequestHandler<SetDataAndActionCommand>
{
    private readonly IApplicationDbContext _context;

    public SetDataAndActionCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(SetDataAndActionCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .Include(x => x.StateTree)
            .Where(x => x.Id == request.UserId)
            .FirstAsync(cancellationToken);

        user.StateTree.JsonData = JsonConvert.SerializeObject(request.Data);
        user.StateTree.Action = request.Action;

        await _context.SaveChangesAsync(cancellationToken);
    }
}
