using DropWord.Application.Common.Interfaces;
using DropWord.Domain.Enums;

namespace DropWord.Application.UseCase.User.Commands.UpdateUser;

public record UpdateUserCommand : IRequest
{
    public long UserId { get; set; }
    public string? Name { get; set; }
    public UserGenderEnum? Gender { get; set; }
}

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
    }
}

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateUserCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == request.UserId);
        if (user == null)
        {
            throw new NotFoundException(nameof(User), request.UserId.ToString());
        }

        user.Name = request.Name ?? user.Name;
        user.Gender = request.Gender ?? user.Gender;

        await _context.SaveChangesAsync(cancellationToken);
    }
}
