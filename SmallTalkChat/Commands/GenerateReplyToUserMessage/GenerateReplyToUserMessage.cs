using CleanArchitecture.Application.Common.Interfaces;

namespace CleanArchitecture.Application.SmallTalkChat.Commands.GenerateReplyToUserMessage;

public record GenerateReplyToUserMessageCommand : IRequest<ReplyToUserMessageDto>
{
}

public class GenerateReplyToUserMessageCommandValidator : AbstractValidator<GenerateReplyToUserMessageCommand>
{
    public GenerateReplyToUserMessageCommandValidator()
    {
    }
}

public class GenerateReplyToUserMessageCommandHandler : IRequestHandler<GenerateReplyToUserMessageCommand, ReplyToUserMessageDto>
{
    private readonly IApplicationDbContext _context;

    public GenerateReplyToUserMessageCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ReplyToUserMessageDto> Handle(GenerateReplyToUserMessageCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
