using DropWord.Application.Manager.Sentence;
using DropWord.Domain.Enums;

namespace DropWord.Application.UseCase.Sentence.Queries.GetSentenceForRepeat;

public record GetSentenceForRepeatQuery : IRequest<SentenceForRepeatDto>
{
    public long UserId { get; set; }
}

// public class GetSentenceForRepeatQueryValidator : AbstractValidator<GetSentenceForRepeatQuery>
// {
//     public GetSentenceForRepeatQueryValidator()
//     {
//     }
// }

public class GetSentenceForRepeatQueryHandler : IRequestHandler<GetSentenceForRepeatQuery, SentenceForRepeatDto>
{
    private readonly ISentenceManager _sentenceManager;


    public GetSentenceForRepeatQueryHandler(ISentenceManager sentenceManager)
    {
        _sentenceManager = sentenceManager;
    }

    public async Task<SentenceForRepeatDto> Handle(GetSentenceForRepeatQuery request,
        CancellationToken cancellationToken)
    {
        var result = await _sentenceManager.GetSentenceForRepeatAsync(request.UserId, SentenceForRepeatModeEnum.OldDataMinCount);
        return result;
    }
}
