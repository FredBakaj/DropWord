using Telegram.Bot.Types.ReplyMarkups;

namespace DropWord.TgBot.Core.ViewComponent.Implementation;

public class DynamicButtonCallbackComponent : IDynamicButtonCallbackComponent
{
    public List<InlineKeyboardButton[]> CreateCollection(int countInRow, Dictionary<string, string> buttonsMeta,
        string callbackData)
    {
        List<InlineKeyboardButton[]> buttons = new List<InlineKeyboardButton[]>();
        for (int i = 0; i < Math.Ceiling((double)buttonsMeta.Keys.Count / countInRow); i++)
        {
            var buttonMetaKeys = buttonsMeta.Keys.Skip(i * countInRow).Take(countInRow).ToList();
            List<InlineKeyboardButton> lineButtons = new List<InlineKeyboardButton>();
            foreach (string key in buttonMetaKeys)
            {
                lineButtons.Add(InlineKeyboardButton.WithCallbackData(text: buttonsMeta[key],
                    callbackData: callbackData + ":" + $"{key}"));
            }

            buttons.Add(lineButtons.ToArray());
        }

        return buttons;
    }
}
