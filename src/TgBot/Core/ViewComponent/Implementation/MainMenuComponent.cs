using DropWord.TgBot.Core.Extension;
using DropWord.TgBot.Core.Field.Controller;
using DropWord.TgBot.Core.Model;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace DropWord.TgBot.Core.ViewComponent.Implementation;

public class MainMenuComponent : IMainMenuComponent
{
    private readonly ITelegramBotClient _botClient;

    public MainMenuComponent(ITelegramBotClient botClient)
    {
        _botClient = botClient;
    }
    public async Task SendAsync(UpdateBDto update, string text)
    {
        var replyMarkup = new ReplyKeyboardMarkup(new[]
        {
            new KeyboardButton[]
            {
                BaseField.RepeatSentenceKeyboard, BaseField.NewSentenceButton
            },
            new KeyboardButton[] { BaseField.SentencesRepetitionByInputKeyboard,BaseField.SettingsKeyboard }
        }) { ResizeKeyboard = true };

        await _botClient.SendTextMessageMarkdown2Async(update.GetUserId(), text, replyMarkup: replyMarkup);
    }
}
