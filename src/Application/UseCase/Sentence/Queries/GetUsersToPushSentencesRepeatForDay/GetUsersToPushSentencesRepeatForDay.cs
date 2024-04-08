using DropWord.Application.Common.Interfaces;
using DropWord.Domain.Enums;

namespace DropWord.Application.UseCase.Sentence.Queries.GetUsersToPushSentencesRepeatForDay;

public record GetUsersToPushSentencesRepeatForDayQuery : IRequest<UsersToPushSentencesRepeatForDayDto>
{
    public int TimeZone { get; set; }
    public List<SentencesRepeatForDayTimesModeEnum> SentencesForDayMode { get; set; } = null!;
}

public class
    GetUsersToPushSentencesRepeatForDayQueryValidator : AbstractValidator<GetUsersToPushSentencesRepeatForDayQuery>
{
    public GetUsersToPushSentencesRepeatForDayQueryValidator()
    {
    }
}

public class
    GetUsersToPushSentencesRepeatForDayQueryHandler : IRequestHandler<GetUsersToPushSentencesRepeatForDayQuery,
        UsersToPushSentencesRepeatForDayDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetUsersToPushSentencesRepeatForDayQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<UsersToPushSentencesRepeatForDayDto> Handle(GetUsersToPushSentencesRepeatForDayQuery request,
        CancellationToken cancellationToken)
    {
        var users = await _context.Users
            .Include(x => x.UserSettings)
            .Include(x => x.UserLearningInfo)
            .Where(x => request.SentencesForDayMode.Contains(x.UserSettings.SentencesRepeatForDayTimesModeEnum)
                        && x.UserSettings.TimeZone == request.TimeZone)
            .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
            .ToListAsync();

        return new UsersToPushSentencesRepeatForDayDto() { Users = users };
    }
}
