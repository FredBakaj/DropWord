using DropWord.Application.Common.Interfaces;

namespace DropWord.Application.UseCase.Sentence.Commands.ResetCountRepeatSentence;

public record ResetCountRepeatSentenceCommand : IRequest
{
    public long UserId { get; set; }
}

public class ResetCountRepeatSentenceCommandValidator : AbstractValidator<ResetCountRepeatSentenceCommand>
{
    public ResetCountRepeatSentenceCommandValidator()
    {
    }
}

public class ResetCountRepeatSentenceCommandHandler : IRequestHandler<ResetCountRepeatSentenceCommand>
{
    private readonly IApplicationDbContext _context;

    public ResetCountRepeatSentenceCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(ResetCountRepeatSentenceCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .Include(x => x.UserLearningInfo)
            .FirstOrDefaultAsync(x => x.Id == request.UserId);

        user!.UserLearningInfo.LastUseForDaySentencesId = null;
        user!.UserLearningInfo.CountUseForDaySentences = null;
        await _context.SaveChangesAsync(cancellationToken);
    }
}
