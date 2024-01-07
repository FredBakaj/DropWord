using DropWord.Application.UseCase.StateTree.Commands.SetAction;
using DropWord.Application.UseCase.StateTree.Commands.SetDataAndAction;
using DropWord.Application.UseCase.StateTree.Commands.SetStateAndAction;
using DropWord.Application.UseCase.StateTree.Queries.GetData;
using DropWord.Application.UseCase.StateTree.Queries.GetStateAndAction;
using DropWord.TgBot.Core.Extension;
using DropWord.TgBot.Core.Model;
using MediatR;

namespace DropWord.TgBot.Core.Handler.Implementation;

public class BotStateTreeUserHandler : IBotStateTreeUserHandler
{
    private readonly ISender _sender;

    public BotStateTreeUserHandler(ISender sender)
    {
        _sender = sender;
    }

    public async Task SetStateAndActionAsync(UpdateBDto update, string state, string action,
        CancellationToken cancellationToken = default)
    {
        var userId = update.GetUserId();
        await _sender.Send(new SetStateAndActionCommand() { UserId = userId, State = state, Action = action },
            cancellationToken);
    }

    public async Task SetDataAndActionAsync<T>(UpdateBDto update, string action, T data,
        CancellationToken cancellationToken = default)
    {
        var userId = update.GetUserId();
        await _sender.Send(new SetDataAndActionCommand() { UserId = userId, Action = action, Data = data! },
            cancellationToken);
    }

    public async Task SetActionAsync(UpdateBDto update, string action, CancellationToken cancellationToken = default)
    {
        var userId = update.GetUserId();
        await _sender.Send(new SetActionCommand() { UserId = userId, Action = action }, cancellationToken);
    }

    public async Task<T> GetDataAsync<T>(UpdateBDto update, CancellationToken cancellationToken = default)
        where T : class
    {
        var userId = update.GetUserId();
        object result = await _sender.Send(new GetDataQuery() { UserId = userId }, cancellationToken);
        return (result as T)!;
    }

    public async Task<StateTreeBDto> GetStateAndActionAsync(UpdateBDto update,
        CancellationToken cancellationToken = default)
    {
        var userId = update.GetUserId();
        StateAndActionDto response =
            await _sender.Send(new GetStateAndActionQuery() { UserId = userId }, cancellationToken);
        var result = new StateTreeBDto() { State = response.State, Action = response.Action };
        return result;
    }
}
