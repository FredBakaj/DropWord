using DropWord.Application.Factory.Sentence;
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
    private readonly ISentencesFactory _sentencesFactory;
    private readonly IMapper _mapper;


    public GetSentenceForRepeatQueryHandler(ISentencesFactory sentencesFactory, IMapper mapper)
    {
        
        _sentencesFactory = sentencesFactory;
        _mapper = mapper;
    }

    public async Task<SentenceForRepeatDto> Handle(GetSentenceForRepeatQuery request,
        CancellationToken cancellationToken)
    {
        var sentencesForRepeat = await _sentencesFactory.CreateSentencesForRepeatAsync(SentenceForRepeatModeEnum.StepByQueue);
        var result = await sentencesForRepeat.Exec(request.UserId);
        return _mapper.Map<SentenceForRepeatDto>(result);
    }
}
