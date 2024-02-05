using DropWord.Application.Common.Interfaces;
using DropWord.Application.Manager.Sentence;

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
    private readonly ISentenceManager _sentenceManager;

    public RepeatSentenceCommandHandler(IApplicationDbContext context, ISentenceManager sentenceManager)
    {
        _context = context;
        _sentenceManager = sentenceManager;
    }

    public async Task Handle(RepeatSentenceCommand request, CancellationToken cancellationToken)
    {
        await _sentenceManager.RepeatSentenceAsync(request.UserId, request.IsLearn, request.UsingSentencesPairId,
            cancellationToken);
    }
}
