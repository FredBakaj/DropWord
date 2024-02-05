using DropWord.Application.Common.Interfaces.Sentence;
using DropWord.Application.UseCase.Sentence.Queries.DiffSentenceWithMarkup;

namespace DropWord.Application.UseCase.Sentence.Queries.GetDiffSentenceWithMarkup;

public record GetDiffSentenceWithMarkupQuery : IRequest<DiffSentenceWithMarkupDto>
{
    public string OldSentence { get; set; } = null!;
    public string NewSentence { get; set; } = null!;
}

public class GetDiffSentenceWithMarkupQueryValidator : AbstractValidator<GetDiffSentenceWithMarkupQuery>
{
    public GetDiffSentenceWithMarkupQueryValidator()
    {
    }
}

public class GetDiffSentenceWithMarkupQueryHandler : IRequestHandler<GetDiffSentenceWithMarkupQuery, DiffSentenceWithMarkupDto>
{
    private readonly IDifferenceSentence _differenceSentence;

    public GetDiffSentenceWithMarkupQueryHandler(IDifferenceSentence differenceSentence)
    {
        _differenceSentence = differenceSentence;
    }

    public async Task<DiffSentenceWithMarkupDto> Handle(GetDiffSentenceWithMarkupQuery request,
        CancellationToken cancellationToken)
    {
        var result = new DiffSentenceWithMarkupDto();
        result.Sentence = _differenceSentence.DiffSentenceWithMarkup(request.OldSentence, request.NewSentence);
        return await Task.FromResult(result);
    }
}
