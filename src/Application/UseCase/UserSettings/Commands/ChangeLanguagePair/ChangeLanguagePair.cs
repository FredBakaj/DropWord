using DropWord.Application.Common.Interfaces;

namespace DropWord.Application.UseCase.UserSettings.Commands.ChangeLanguagePair;

public record ChangeLanguagePairCommand : IRequest
{
    public long UserId { get; set; }
    public string MainLanguage { get; set; } = null!;
    public string LearnLanguage { get; set; } = null!;
}

public class ChangeLanguagePairCommandValidator : AbstractValidator<ChangeLanguagePairCommand>
{
    public ChangeLanguagePairCommandValidator()
    {
    }
}

public class ChangeLanguagePairCommandHandler : IRequestHandler<ChangeLanguagePairCommand>
{
    private readonly IApplicationDbContext _context;

    public ChangeLanguagePairCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(ChangeLanguagePairCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .Include(x => x.UserSettings)
            .Where(x => x.Id == request.UserId)
            .FirstAsync(cancellationToken);

        user.UserSettings.MainLanguage = request.MainLanguage;
        user.UserSettings.LearnLanguage = request.LearnLanguage;

        await _context.SaveChangesAsync(cancellationToken: cancellationToken);
    }
}
