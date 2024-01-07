using DropWord.Application.Common.Interfaces;
using DropWord.Domain.Entities;
using DropWord.Infrastructure.Common.Enum;

namespace DropWord.Application.UseCase.User.Commands.InitializeUser;

public record InitializeUserCommand : IRequest
{
    public long UserId { get; set; }
    public string State { get; set; } = null!;
    public string Action { get; set; } = null!;
    public string InterfaceLanguage { get; set; } = null!;
}

public class InitializeUserCommandValidator : AbstractValidator<InitializeUserCommand>
{
    public InitializeUserCommandValidator()
    {
    }
}

public class InitializeUserCommandHandler : IRequestHandler<InitializeUserCommand>
{
    private readonly IApplicationDbContext _context;

    public InitializeUserCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(InitializeUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .Where(x => x.Id == request.UserId)
            .FirstOrDefaultAsync(cancellationToken);
        if (user == null)
        {
            var stateTree = new StateTreeEntity()
            {
                State = request.State, 
                Action = request.Action,
                JsonData = String.Empty
            };
            var userSettings = new UserSettingsEntity()
            {
                FirstLanguage = String.Empty,
                SecondLanguage = String.Empty,
                InterfaceLanguage = request.InterfaceLanguage,
                HideLanguageEnum = HideLanguageEnum.FirstLanguage,
            };
            var user_ = new UserEntity()
            {
                Id = request.UserId, 
                StateTree = stateTree, 
                UserSettings = userSettings
            };

            _context.Users.Add(user_);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
