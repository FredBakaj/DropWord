using DropWord.Application.Common.Interfaces;
using DropWord.Application.Common.Interfaces.Sentence;
using DropWord.Domain.Entities;
using DropWord.Domain.Exceptions;

namespace DropWord.Application.UseCase.SentencesCollection.Commands.AddCollection;

public record AddCollectionCommand : IRequest<SentencesCollectionDto>
{
    public long UserId { get; set; }
    public IEnumerable<string> Sentences { get; set; } = null!;
    public string Description { get; set; } = null!;
}

public class AddCollectionCommandValidator : AbstractValidator<AddCollectionCommand>
{
    public AddCollectionCommandValidator()
    {
    }
}

public class AddCollectionCommandHandler : IRequestHandler<AddCollectionCommand, SentencesCollectionDto>
{
    private readonly IApplicationDbContext _context;
    private readonly ITranslate _translate;
    private readonly IMapper _mapper;

    public AddCollectionCommandHandler(IApplicationDbContext context, ITranslate translate, IMapper mapper)
    {
        _context = context;
        _translate = translate;
        _mapper = mapper;
    }

    public async Task<SentencesCollectionDto> Handle(AddCollectionCommand request, CancellationToken cancellationToken)
    {
        var detectLanguage = await _translate.DetectLanguageListAsync(request.Sentences);
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

        var translateSentences =
            await _translate.TranslateListAsync(request.Sentences, firstSentenceLanguage, translateTo);

        //Сохранение предложений в БД
        var sentencesPairs = new List<SentencesPairEntity>();
        foreach (var item in translateSentences)
        {
            var originalSentence =
                new SentenceEntity() { Language = item.OriginalLanguage, Sentence = item.OriginalSentence };
            var translateSentence =
                new SentenceEntity() { Language = item.TranslateLanguage, Sentence = item.TranslateSentence };

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
            sentencesPairs.Add(sentencePair);
        }

        var newUserSentencesCollection =
            _context.UserSentencesCollection.Add(new UserSentencesCollectionEntity()
            {
                UserId = request.UserId,
                FirstLanguage = userSettings.MainLanguage,
                SecondLanguage = userSettings.LearnLanguage,
                Description = request.Description,
                Created = DateTimeOffset.UtcNow,
                SentencesPairs = sentencesPairs
            });

        await _context.SaveChangesAsync(cancellationToken);

        var result = new SentencesCollectionDto()
        {
            CollectionId = newUserSentencesCollection.Entity.Id,
            SentencesPairs = _mapper.Map<List<SentencesPairDto>>(sentencesPairs)
        };

        return result;
    }
}
