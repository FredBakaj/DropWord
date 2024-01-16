using DropWord.Application.Common.Interfaces;
using DropWord.Domain.Entities;

namespace DropWord.Application.UseCase.Sentence.Commands.LearnNewSentence;

public record LearnNewSentenceCommand : IRequest
{
    public long UserId { get; set; }
    public int SentencePairId { get; set; }
}

public class LearnNewSentenceCommandValidator : AbstractValidator<LearnNewSentenceCommand>
{
    public LearnNewSentenceCommandValidator()
    {
    }
}

public class LearnNewSentenceCommandHandler : IRequestHandler<LearnNewSentenceCommand>
{
    private readonly IApplicationDbContext _context;

    public LearnNewSentenceCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(LearnNewSentenceCommand request, CancellationToken cancellationToken)
    {
        var usingSentencePair = new UsingSentencesPairEntity()
        {
            UserId = request.UserId, SentencesPairId = request.SentencePairId, CountUse = 1, IsLearning = false,
        };
        
        _context.UsingSentencesPair.Add(usingSentencePair);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
