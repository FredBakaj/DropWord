using DropWord.Application.Common.Interfaces;

namespace DropWord.Application.UseCase.Sentence.Commands.UpdateSentence;

public record UpdateSentenceCommand : IRequest<UpdateSentencesPairDto>
{
    public long UserId { get; set; }
    public int SentenceId { get; set; }
    public string Sentence { get; set; } = null!;
}

public class UpdateSentenceCommandValidator : AbstractValidator<UpdateSentenceCommand>
{
    public UpdateSentenceCommandValidator()
    {
    }
}

public class UpdateSentenceCommandHandler : IRequestHandler<UpdateSentenceCommand, UpdateSentencesPairDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public UpdateSentenceCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<UpdateSentencesPairDto> Handle(UpdateSentenceCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .Include(x => x.UserSettings)
            .Where(x => x.Id == request.UserId)
            .FirstAsync();

        var sentencesPair = await _context.SentencesPair
            .Include(x => x.FirstSentence)
            .Include(x => x.SecondSentence)
            .Where(x => x.FirstLanguage == user.UserSettings.MainLanguage
                        && x.SecondLanguage == user.UserSettings.LearnLanguage
                        && (x.FirstSentence.Id == request.SentenceId || x.SecondSentence.Id == request.SentenceId))
            .FirstOrDefaultAsync();

        if (sentencesPair!.FirstSentence.Id == request.SentenceId)
        {
            sentencesPair.FirstSentence.Sentence = request.Sentence;
        }
        else if (sentencesPair!.SecondSentence.Id == request.SentenceId)
        {
            sentencesPair.SecondSentence.Sentence = request.Sentence;
        }

        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<UpdateSentencesPairDto>(sentencesPair);
    }
}
