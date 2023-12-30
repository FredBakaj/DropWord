using DropWord.TgBot.Core.Src.Command;

namespace DropWord.TgBot.Core.Factory.Implementation;

public class CommandFactory : IFactory<IBotCommand>
{
    private readonly IEnumerable<IBotCommand> _botCommands;

    public CommandFactory(IEnumerable<IBotCommand> botCommands)
    {
        _botCommands = botCommands;
    }

    public Task<IBotCommand> CreateAsync(object value)
    {
        string? value_ = value as string;
        //получает все что до первого пробела, что являеться коммандой
        if (value_ != null)
        {
            value_ = String.Join(" ", value_.Split(" ")[..1]);
            return Task.FromResult(_botCommands.First(c => c.GetCommand() == value_));
        }

        throw new InvalidOperationException("Can`t convert value to string");
    }
}
