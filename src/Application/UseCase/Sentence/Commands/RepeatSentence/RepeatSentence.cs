using DropWord.Application.Common.Interfaces;

namespace DropWord.Application.UseCase.Sentence.Commands.RepeatSentence;

public record RepeatSentenceCommand : IRequest
{
    public long UserId { get; set; }
    public bool IsLearn { get; set; }
    public int UsingSentencesPairId { get; set; }
}

public class RepeatSentenceCommandValidator : AbstractValidator<RepeatSentenceCommand>
{
    public RepeatSentenceCommandValidator()
    {
    }
}

public class RepeatSentenceCommandHandler : IRequestHandler<RepeatSentenceCommand>
{
    private readonly IApplicationDbContext _context;

    public RepeatSentenceCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(RepeatSentenceCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .Where(x => x.Id == request.UserId)
            .Include(x => x.UserLearningInfo)
            .Include(x => x.UsingSentencesPairs)
            .Select(x => new
            {
                User = x,
                UsingSentencesPair = x.UsingSentencesPairs
                    .Where(y => y.Id == request.UsingSentencesPairId)
                    .FirstOrDefault()
            })
            .FirstOrDefaultAsync();

        if (user!.User != null && user!.UsingSentencesPair != null)
        {
            if (user.User.UserLearningInfo.CountUseForDaySentences == null ||
                user.User.UserLearningInfo.LastUseForDaySentencesId == null)
            {
                user.User.UserLearningInfo.CountUseForDaySentences = 1;
            }
            else
            {
                user.User.UserLearningInfo.CountUseForDaySentences += 1;
            }

            user.User.UserLearningInfo.LastUseForDaySentencesId = request.UsingSentencesPairId;
            user.UsingSentencesPair.CountUse += 1;
            user.UsingSentencesPair.IsLearning = request.IsLearn;
            user.UsingSentencesPair.UpdateDate = DateTime.Now;
            
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
