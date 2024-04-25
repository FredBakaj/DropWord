using DropWord.TgBot.Core.Attribute;
using DropWord.TgBot.Core.Extension;
using DropWord.TgBot.Core.Field.Controller;
using DropWord.TgBot.Core.Field.View;
using DropWord.TgBot.Core.Model;
using DropWord.TgBot.Core.ViewComponent;
using DropWord.TgBot.Core.ViewDto;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace DropWord.TgBot.Core.Src.View.Implementation;

public class SettingsBotView : ABotView
{
    private readonly ITelegramBotClient _botClient;
    private readonly IDynamicButtonCallbackComponent _dynamicButtonCallbackComponent;
    private readonly IMainMenuComponent _mainMenuComponent;

    public SettingsBotView(ITelegramBotClient botClient, 
        IDynamicButtonCallbackComponent dynamicButtonCallbackComponent,
        IMainMenuComponent mainMenuComponent)
    {
        _botClient = botClient;
        _dynamicButtonCallbackComponent = dynamicButtonCallbackComponent;
        _mainMenuComponent = mainMenuComponent;
    }

    [BotView(SettingsViewField.SettingsMenu)]
    public async Task SettingsMenu(SettingsMenuVDto viewDto)
    {
        var settingsItem = SettingsMenuItem(viewDto.ChangeModeIcon, viewDto.LearnLanguagePairEmoji,
            viewDto.TimeZone, viewDto.TimesForDay);
        await _botClient.SendTextMessageAsync(viewDto.Update.GetUserId(), settingsItem.Item1,
            replyMarkup: settingsItem.Item2);
    }

    [BotView(SettingsViewField.EditSettingsMenu)]
    public async Task ChangeLearnSentencesModeCallback(SettingsMenuVDto viewDto)
    {
        var settingsItem = SettingsMenuItem(viewDto.ChangeModeIcon, viewDto.LearnLanguagePairEmoji,
            viewDto.TimeZone, viewDto.TimesForDay);
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
        var buttons = _dynamicButtonCallbackComponent.CreateCollection(constCountLang, learnLanguageVariants,
            BaseField.ChangeLearnLanguagePairCallback);
        
        //алогоритм преоброзования линейного словоря в матрицу кнопок калбека
        
        buttons.Add(new []{InlineKeyboardButton.WithCallbackData(text: "Повернутися ⬅️",
            callbackData: BaseField.BackToSettingsMenuCallback)});

        await _botClient.EditMessageTextAsync(viewDto.Update.GetUserId(), viewDto.Update.GetMessage().MessageId,
            text, replyMarkup: new InlineKeyboardMarkup(buttons.ToArray()));

    }

    [BotView(SettingsViewField.OpenChangeTimeZoneCallback)]
    public async Task ChangeTimeZoneCallback(ChangeTimeZoneCallbackVDto viewDto)
    {
        var text = "Змінити часову зону 🌐⏰\n" +
                   $"➡️ {viewDto.CurrentTimeZone}";

        var constCountLang = 3;
        var timeZoneVariants = viewDto.TimeZoneVariants;
        var buttons = _dynamicButtonCallbackComponent.CreateCollection(constCountLang, timeZoneVariants,
            BaseField.ChangeTimeZoneCallback);
        
        //алогоритм преоброзования линейного словоря в матрицу кнопок калбека
        buttons.Add(new []{InlineKeyboardButton.WithCallbackData(text: "Повернутися ⬅️",
            callbackData: BaseField.BackToSettingsMenuCallback)});

        await _botClient.EditMessageTextAsync(viewDto.Update.GetUserId(), viewDto.Update.GetMessage().MessageId,
            text, replyMarkup: new InlineKeyboardMarkup(buttons.ToArray()));
    }
    
    [BotView(SettingsViewField.OpenChangeTimesForDayCallback)]
    public async Task ChangeTimesForDayCallback(ChangeTimesForDayCallbackVDto viewDto)
    {
        var text = "Змінити кількість повторень 🔃⏰\n" +
                   $"➡️ {viewDto.CurrentTimesForDay}";

        var constCountLang = 3;
        var timesForDayVariants = viewDto.TimesForDayVariants;
        var buttons = _dynamicButtonCallbackComponent.CreateCollection(constCountLang, timesForDayVariants,
            BaseField.ChangeTimesForDayCallback);
        
        //алогоритм преоброзования линейного словоря в матрицу кнопок калбека
        buttons.Add(new []{InlineKeyboardButton.WithCallbackData(text: "Повернутися ⬅️",
            callbackData: BaseField.BackToSettingsMenuCallback)});

        await _botClient.EditMessageTextAsync(viewDto.Update.GetUserId(), viewDto.Update.GetMessage().MessageId,
            text, replyMarkup: new InlineKeyboardMarkup(buttons.ToArray()));
    }
    
    [BotView(SettingsViewField.StartInputFeedback)]
    public async Task StartInputFeedback(UpdateBDto updateBDto)
    {
        var text = "Введіть питання/відгук/скаргу 📝";

        var replyMarkup = new ReplyKeyboardMarkup(new[]
        {
            new KeyboardButton[] { BaseField.CancelInputFeedbackKeyboard }
        }) { ResizeKeyboard = true };

        await _botClient.SendTextMessageAsync(updateBDto.GetUserId(), text, replyMarkup: replyMarkup);
    }

    [BotView(SettingsViewField.SendFeedback)]
    public async Task SendFeedback(UpdateBDto updateBDto)
    {
        var text = "Ваше повідомлення відправлено ✉️";
        await _mainMenuComponent.SendAsync(updateBDto, text);
    }
    
    private (string, InlineKeyboardMarkup) SettingsMenuItem(string changeEmojiButton, string learnLanguagePairEmoji,
    string timeZone, string timesForDay)
    {
        var text = "Налаштування ⚙️";
        //TODO добавить инструкцию к кнопкам
        var ChangeLanguageEmojiButton = changeEmojiButton;
        InlineKeyboardMarkup inlineKeyboard = new(new[]
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData(text: $"Мова відображення: {ChangeLanguageEmojiButton}",
                    callbackData: BaseField.ChangeLearnSentencesModeCallback),
            },
            new[]
            {
            InlineKeyboardButton.WithCallbackData(text: $"Часовий пояс: {timeZone} 🌐⏰",
            callbackData: BaseField.OpenChangeTimeZoneCallback),
            },
            new[]
            {
                InlineKeyboardButton.WithCallbackData(text: $"Кількість повторень: {timesForDay} в день 🔃⏰",
                    callbackData: BaseField.OpenChangeTimesForDayCallback),
            },
            new[]
            {
                InlineKeyboardButton.WithCallbackData(text: $"Надіслати питання/відгук/скаргу 📝",
                    callbackData: BaseField.InputFeedbackCallback),
            }
        });
        return (text, inlineKeyboard);
    }
}
