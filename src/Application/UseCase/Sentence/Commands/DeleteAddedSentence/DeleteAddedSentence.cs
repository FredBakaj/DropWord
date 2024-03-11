using DropWord.Application.Common.Interfaces;

namespace DropWord.Application.UseCase.Sentence.Commands.DeleteAddedSentence;

public record DeleteAddedSentenceCommand : IRequest
{
    public long UserId { get; set; }
    public int SentencesPairId { get; set; }
}

// public class DeleteAddedSentenceCommandValidator : AbstractValidator<DeleteAddedSentenceCommand>
// {
//     public DeleteAddedSentenceCommandValidator()
//     {
//     }
// }

public class DeleteAddedSentenceCommandHandler : IRequestHandler<DeleteAddedSentenceCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteAddedSentenceCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteAddedSentenceCommand request, CancellationToken cancellationToken)
    {
        var sentencesPair = await _context.SentencesPair
            .Where(x => x.UserId == request.UserId && x.Id == request.SentencesPairId)
            .FirstOrDefaultAsync();

        sentencesPair!.WhenDeleted = DateTimeOffset.UtcNow;
        await _context.SaveChangesAsync(cancellationToken);
    }
}
