using DropWord.Application.Common.Interfaces;
using DropWord.Domain.Constants;
using DropWord.Domain.Entities;
using DropWord.Domain.Enums;

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
                JsonData = String.Empty,
                JsonTempData = String.Empty,
            };
            var userSettings = new UserSettingsEntity()
            {
                MainLanguage = LanguageConst.Ukrainian,
                LearnLanguage = LanguageConst.English,
                InterfaceLanguage = request.InterfaceLanguage,
                LearnSentencesModeEnum = LearnSentencesModeEnum.MainLanguage,
                SentencesRepeatForDayTimesModeEnum = SentencesRepeatForDayTimesModeEnum.Times1InDay,
                SentencesRepeatForDayModeEnum = SentencesRepeatForDayModeEnum.Card
                
            };

            var userLearningInfo = new UserLearningInfoEntity()
            {
                CountUseForDaySentences = null,
                LastUseForDaySentencesId = null,
            };
            var user_ = new UserEntity()
            {
                Id = request.UserId,
                //TODO добавить генерацию имени
                Name = "Farilder",
                StateTree = stateTree, 
                UserSettings = userSettings,
                UserLearningInfo = userLearningInfo,
            };

            _context.Users.Add(user_);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
