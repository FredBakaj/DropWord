using DropWord.Application.Common.Interfaces;
using DropWord.Application.Manager.Sentence;

namespace DropWord.Application.UseCase.Sentence.Commands.SentenceRepeatForDayCard;

public record SentenceRepeatForDayCardCommand : IRequest
{
    public long UserId { get; set; }
    public int UsingSentencesPairId { get; set; }
}

// public class SentenceRepeatForDayCardCommandValidator : AbstractValidator<SentenceRepeatForDayCardCommand>
// {
//     public SentenceRepeatForDayCardCommandValidator()
//     {
//     }
// }

public class SentenceRepeatForDayCardCommandHandler : IRequestHandler<SentenceRepeatForDayCardCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly ISentenceManager _sentenceManager;

    public SentenceRepeatForDayCardCommandHandler(IApplicationDbContext context, ISentenceManager sentenceManager)
    {
        _context = context;
        _sentenceManager = sentenceManager;
    }

    public async Task Handle(SentenceRepeatForDayCardCommand request, CancellationToken cancellationToken)
    {
        await _sentenceManager.RepeatSentenceAsync(request.UserId, false, request.UsingSentencesPairId,
            cancellationToken);
    }
}
