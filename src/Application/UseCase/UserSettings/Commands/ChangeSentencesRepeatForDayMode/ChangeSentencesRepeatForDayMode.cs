using DropWord.Application.Common.Interfaces;
using DropWord.Domain.Enums;

namespace DropWord.Application.UseCase.UserSettings.Commands.ChangeSentencesRepeatForDayMode;

public record ChangeSentencesRepeatForDayModeCommand : IRequest
{
    public long UserId { get; set; }
    public SentencesRepeatForDayTimesModeEnum SentencesRepeatForDayTimesModeEnum { get; set; }
}

public class ChangeSentencesRepeatForDayModeCommandValidator : AbstractValidator<ChangeSentencesRepeatForDayModeCommand>
{
    public ChangeSentencesRepeatForDayModeCommandValidator()
    {
    }
}

public class ChangeSentencesRepeatForDayModeCommandHandler : IRequestHandler<ChangeSentencesRepeatForDayModeCommand>
{
    private readonly IApplicationDbContext _context;

    public ChangeSentencesRepeatForDayModeCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(ChangeSentencesRepeatForDayModeCommand request, CancellationToken cancellationToken)
    {
        var userSettings = await _context.UserSettings.FirstAsync(x => x.UserId == request.UserId);
        userSettings.SentencesRepeatForDayTimesModeEnum = request.SentencesRepeatForDayTimesModeEnum;
        await _context.SaveChangesAsync(cancellationToken);
    }
}
