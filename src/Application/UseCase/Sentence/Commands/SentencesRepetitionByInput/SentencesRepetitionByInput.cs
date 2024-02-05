using System.Globalization;
using DropWord.Application.Common.Interfaces;
using DropWord.Application.Common.Interfaces.Sentence;
using DropWord.Application.Manager.Sentence;
using DropWord.Domain.Enums;

namespace DropWord.Application.UseCase.Sentence.Commands.SentencesRepetitionByInput;

public record SentencesRepetitionByInputCommand : IRequest<StatusOfSentenceInputEnum>
{
    public long UserId { get; set; }
    public int UsingSentencesPairId { get; set; }
    public SentenceToLearnLabelEnum SentenceToLearnLabelEnum { get; set; }
    public string Sentence { get; set; } = null!;
}

public class SentencesRepetitionByInputCommandValidator : AbstractValidator<SentencesRepetitionByInputCommand>
{
    public SentencesRepetitionByInputCommandValidator()
    {
    }
}

public class
    SentencesRepetitionByInputCommandHandler : IRequestHandler<SentencesRepetitionByInputCommand,
        StatusOfSentenceInputEnum>
{
    private readonly IApplicationDbContext _context;
    private readonly ISentenceManager _sentenceManager;
    private readonly IDifferenceSentence _differenceSentence;
    private readonly IConfig _config;
    private readonly double _coefficientDiffSentencePercent;

    public SentencesRepetitionByInputCommandHandler(IApplicationDbContext context,
        ISentenceManager sentenceManager, IDifferenceSentence differenceSentence, IConfig config)
    {
        _context = context;
        _sentenceManager = sentenceManager;
        _differenceSentence = differenceSentence;
        _config = config;

        _coefficientDiffSentencePercent = double.Parse(config.GetValue("CoefficientDiffSentencePercent"), CultureInfo.InvariantCulture);
    }

    public async Task<StatusOfSentenceInputEnum> Handle(SentencesRepetitionByInputCommand request,
        CancellationToken cancellationToken)
    {
        var sentencePair = await _context.UsingSentencesPair
            .Include(x => x.SentencesPair)
            .ThenInclude(x => x.FirstSentence)
            .Include(x => x.SentencesPair)
            .ThenInclude(x => x.SecondSentence)
            .Where(x => x.UserId == request.UserId && x.Id == request.UsingSentencesPairId)
            .Select(x => x.SentencesPair)
            .FirstOrDefaultAsync();

        var sentenceLearn = string.Empty;
        if (request.SentenceToLearnLabelEnum == SentenceToLearnLabelEnum.First)
        {
            sentenceLearn = sentencePair!.FirstSentence.Sentence;
        }
        else if (request.SentenceToLearnLabelEnum == SentenceToLearnLabelEnum.Second)
        {
            sentenceLearn = sentencePair!.SecondSentence.Sentence;
        }

        if (sentenceLearn == request.Sentence)
        {
            await _sentenceManager.RepeatSentenceAsync(request.UserId, true, request.UsingSentencesPairId,
                cancellationToken);
            return StatusOfSentenceInputEnum.Right;
        }
        else if (_differenceSentence.DiffSentencePercent(sentenceLearn, request.Sentence) >
                 _coefficientDiffSentencePercent)
        {
            await _sentenceManager.RepeatSentenceAsync(request.UserId, false, request.UsingSentencesPairId,
                cancellationToken);
            return StatusOfSentenceInputEnum.InputWithErrors;
        }
        else
        {
            await _sentenceManager.RepeatSentenceAsync(request.UserId, false, request.UsingSentencesPairId,
                cancellationToken);
            return StatusOfSentenceInputEnum.Incorrect;
        }
    }
}
