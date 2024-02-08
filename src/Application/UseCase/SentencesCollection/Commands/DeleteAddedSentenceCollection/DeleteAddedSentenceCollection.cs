using DropWord.Application.Common.Interfaces;
using DropWord.Domain.Exceptions;

namespace DropWord.Application.UseCase.SentencesCollection.Commands.DeleteAddedSentenceCollection;

public record DeleteAddedSentenceCollectionCommand : IRequest
{
    public long UserId { get; set; }
    public int CollectionId { get; set; }
}

public class DeleteAddedSentenceCollectionCommandValidator : AbstractValidator<DeleteAddedSentenceCollectionCommand>
{
    public DeleteAddedSentenceCollectionCommandValidator()
    {
    }
}

public class DeleteAddedSentenceCollectionCommandHandler : IRequestHandler<DeleteAddedSentenceCollectionCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteAddedSentenceCollectionCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteAddedSentenceCollectionCommand request, CancellationToken cancellationToken)
    {
        var collection = await _context.UserSentencesCollection
            .Include(x => x.SentencesPairs)
            .ThenInclude(x => x.UsingSentencesPairs)
            .Where(x => x.UserId == request.UserId && x.Id == request.CollectionId)
            .FirstOrDefaultAsync();

        if (collection == null)
        {
            throw new NotFoundCollectionException(
                $"collection {request.CollectionId} for user {request.UserId} was not found");
        }

        foreach (var item in collection!.SentencesPairs)
        {
            item.WhenDeleted = DateTimeOffset.Now;
            foreach (var secondItem in item.UsingSentencesPairs)
            {
                secondItem.WhenDeleted = DateTimeOffset.Now;
            }
        }

        collection.WhenDeleted = DateTimeOffset.Now;

        await _context.SaveChangesAsync(cancellationToken);
    }
}
