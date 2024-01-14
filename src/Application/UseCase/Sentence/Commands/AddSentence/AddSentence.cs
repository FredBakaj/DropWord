using DropWord.Application.Common.Interfaces;
using DropWord.Application.Common.Interfaces.Sentence;
using DropWord.Domain.Entities;
using DropWord.Domain.Exceptions;

namespace DropWord.Application.UseCase.Sentence.Commands.AddSentence;

public record AddSentenceCommand : IRequest<IEnumerable<AddSentenceDto>>
{
    public long UserId { get; set; }
    public string Content { get; set; } = null!;
}

public class AddSentenceCommandValidator : AbstractValidator<AddSentenceCommand>
{
    public AddSentenceCommandValidator()
    {
    }
}

public class AddSentenceCommandHandler : IRequestHandler<AddSentenceCommand, IEnumerable<AddSentenceDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IParse _parse;
    private readonly ITranslate _translate;

    public AddSentenceCommandHandler(IApplicationDbContext context, IParse parse, ITranslate translate)
    {
        _context = context;
        _parse = parse;
        _translate = translate;
    }

    public async Task<IEnumerable<AddSentenceDto>> Handle(AddSentenceCommand request,
        CancellationToken cancellationToken)
    {
        // Получение из контента предложений
        var sentencesModel = await _parse.ParseAsync(request.Content);
        var sentences = sentencesModel.Select(x => x.Sentence);
        // Определение языков предложений
        var detectLanguage = await _translate.DetectLanguageListAsync(sentences);
        // Проверка на наличие больше одного языка в контенте
        var firstSentenceLanguage = detectLanguage.First().Language;
        if (!detectLanguage.All(x => x.Language == firstSentenceLanguage))
        {
            throw new DetectMoreThanOneLanguageException("more than one language is detected in the sentences");
        }
        //Перевод предложений
        var userSettings = await _context.UserSettings.FirstAsync(x => x.UserId == request.UserId);
        var translateTo = firstSentenceLanguage == userSettings.MainLanguage
            ? userSettings.LearnLanguage
            : userSettings.MainLanguage;

        var translateSentences = await _translate.TranslateListAsync(sentences, firstSentenceLanguage, translateTo);
        //Сохранение предложений в БД
        var sentencesPair = new List<SentencesPairEntity>();
        foreach (var item in translateSentences)
        {
            var originalSentence =
                new SentenceEntity()
                {
                    Language = item.OriginalLanguage, 
                    Sentence = item.OriginalSentence
                };
            var translateSentence =
                new SentenceEntity()
                {
                    Language = item.TranslateLanguage, 
                    Sentence = item.TranslateSentence
                };

            SentencesPairEntity sentencePair = null!;
            if (item.OriginalLanguage == userSettings.MainLanguage)
            {
                sentencePair = new SentencesPairEntity()
                {
                    FirstSentence = originalSentence,
                    SecondSentence = translateSentence,
                    FirstLanguage = item.OriginalLanguage,
                    SecondLanguage = item.TranslateLanguage,
                    UserId = request.UserId
                };
            }
            else if (item.TranslateLanguage == userSettings.MainLanguage)
            {
                sentencePair = new SentencesPairEntity()
                {
                    FirstSentence = translateSentence,
                    SecondSentence = originalSentence,
                    FirstLanguage = item.TranslateLanguage,
                    SecondLanguage = item.OriginalLanguage,
                    UserId = request.UserId
                };
            }
            _context.SentencesPair.Add(sentencePair);
            sentencesPair.Add(sentencePair);
        }

        await _context.SaveChangesAsync(cancellationToken);
        //Генирация результата
        var result = new List<AddSentenceDto>();
        foreach (var item in sentencesPair)
        {
            result.Add(new AddSentenceDto()
            {
                Id = item.Id,
                FirstSentence = item.FirstSentence.Sentence,
                SecondSentence = item.SecondSentence.Sentence
            });
        }

        return result;
    }
}
