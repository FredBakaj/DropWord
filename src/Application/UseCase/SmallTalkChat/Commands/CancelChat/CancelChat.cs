using DropWord.Application.Common.Interfaces;

namespace DropWord.Application.UseCase.SmallTalkChat.Commands.CancelChat;

public record CancelChatCommand : IRequest
{
    public long UserId { get; set; }
}

public class CancelChatCommandValidator : AbstractValidator<CancelChatCommand>
{
    public CancelChatCommandValidator()
    {
    }
}

public class CancelChatCommandHandler : IRequestHandler<CancelChatCommand>
{
    private readonly IApplicationDbContext _context;

    public CancelChatCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(CancelChatCommand request, CancellationToken cancellationToken)
    {
        var chat = await _context.AutoChatData
            .Where(x => x.UserId == request.UserId 
                        && !x.IsClosed)
            .Take(10)
            .ToListAsync();

        chat.ForEach(x => x.IsClosed = true);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
