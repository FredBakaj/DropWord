using DropWord.TgBot.Core.Field;
using DropWord.TgBot.Core.Model;

namespace DropWord.TgBot.Core.Src.Command.Implementation;

public class StartCommand : IBotCommand
{
    public string GetCommand() => CommandField.Start;
    public bool IsMoveNext() => true;

    

    public Task Exec(UpdateBDto telegramUpdate)
    {
        return Task.CompletedTask;
    }
}