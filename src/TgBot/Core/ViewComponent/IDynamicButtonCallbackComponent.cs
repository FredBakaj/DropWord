using Telegram.Bot.Types.ReplyMarkups;

namespace DropWord.TgBot.Core.ViewComponent;

public interface IDynamicButtonCallbackComponent
{
    public List<InlineKeyboardButton[]> CreateCollection(int countInRow, Dictionary<string, string> buttonsMeta,
        string callbackData);
}
