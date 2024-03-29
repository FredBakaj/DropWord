﻿using DropWord.TgBot.Core.Extension;
using DropWord.TgBot.Core.Field;
using DropWord.TgBot.Core.Field.Controller;
using DropWord.TgBot.Core.Field.View;
using DropWord.TgBot.Core.Handler;
using DropWord.TgBot.Core.Model;
using Microsoft.IdentityModel.Abstractions;
using Telegram.Bot;

namespace DropWord.TgBot.Core.Src.Command.Implementation;

public class ReloadCommand : IBotCommand
{
    private readonly ITelegramBotClient _client;
    private readonly IBotStateTreeUserHandler _botStateTreeUserHandler;
    public string GetCommand() => CommandField.Reload;

    public bool IsMoveNext() => true;

    public ReloadCommand(ITelegramBotClient client,
        IBotStateTreeUserHandler botStateTreeUserHandler)
    {
        _client = client;
        _botStateTreeUserHandler = botStateTreeUserHandler;
    }

    public async Task Exec(UpdateBDto update)
    {
        await _botStateTreeUserHandler.SetStateAndActionAsync(update, BaseField.BaseState, BaseField.ReloadAction,
            CancellationToken.None);
        await _client.SendTextMessageAsync(update.GetUserId(), "бот перезавантажен)");
    }
}
