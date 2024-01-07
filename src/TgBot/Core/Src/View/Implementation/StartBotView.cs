using DropWord.TgBot.Core.Attribute;
using DropWord.TgBot.Core.Extension;
using DropWord.TgBot.Core.Field.Controller;
using DropWord.TgBot.Core.Field.View;
using DropWord.TgBot.Core.Model;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace DropWord.TgBot.Core.Src.View.Implementation;

public class StartBotView : ABotView
{
    private readonly ITelegramBotClient _botClient;

    public StartBotView(ITelegramBotClient botClient)
    {
        _botClient = botClient;
    }

    [BotView(StartViewField.Start)]
    public async Task StartAsync(UpdateBDto update)
    {
        var text =
            "Вітаю, я бот помічних. Буду допомагати тобі вивчати мову. Щоб продовжити обери мову із запропанованих нижче";
        var replyMarkup = new ReplyKeyboardMarkup(new[]
        {
            new KeyboardButton[]
            {
                StartControllerField.UkrainianEnglishLanguageButton,
                StartControllerField.UkrainianGermanLanguageButton
            },
            new KeyboardButton[]
            {
                StartControllerField.UkrainianPolishLanguageButton,
                StartControllerField.UkrainianFrenchLanguageButton
            }
        }) { ResizeKeyboard = true };
        await _botClient.SendTextMessageAsync(update.GetUserId(), text, replyMarkup: replyMarkup);
    }

    [BotView(StartViewField.SelectLanguage)]
    public async Task SelectLanguageAsync(UpdateBDto update)
    {
        var text =
            "Будьласка оберіть одну із запропанованих мов низжче.";
        var replyMarkup = new ReplyKeyboardMarkup(new[]
        {
            new KeyboardButton[]
            {
                StartControllerField.UkrainianEnglishLanguageButton,
                StartControllerField.UkrainianGermanLanguageButton
            },
            new KeyboardButton[]
            {
                StartControllerField.UkrainianPolishLanguageButton,
                StartControllerField.UkrainianFrenchLanguageButton
            }
        }) { ResizeKeyboard = true };
        await _botClient.SendTextMessageAsync(update.GetUserId(), text, replyMarkup: replyMarkup);
    }
}
