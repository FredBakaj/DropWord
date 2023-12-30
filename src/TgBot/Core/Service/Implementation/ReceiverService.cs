using Telegram.Bot;

namespace DropWord.TgBot.Core.Service.Implementation;

// Compose Receiver and UpdateHandler implementation
public class ReceiverService : ReceiverServiceBase<IUpdateHandler>
{
    public ReceiverService(
        ITelegramBotClient botClient,
        IUpdateHandler updateHandler,
        ILogger<ReceiverServiceBase<IUpdateHandler>> logger)
        : base(botClient, updateHandler, logger)
    {
    }
}
