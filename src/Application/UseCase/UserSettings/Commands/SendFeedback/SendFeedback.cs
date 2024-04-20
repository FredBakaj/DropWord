using DropWord.Application.Common.Interfaces;
using DropWord.Domain.Entities;

namespace DropWord.Application.UseCase.UserSettings.Commands.SendFeedback;

public record SendFeedbackCommand : IRequest<object>
{
    public long UserId { get; set; }
    public string Text { get; set; } = null!;
}

public class SendFeedbackCommandValidator : AbstractValidator<SendFeedbackCommand>
{
    public SendFeedbackCommandValidator()
    {
    }
}

public class SendFeedbackCommandHandler : IRequestHandler<SendFeedbackCommand, object>
{
    private readonly IApplicationDbContext _context;

    public SendFeedbackCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<object> Handle(SendFeedbackCommand request, CancellationToken cancellationToken)
    {
        var feedback = new FeedbackEntity()
        {
            UserId = request.UserId,
            Text = request.Text
        };

        _context.Feedback.Add(feedback);
        await _context.SaveChangesAsync(cancellationToken);

        return feedback;
    }
}
