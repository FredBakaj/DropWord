using DropWord.Application.Common.Interfaces;

namespace DropWord.Application.UseCase.Language.Commands.ChangeLanguagePair;

public record ChangeLanguagePairCommand : IRequest
{
    public long UserId { get; set; }
    public string FirstLanguage { get; set; } = null!;
    public string SecondLanguage { get; set; } = null!;
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

        user.UserSettings.FirstLanguage = request.FirstLanguage;
        user.UserSettings.SecondLanguage = request.SecondLanguage;

        await _context.SaveChangesAsync(cancellationToken: cancellationToken);
    }
}
