using DropWord.Application.Common.Interfaces;

namespace DropWord.Application.UseCase.User.Queries.GetCountUsers;

public record GetCountUsersQuery : IRequest<CountUsersDto>
{
    public int ReturnUserDays {get; set; }
}

public class GetCountUsersQueryValidator : AbstractValidator<GetCountUsersQuery>
{
    public GetCountUsersQueryValidator()
    {
    }
}

public class GetCountUsersQueryHandler : IRequestHandler<GetCountUsersQuery, CountUsersDto>
{
    private readonly IApplicationDbContext _context;

    public GetCountUsersQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CountUsersDto> Handle(GetCountUsersQuery request, CancellationToken cancellationToken)
    {
        var countUsers = await _context.Users.CountAsync(cancellationToken);
        var listReturnUsers = await _context.AnalyticsUserAction
            .Where(action => action.Created >= DateTime.Now.AddDays(-request.ReturnUserDays)) // Фильтр по дате
            .GroupBy(action => action.UserId) // Группировка по UserId
            .Select(group => new 
            {
                UserId = group.Key,
                DaysCount = group
                    .Select(action => action.Created.Date) // Преобразуем Created в дату (без времени)
                    .Distinct()
                    .Count()
            })
            .Where(x => x.DaysCount >= 2) // Фильтр по количеству дней
            .ToListAsync();
        
        var result = new CountUsersDto()
        {
            CountUsers = countUsers,
            CountReturnUsers = listReturnUsers.Count()
        };
        return result;
    }
}
