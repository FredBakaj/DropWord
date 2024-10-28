using DropWord.Application.Common.Interfaces;
using DropWord.Domain.Enums;

namespace DropWord.Application.UseCase.SmallTalkChat.Queries.GetUserCountMessage;

public record GetUserCountMessageQuery : IRequest<UserCountMessageDto>
{
    public long UserId { get; set; }
}

public class GetUserCountMessageQueryValidator : AbstractValidator<GetUserCountMessageQuery>
{
    public GetUserCountMessageQueryValidator()
    {
    }
}

public class GetUserCountMessageQueryHandler : IRequestHandler<GetUserCountMessageQuery, UserCountMessageDto>
{
    private readonly IApplicationDbContext _context;

    public GetUserCountMessageQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<UserCountMessageDto> Handle(GetUserCountMessageQuery request, CancellationToken cancellationToken)
    {
        var countMessage = await _context.AutoChatData
            .Include(x => x.AutoChatHistories)
            .SelectMany(x => x.AutoChatHistories.Select(y => new
            {
                UserId = x.UserId,
                HistoryRecordId = y.Id,
                SenderEnum = y.SenderEnum,
                RecordCreated = y.Created
            }))
            .Where(x => x.UserId == request.UserId
            && x.SenderEnum == AutoChatSenderEnum.User
            && x.RecordCreated >= DateTime.Now.AddHours(-24))
            .CountAsync();
            
        
        return new UserCountMessageDto() { CountMessage = countMessage };
    }
}
