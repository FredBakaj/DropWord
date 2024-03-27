using System.Globalization;
using DropWord.Application.Common.Interfaces;
using DropWord.Application.Common.Interfaces.Sentence;
using DropWord.Application.Manager.Sentence;
using DropWord.Domain.Enums;

namespace DropWord.Application.UseCase.Sentence.Commands.SentencesRepeatForDay;

public record SentencesRepeatForDayCommand : IRequest<StatusOfSentenceInputEnum>
{
    public long UserId { get; set; }
    public int UsingSentencesPairId { get; set; }
    public SentenceToLearnLabelEnum SentenceToLearnLabelEnum { get; set; }
    public string Sentence { get; set; } = null!;
}

public class SentencesRepeatForDayCommandValidator : AbstractValidator<SentencesRepeatForDayCommand>
{
    public SentencesRepeatForDayCommandValidator()
    {
    }
}

public class SentencesRepeatForDayCommandHandler : IRequestHandler<SentencesRepeatForDayCommand, StatusOfSentenceInputEnum>
{
    private readonly IApplicationDbContext _context;
    private readonly ISentenceManager _sentenceManager;
    private readonly IDifferenceSentence _differenceSentence;
    private readonly IConfig _config;
    private readonly double _coefficientDiffSentencePercent;


    public SentencesRepeatForDayCommandHandler(IApplicationDbContext context,
        ISentenceManager sentenceManager, IDifferenceSentence differenceSentence, IConfig config)
    {
        _context = context;
        _sentenceManager = sentenceManager;
        _differenceSentence = differenceSentence;
        _config = config;
        _coefficientDiffSentencePercent = double.Parse(config.GetValue("CoefficientDiffSentencePercent"), CultureInfo.InvariantCulture);

    }

    public async Task<StatusOfSentenceInputEnum> Handle(SentencesRepeatForDayCommand request, CancellationToken cancellationToken)
    {
        var sentencesPair = await _sentenceManager.GetSentencesPairAsync(request.UserId, request.UsingSentencesPairId);
        var sentenceLearn = _sentenceManager.GetSentenceLearnFromPair(sentencesPair, request.SentenceToLearnLabelEnum);
        
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
