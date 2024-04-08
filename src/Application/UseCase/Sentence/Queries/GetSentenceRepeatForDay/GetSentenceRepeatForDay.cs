using DropWord.Application.Common.Interfaces;
using DropWord.Application.Manager.Sentence;
using DropWord.Domain.Enums;

namespace DropWord.Application.UseCase.Sentence.Queries.GetSentenceRepeatForDay;

public record GetSentenceRepeatForDayQuery : IRequest<SentenceRepeatForDayDto>
{
    public long UserId { get; set; }
}

// public class GetSentenceRepeatForDayQueryValidator : AbstractValidator<GetSentenceRepeatForDayQuery>
// {
//     public GetSentenceRepeatForDayQueryValidator()
//     {
//     }
// }

public class
    GetSentenceRepeatForDayQueryHandler : IRequestHandler<GetSentenceRepeatForDayQuery, SentenceRepeatForDayDto>
{
    private readonly IApplicationDbContext _context;
    private readonly ISentenceManager _sentenceManager;
    private readonly IMapper _mapper;

    public GetSentenceRepeatForDayQueryHandler(IApplicationDbContext context, ISentenceManager sentenceManager,
        IMapper mapper)
    {
        _context = context;
        _sentenceManager = sentenceManager;
        _mapper = mapper;
    }

    public async Task<SentenceRepeatForDayDto> Handle(GetSentenceRepeatForDayQuery request,
        CancellationToken cancellationToken)
    {
        var result =
            await _sentenceManager.GetSentenceForRepeatAsync(request.UserId, SentenceForRepeatModeEnum.OldDataMinCount);

        return _mapper.Map<SentenceRepeatForDayDto>(result);
    }
}
