using DropWord.Application.Common.Interfaces;
using DropWord.Domain.Enums;
using DropWord.Domain.Exceptions;

namespace DropWord.Application.UseCase.UserSettings.Commands.ChangeLearnSentencesMode;

public record ChangeLearnSentencesModeCommand : IRequest<UserDto>
{
    public long UserId { get; set; }
}

public class ChangeLearnSentencesModeCommandValidator : AbstractValidator<ChangeLearnSentencesModeCommand>
{
    public ChangeLearnSentencesModeCommandValidator()
    {
    }
}

public class ChangeLearnSentencesModeCommandHandler : IRequestHandler<ChangeLearnSentencesModeCommand, UserDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public ChangeLearnSentencesModeCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<UserDto> Handle(ChangeLearnSentencesModeCommand request, CancellationToken cancellationToken)
    {
        var changeModeDict = new Dictionary<LearnSentencesModeEnum, LearnSentencesModeEnum>()
        {
            { LearnSentencesModeEnum.MainLanguage, LearnSentencesModeEnum.LearnLanguage },
            { LearnSentencesModeEnum.LearnLanguage, LearnSentencesModeEnum.Learned },
            { LearnSentencesModeEnum.Learned, LearnSentencesModeEnum.Random },
            { LearnSentencesModeEnum.Random, LearnSentencesModeEnum.MainLanguage },
        };
        //TODO вынести получение пользователя в менеджер, и использовать его в тех местах где есть похожий код
        var user = await _context.Users
            .Include(x => x.UserSettings)
            .Where(x => x.Id == request.UserId)
            .FirstOrDefaultAsync();

        if (user == null)
        {
            throw new NotFoundUserException("not found user");
        }

        user.UserSettings.LearnSentencesModeEnum = changeModeDict[user.UserSettings.LearnSentencesModeEnum];
        await _context.SaveChangesAsync(cancellationToken);
        return _mapper.Map<UserDto>(user);
    }
}
