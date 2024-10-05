using DropWord.TgBot.Core.Extension;
using DropWord.TgBot.Core.Field.Controller;
using DropWord.TgBot.Core.Model;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
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
        var replyMarkup = CreateKeyboardButton();

        await _botClient.SendTextMessageMarkdown2Async(update.GetUserId(), text, replyMarkup: replyMarkup);
    }
    public async Task SendHTMLAsync(UpdateBDto update, string text)
    {
        ReplyKeyboardMarkup replyMarkup = CreateKeyboardButton();

        await _botClient.SendTextMessageAsync(update.GetUserId(), text, replyMarkup: replyMarkup, parseMode:ParseMode.Html, disableWebPagePreview:true);
    }

    private ReplyKeyboardMarkup CreateKeyboardButton()
    {
        return new ReplyKeyboardMarkup(new[]
        {
            new KeyboardButton[]
            {
                BaseField.RepeatSentenceKeyboard, BaseField.NewSentenceButton
            },
            new KeyboardButton[]
            {
                BaseField.SentencesRepetitionByInputKeyboard, BaseField.RecommendedNewSentenceButton
            },
            new KeyboardButton[]
            {
                BaseField.ChatKeyboard, BaseField.SettingsKeyboard
            }
        }) { ResizeKeyboard = true };
    }
    
}
