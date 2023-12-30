using DropWord.TgBot.Core.Factory;
using DropWord.TgBot.Core.Model;
using DropWord.TgBot.Core.Src.Command;

namespace DropWord.TgBot.Core.Src.Middleware.Implementation;

/// <summary>
/// Обработака команд пользователя
/// </summary>
public class CommandMiddleware : ABotMiddleware
{
    private readonly IFactory<IBotCommand> _commandFactory;
    private readonly ILogger<CommandMiddleware> _logger;


    public CommandMiddleware(IFactory<IBotCommand> commandFactory, ILogger<CommandMiddleware> logger)
    {
        _commandFactory = commandFactory;
        _logger = logger;
    }

    public override async Task Next(UpdateBDto update)
    {
        try
        {
            bool isMoveNext = true;
            if (update.Message != null
                && update.Message.Text != null
                && update.Message.Text.StartsWith("/"))
            {
                var commandText = update.Message.Text.Substring(1);
                var command = await _commandFactory.CreateAsync(commandText);
                isMoveNext = command.IsMoveNext();
                await command.Exec(update);
            }

            if (isMoveNext)
            {
                await base.Next(update);
            }
        }
        catch (Exception e)
        {
            _logger.LogInformation(e, e.Message);
        }
    }
}
