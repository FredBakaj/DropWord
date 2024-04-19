using DropWord.Application.Common.Interfaces;
using DropWord.Application.Common.Interfaces.Sentence;
using DropWord.Application.Manager.Sentence;
using DropWord.Domain.Entities;
using DropWord.Domain.Exceptions;

namespace DropWord.Application.UseCase.Sentence.Commands.AddSentence;

public record AddSentenceCommand : IRequest<AddSentencePairDto>
{
    public long UserId { get; set; }
    public string Sentence { get; set; } = null!;
}

public class AddSentenceCommandValidator : AbstractValidator<AddSentenceCommand>
{
    public AddSentenceCommandValidator()
    {
    }
}

public class AddSentenceCommandHandler : IRequestHandler<AddSentenceCommand, AddSentencePairDto>
{
    private readonly IApplicationDbContext _context;
    private readonly ITranslate _translate;
    private readonly ISentenceManager _sentenceManager;

    public AddSentenceCommandHandler(IApplicationDbContext context, ITranslate translate, ISentenceManager sentenceManager)
    {
        _context = context;
        _translate = translate;
        _sentenceManager = sentenceManager;
    }

    public async Task<AddSentencePairDto> Handle(AddSentenceCommand request,
        CancellationToken cancellationToken)
    {
        if (!_sentenceManager.IsNotOneWord(request.Sentence))
        {
            throw new TryAddOneWordException("User try added one word");
        }
        //Проверка валидоно ли предложение для добавления
        if (!_sentenceManager.IsValidSentenceForAdd(request.Sentence))
        {
            throw new SentencesNotValidForAddException("Sentence is not valid");
        }
        
        //Проверка привышен ли лимит на добавленние предложений
        if (! await _sentenceManager.IsLimitAddSentencesExceededAsync(request.UserId))
        {
            throw new LimitAddSentencesExceededException("Limit for add sentences is exceeded");
        }
        
        var sentence = request.Sentence;
        // Определение языков предложений
        var detectLanguage = await _translate.DetectLanguageAsync(sentence);
        // Проверка на наличие больше одного языка в контенте
        var firstSentenceLanguage = detectLanguage.Language;

        //Перевод предложений
        var userSettings = await _context.UserSettings.FirstAsync(x => x.UserId == request.UserId);
        var translateTo = firstSentenceLanguage == userSettings.MainLanguage
            ? userSettings.LearnLanguage
            : userSettings.MainLanguage;

        var translateSentences = await _translate.TranslateAsync(sentence, firstSentenceLanguage, translateTo);

        //Сохранение предложений в БД
        var originalSentence =
            new SentenceEntity()
            {
                Language = translateSentences.OriginalLanguage, Sentence = translateSentences.OriginalSentence
            };
        var translateSentence =
            new SentenceEntity()
            {
                Language = translateSentences.TranslateLanguage, Sentence = translateSentences.TranslateSentence
            };

        SentencesPairEntity sentencePair = null!;
        if (translateSentences.OriginalLanguage == userSettings.MainLanguage)
        {
            sentencePair = new SentencesPairEntity()
            {
                FirstSentence = originalSentence,
                SecondSentence = translateSentence,
                FirstLanguage = translateSentences.OriginalLanguage,
                SecondLanguage = translateSentences.TranslateLanguage,
                UserId = request.UserId
            };
        }
        else if (translateSentences.TranslateLanguage == userSettings.MainLanguage)
        {
            sentencePair = new SentencesPairEntity()
            {
                FirstSentence = translateSentence,
                SecondSentence = originalSentence,
                FirstLanguage = translateSentences.TranslateLanguage,
                SecondLanguage = translateSentences.OriginalLanguage,
                UserId = request.UserId
            };
        }

        _context.SentencesPair.Add(sentencePair);

        await _context.SaveChangesAsync(cancellationToken);
        //Генирация результата

        var firstSentence = new SentenceDto()
        {
            Id = sentencePair.FirstSentence.Id,
            Language = sentencePair.FirstSentence.Language,
            Sentence = sentencePair.FirstSentence.Sentence,
        };
        var secondSentence = new SentenceDto()
        {
            Id = sentencePair.SecondSentence.Id,
            Language = sentencePair.SecondSentence.Language,
            Sentence = sentencePair.SecondSentence.Sentence,
        };

        var result = new AddSentencePairDto()
        {
            Id = sentencePair.Id, FirstSentence = firstSentence, SecondSentence = secondSentence
        };
        
        return result;
    }
}
