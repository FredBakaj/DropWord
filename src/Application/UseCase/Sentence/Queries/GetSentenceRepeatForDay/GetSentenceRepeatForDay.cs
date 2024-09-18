using DropWord.Application.Common.Interfaces;
using DropWord.Application.Factory.Sentence;
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

public class GetSentenceRepeatForDayQueryHandler : IRequestHandler<GetSentenceRepeatForDayQuery, SentenceRepeatForDayDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ISentencesFactory _sentencesFactory;

    public GetSentenceRepeatForDayQueryHandler(IApplicationDbContext context,
        IMapper mapper, ISentencesFactory sentencesFactory)
    {
        _context = context;
        _mapper = mapper;
        _sentencesFactory = sentencesFactory;
    }

    public async Task<SentenceRepeatForDayDto> Handle(GetSentenceRepeatForDayQuery request,
        CancellationToken cancellationToken)
    {
        var sentencesForRepeat = await _sentencesFactory.CreateSentencesForRepeatAsync(SentenceForRepeatModeEnum.OldDataMinCount);
        var result = await sentencesForRepeat.Exec(request.UserId);
        return _mapper.Map<SentenceRepeatForDayDto>(result);
    }
}
