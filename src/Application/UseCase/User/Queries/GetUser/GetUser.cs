using DropWord.Application.Common.Interfaces;
using DropWord.Domain.Exceptions;

namespace DropWord.Application.UseCase.User.Queries.GetUser;

public record GetUserQuery : IRequest<UserDto>
{
    public long UserId { get; set; }
}

public class GetUserQueryValidator : AbstractValidator<GetUserQuery>
{
    public GetUserQueryValidator()
    {
    }
}

public class GetUserQueryHandler : IRequestHandler<GetUserQuery, UserDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetUserQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<UserDto> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.Include(x => x.UserSettings)
            .Where(x => x.Id == request.UserId)
            .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();

        if (user == null)
        {
            throw new NotFoundUserException($"user with id {request.UserId} was not found");
        }

        return user;
    }
}
