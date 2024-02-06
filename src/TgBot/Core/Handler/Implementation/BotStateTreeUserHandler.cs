using System.Text.Json;
using DropWord.Application.UseCase.StateTree.Commands.ClearData;
using DropWord.Application.UseCase.StateTree.Commands.ClearTempData;
using DropWord.Application.UseCase.StateTree.Commands.SetAction;
using DropWord.Application.UseCase.StateTree.Commands.SetDataAndAction;
using DropWord.Application.UseCase.StateTree.Commands.SetStateAndAction;
using DropWord.Application.UseCase.StateTree.Commands.SetTempData;
using DropWord.Application.UseCase.StateTree.Queries.GetData;
using DropWord.Application.UseCase.StateTree.Queries.GetStateAndAction;
using DropWord.Application.UseCase.StateTree.Queries.GetTempData;
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
        CancellationToken cancellationToken = default) where T : class
    {
        var stateDataModel = new StateDataBDto<T>() { DataType = data.GetType().Name, Data = data };
        var userId = update.GetUserId();
        await _sender.Send(new SetDataAndActionCommand() { UserId = userId, Action = action, Data = stateDataModel },
            cancellationToken);
    }

    public async Task SetActionAsync(UpdateBDto update, string action, CancellationToken cancellationToken = default)
    {
        var userId = update.GetUserId();
        await _sender.Send(new SetActionCommand() { UserId = userId, Action = action }, cancellationToken);
    }

    public async Task<T?> GetDataAsync<T>(UpdateBDto update, CancellationToken cancellationToken = default)
        where T : class
    {
        var userId = update.GetUserId();
        string userData = await _sender.Send(new GetDataQuery() { UserId = userId }, cancellationToken);
        if (userData != null && userData != string.Empty)
        {
            var dataStateModel = JsonSerializer.Deserialize<StateDataBDto<T>>(userData);
            if (dataStateModel!.DataType == typeof(T).Name)
            {
                return dataStateModel!.Data;
            }
        }

        return null;
    }

    public async Task ClearDataAsync(UpdateBDto update, CancellationToken cancellationToken = default)
    {
        var userId = update.GetUserId();
        await _sender.Send(new ClearDataCommand() { UserId = userId }, cancellationToken);
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

    public async Task SetTempDataAsync<T>(UpdateBDto update, T tempData, CancellationToken cancellationToken = default)
        where T : class
    {
        var stateDataModel = new StateTempDataBDto<T>() { DataType = tempData.GetType().Name, Data = tempData };
        var userId = update.GetUserId();
        await _sender.Send(new SetTempDataCommand() { UserId = userId, Data = stateDataModel },
            cancellationToken);
    }

    public async Task<T?> GetTempDataAsync<T>(UpdateBDto update, CancellationToken cancellationToken = default)
        where T : class
    {
        var userId = update.GetUserId();
        string userData = await _sender.Send(new GetTempDataQuery() { UserId = userId }, cancellationToken);
        if (userData != null && userData != string.Empty)
        {
            var dataStateModel = JsonSerializer.Deserialize<StateTempDataBDto<T>>(userData);
            if (dataStateModel!.DataType == typeof(T).Name)
            {
                return dataStateModel!.Data;
            }
        }

        return null;
    }

    public async Task ClearTempDataAsync(UpdateBDto update, CancellationToken cancellationToken = default)
    {
        var userId = update.GetUserId();
        await _sender.Send(new ClearTempDataCommand() { UserId = userId }, cancellationToken);
    }
}
