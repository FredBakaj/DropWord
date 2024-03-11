using DropWord.Application.Common.Interfaces;

namespace DropWord.Application.UseCase.Sentence.Commands.UpdateStatusShowSentencesForRepeat;

public record UpdateStatusShowSentencesForRepeatCommand : IRequest
{
    public int UsingSentencesPairId { get; set; }
}

public class
    UpdateStatusShowSentencesForRepeatCommandValidator : AbstractValidator<UpdateStatusShowSentencesForRepeatCommand>
{
    public UpdateStatusShowSentencesForRepeatCommandValidator()
    {
    }
}

public class
    UpdateStatusShowSentencesForRepeatCommandHandler : IRequestHandler<UpdateStatusShowSentencesForRepeatCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateStatusShowSentencesForRepeatCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateStatusShowSentencesForRepeatCommand request, CancellationToken cancellationToken)
    {
        var usingSentencesPair =
            await _context.UsingSentencesPair
                .FirstOrDefaultAsync(x => x.Id == request.UsingSentencesPairId);
        usingSentencesPair!.UpdateDate = DateTime.UtcNow;
        await _context.SaveChangesAsync(cancellationToken: cancellationToken);
    }
}
