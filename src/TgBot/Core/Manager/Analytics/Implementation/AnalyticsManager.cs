using DropWord.Application.UseCase.Analytics.Commands.SendUserAction;
using MediatR;

namespace DropWord.TgBot.Core.Manager.Analytics.Implementation;

public class AnalyticsManager : IAnalyticsManager
{
    private readonly ISender _sender;

    public AnalyticsManager(ISender sender)
    {
        _sender = sender;
    }

    public async Task SendUserActionAsync(long userId, string typeAction, string action, object? data)
    {
        await _sender.Send(new SendUserActionCommand()
        {
            UserId = userId, TypeAction = typeAction, Action = action, Data = data?.ToString()
        });
    }
}
