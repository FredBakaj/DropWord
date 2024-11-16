using DropWord.Application.Common.Interfaces;

namespace DropWord.Application.UseCase.User.Queries.GetCountUsers;

public record GetCountUsersQuery : IRequest<CountUsersDto>
{
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
        return new CountUsersDto
        { 
            Count = await _context.Users.CountAsync(cancellationToken)
        };
    }
}
