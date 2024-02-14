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
    public async Task SettingsMenu(SettingsMenuKeyboardVDto viewBDto)
    {
        var settingsItem = SettingsMenuItem(viewBDto.ChangeModeIcon);
        await _botClient.SendTextMessageAsync(viewBDto.Update.GetUserId(), settingsItem.Item1,
            replyMarkup: settingsItem.Item2);
    }

    [BotView(SettingsViewField.ChangeLearnSentencesModeCallback)]
    public async Task ChangeLearnSentencesModeCallback(ChangeLearnSentencesModeCallbackVDto viewDto)
    {
        var settingsItem = SettingsMenuItem(viewDto.ChangeModeIcon);
        await _botClient.EditMessageTextAsync(viewDto.Update.GetUserId(),
            viewDto.Update.GetMessage().MessageId,
            settingsItem.Item1,
            replyMarkup: settingsItem.Item2);
    }

    private (string, InlineKeyboardMarkup) SettingsMenuItem(string changeEmojiButton)
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
            }
        });
        return (text, inlineKeyboard);
    }
}
