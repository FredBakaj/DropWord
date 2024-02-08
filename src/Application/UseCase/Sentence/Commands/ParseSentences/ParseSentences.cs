using DropWord.Application.Common.Interfaces;
using DropWord.Application.Common.Interfaces.Sentence;
using DropWord.Domain.Enums;

namespace DropWord.Application.UseCase.Sentence.Commands.ParseSentences;

public record ParseSentencesCommand : IRequest<ParseSentencesDto>
{
    public string Content { get; set; } = null!;
}

public class ParseSentencesCommandValidator : AbstractValidator<ParseSentencesCommand>
{
    public ParseSentencesCommandValidator()
    {
    }
}

public class ParseSentencesCommandHandler : IRequestHandler<ParseSentencesCommand, ParseSentencesDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IParse _parse;

    public ParseSentencesCommandHandler(IApplicationDbContext context, IParse parse)
    {
        _context = context;
        _parse = parse;
    }

    public async Task<ParseSentencesDto> Handle(ParseSentencesCommand request, CancellationToken cancellationToken)
    {
        // Получение из контента предложений
        var sentencesModel = await _parse.ParseAsync(request.Content);
        var sentences = sentencesModel.Select(x => x.Sentence);
        var result = new ParseSentencesDto();
        result.Sentences = sentences;
        if (sentences.Count() > 1)
        {
            result.ParseSentenceLabel = ParseSentenceLabelEnum.Text;
        }
        else
        {
            result.ParseSentenceLabel = ParseSentenceLabelEnum.Single;
        }

        return await Task.FromResult(result);
    }
}
