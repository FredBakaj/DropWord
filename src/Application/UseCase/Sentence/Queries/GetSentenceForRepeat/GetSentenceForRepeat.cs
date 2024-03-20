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
    private readonly IMapper _mapper;


    public GetSentenceForRepeatQueryHandler(ISentenceManager sentenceManager, IMapper mapper)
    {
        _sentenceManager = sentenceManager;
        _mapper = mapper;
    }

    public async Task<SentenceForRepeatDto> Handle(GetSentenceForRepeatQuery request,
        CancellationToken cancellationToken)
    {
        var result = await _sentenceManager.GetSentenceForRepeatAsync(request.UserId, SentenceForRepeatModeEnum.StepByQueue);
        return _mapper.Map<SentenceForRepeatDto>(result);
    }
}
