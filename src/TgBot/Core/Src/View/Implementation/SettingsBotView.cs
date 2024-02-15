using DropWord.TgBot.Core.Attribute;
using DropWord.TgBot.Core.Extension;
using DropWord.TgBot.Core.Field.Controller;
using DropWord.TgBot.Core.Field.View;
using DropWord.TgBot.Core.ViewDto;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace DropWord.TgBot.Core.Src.View.Implementation;

public class SettingsBotView : ABotView
{
    private readonly ITelegramBotClient _botClient;

    public SettingsBotView(ITelegramBotClient botClient)
    {
        _botClient = botClient;
    }

    [BotView(SettingsViewField.SettingsMenu)]
    public async Task SettingsMenu(SettingsMenuVDto viewDto)
    {
        var settingsItem = SettingsMenuItem(viewDto.ChangeModeIcon, viewDto.LearnLanguagePairEmoji);
        await _botClient.SendTextMessageAsync(viewDto.Update.GetUserId(), settingsItem.Item1,
            replyMarkup: settingsItem.Item2);
    }

    [BotView(SettingsViewField.EditSettingsMenu)]
    public async Task ChangeLearnSentencesModeCallback(SettingsMenuVDto viewDto)
    {
        var settingsItem = SettingsMenuItem(viewDto.ChangeModeIcon, viewDto.LearnLanguagePairEmoji);
        await _botClient.EditMessageTextAsync(viewDto.Update.GetUserId(),
            viewDto.Update.GetMessage().MessageId,
            settingsItem.Item1,
            replyMarkup: settingsItem.Item2);
    }

    [BotView(SettingsViewField.OpenChangeLearnLanguagePairCallback)]
    public async Task ChangeLearnLanguagePairCallback(ChangeLearnLanguagePairCallbackVDto viewDto)
    {
        var text = "змінити мови вивчення 🔤\n" +
                   $"➡️ {viewDto.MainLanguage} {viewDto.LearnLanguage}";

        var constCountLang = 3;
        var learnLanguageVariants = viewDto.LearnLanguageVariants;
        //алогоритм преоброзования линейного словоря в матрицу кнопок калбека
        List<InlineKeyboardButton[]> buttons = new List<InlineKeyboardButton[]>(); 
        for (int i = 0; i < (int) (learnLanguageVariants.Keys.Count / constCountLang); i++)
        {
            var languagePairItems = learnLanguageVariants.Keys.Skip(i * constCountLang).Take(constCountLang).ToList();
            List<InlineKeyboardButton> lineButtons = new List<InlineKeyboardButton>();
            foreach (string item in languagePairItems)
            {
                lineButtons.Add(InlineKeyboardButton.WithCallbackData(text: learnLanguageVariants[item],
                    callbackData: BaseField.ChangeLearnLanguagePairCallback + ":" + $"{viewDto.MainLanguage}|{item}"));
            }
            buttons.Add(lineButtons.ToArray());
        }
        buttons.Add(new []{InlineKeyboardButton.WithCallbackData(text: "Повернутися",
            callbackData: BaseField.BackToSettingsMenuCallback)});

        await _botClient.EditMessageTextAsync(viewDto.Update.GetUserId(), viewDto.Update.GetMessage().MessageId,
            text, replyMarkup: new InlineKeyboardMarkup(buttons.ToArray()));

    }
    
    private (string, InlineKeyboardMarkup) SettingsMenuItem(string changeEmojiButton, string learnLanguagePairEmoji)
    {
        var text = "Налаштування ⚙️";
        //TODO добавить инструкцию к кнопкам
        var ChangeLanguageEmojiButton = changeEmojiButton;
        InlineKeyboardMarkup inlineKeyboard = new(new[]
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData(text: ChangeLanguageEmojiButton,
                    callbackData: BaseField.ChangeLearnSentencesModeCallback),
                InlineKeyboardButton.WithCallbackData(text: $"Змінити {learnLanguagePairEmoji}",
                    callbackData: BaseField.OpenChangeLearnLanguagePairCallback),
            }
        });
        return (text, inlineKeyboard);
    }
}
