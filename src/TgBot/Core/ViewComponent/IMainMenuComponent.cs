﻿using DropWord.TgBot.Core.Model;

namespace DropWord.TgBot.Core.ViewComponent;

public interface IMainMenuComponent
{
    public Task SendAsync(UpdateBDto update, string text);
}
