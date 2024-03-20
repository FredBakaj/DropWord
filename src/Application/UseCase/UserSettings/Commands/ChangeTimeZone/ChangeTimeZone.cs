using DropWord.Application.Common.Interfaces;

namespace DropWord.Application.UseCase.UserSettings.Commands.ChangeTimeZone;

public record ChangeTimeZoneCommand : IRequest
{
    public long UserId { get; set; }
    public int TimeZone { get; set; }
}

public class ChangeTimeZoneCommandValidator : AbstractValidator<ChangeTimeZoneCommand>
{
    public ChangeTimeZoneCommandValidator()
    {
    }
}

public class ChangeTimeZoneCommandHandler : IRequestHandler<ChangeTimeZoneCommand>
{
    private readonly IApplicationDbContext _context;

    public ChangeTimeZoneCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(ChangeTimeZoneCommand request, CancellationToken cancellationToken)
    {
        var userSettings = await _context.UserSettings.FirstAsync(x => x.UserId == request.UserId);
        userSettings.TimeZone = request.TimeZone;
        await _context.SaveChangesAsync(cancellationToken);
    }
}
