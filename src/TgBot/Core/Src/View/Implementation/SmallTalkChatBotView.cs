using DropWord.Domain.Enums;
using DropWord.TgBot.Core.Attribute;
using DropWord.TgBot.Core.Extension;
using DropWord.TgBot.Core.Field;
using DropWord.TgBot.Core.Field.Controller;
using DropWord.TgBot.Core.Field.View;
using DropWord.TgBot.Core.Model;
using DropWord.TgBot.Core.ViewDto;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace DropWord.TgBot.Core.Src.View.Implementation;

public class SmallTalkChatBotView : ABotView
{
    private readonly ITelegramBotClient _botClient;
    private readonly ILogger<SmallTalkChatBotView> _logger;

    public SmallTalkChatBotView(ITelegramBotClient botClient, ILogger<SmallTalkChatBotView> logger)
    {
        _botClient = botClient;
        _logger = logger;
    }

    [BotView(SmallTalkChatViewField.StartSmallTalkChatAction)]
    public async Task StartSmallTalkChatAction(UpdateBDto updateBDto)
    {
        var text =
            $"Чат для спілкування і застосування своїх навичок на практиці. Для більш детальної інформації /{CommandField.HelpChat}. " +
            $"Щоб почати натисніть кнопку \"{SmallTalkChatField.SearchNewUserKeyboard}\"";

        var keyboard = GetMenuButtons();
        await _botClient.SendTextMessageAsync(updateBDto.GetUserId(), text, replyMarkup: keyboard);
    }

    [BotView(SmallTalkChatViewField.AutoChatAction)]
    public async Task AutoChatAction(UpdateBDto updateBDto)
    {
        var text = $"Натисніть кнопку \"{SmallTalkChatField.SearchNewUserKeyboard}\"";
        var keyboard = GetMenuButtons();
        await _botClient.SendTextMessageAsync(updateBDto.GetUserId(), text, replyMarkup: keyboard);
    }

    [BotView(SmallTalkChatViewField.CancelSearchKeyboard)]
    public async Task CancelSearchKeyboard(UpdateBDto updateBDto)
    {
        var text = $"Скасування пошуку ❌";
        var keyboard = GetMenuButtons();
        await _botClient.SendTextMessageAsync(updateBDto.GetUserId(), text, replyMarkup: keyboard);
    }

    [BotView(SmallTalkChatViewField.SearchNewUserKeyboard)]
    public async Task SearchNewUserKeyboard(UpdateBDto updateBDto)
    {
        var text = "Шукаємо 🔎";
        var keyboard = new ReplyKeyboardMarkup(new[]
        {
            new KeyboardButton[] { SmallTalkChatField.CancelSearchKeyboard }
        }) { ResizeKeyboard = true };
        await _botClient.SendTextMessageAsync(updateBDto.GetUserId(), text, replyMarkup: keyboard);
    }

    [BotView(SmallTalkChatViewField.SearchNewUserRunning)]
    public async Task SearchNewUserRunning(UpdateBDto updateBDto)
    {
        var text = "Пошук співрозмовника триває";
        await _botClient.SendTextMessageAsync(updateBDto.GetUserId(), text);
    }

    [BotView(SmallTalkChatViewField.SearchNewUserSuccessfulResult)]
    public async Task SearchNewUserSuccessfulResult(SearchNewUserSuccessfulResultVDto viewDto)
    {
        var text = $"👤<b>{viewDto.Name}</b>\n" +
                   $"📝 {viewDto.Interests}\n\n" +
                   $"➡️ Напишіть повідомлення ✉️";
        var keyboard = GetChatingMenuButtons();
        await _botClient.SendTextMessageAsync(viewDto.Update.GetUserId(), text, replyMarkup: keyboard,
            parseMode: ParseMode.Html);
    }

    [BotView(SmallTalkChatViewField.SearchNewUserBadResult)]
    public async Task SearchNewUserBadResult(UpdateBDto updateBDto)
    {
        var text = $"🔴 Виникла помилка";
        var keyboard = GetMenuButtons();
        await _botClient.SendTextMessageAsync(updateBDto.GetUserId(), text, replyMarkup: keyboard);
    }

    [BotView(SmallTalkChatViewField.SmallTalkWriteMessage)]
    public async Task SmallTalkWriteMessage(SmallTalkWriteMessageVDto viewDto)
    {
        var text = $"💬 <b>{viewDto.InterlocutorsName}</b>\n" +
                   $"{viewDto.Message}";
        await _botClient.SendTextMessageAsync(viewDto.Update.GetUserId(), text, parseMode: ParseMode.Html);
    }

    [BotView(SmallTalkChatViewField.SmallTalkEndChating)]
    public async Task SmallTalkEndChating(UpdateBDto updateBDto)
    {
        var text = $"Співрозмовник закінчив розмову 🫡";
        var keyboard = GetMenuButtons();
        await _botClient.SendTextMessageAsync(updateBDto.GetUserId(), text, replyMarkup: keyboard);
    }


    [BotView(SmallTalkChatViewField.SmallTalkAnalysisMessageSuccessful)]
    public async Task SmallTalkAnalysisMessageSuccessful(SmallTalkAnalysisMessageVDto viewDto)
    {
        var text = $"**Аналіз минулої переписки** 🧐\n" +
                   $"{viewDto.TextAnalysis}";
        await _botClient.SendTextMessageAsync(viewDto.Update.GetUserId(), text, parseMode: ParseMode.Markdown);
    }

    [BotView(SmallTalkChatViewField.SmallTalkAnalysisMessageSuccessfulAndContinueChat)]
    public async Task SmallTalkAnalysisMessageSuccessfulAndContinueChat(SmallTalkAnalysisMessageVDto viewDto)
    {
        var text = $"**Аналіз повідомлень** 🧐\n" +
                   $"{viewDto.TextAnalysis}\n" +
                   $"Можете продовжувати переписку ✍️";
        await _botClient.SendTextMessageAsync(viewDto.Update.GetUserId(), text, parseMode: ParseMode.Markdown);
    }

    [BotView(SmallTalkChatViewField.SmallTalkAnalysisMessageReanalysisError)]
    public async Task SmallTalkAnalysisMessageReanalysisError(UpdateBDto updateBDto)
    {
        var text = $"🟡 Вже було проаналізовано ці повідомлення, продовжуйте переписку";
        await _botClient.SendTextMessageAsync(updateBDto.GetUserId(), text);
    }

    [BotView(SmallTalkChatViewField.SmallTalkAnalysisMessageError)]
    public async Task SmallTalkAnalysisMessageError(UpdateBDto updateBDto)
    {
        var text = $"🔴 Під час аналізу сталася помилка";
        await _botClient.SendTextMessageAsync(updateBDto.GetUserId(), text);
    }

    [BotView(SmallTalkChatViewField.SmallTalkAnalysisMessageProcessing)]
    public async Task SmallTalkAnalysisMessageProcessing(UpdateBDto updateBDto)
    {
        var text = $"🟡 Аналіз запущено. Зачекайте";
        await _botClient.SendTextMessageAsync(updateBDto.GetUserId(), text);
    }

    [BotView(SmallTalkChatViewField.SmallTalkAnalysisMessageNoTalkMessagesError)]
    public async Task SmallTalkAnalysisMessageNoTalkMessagesError(UpdateBDto updateBDto)
    {
        var text = $"🔴 Немає повідомлень для аналізу, продовжуйте переписку";
        await _botClient.SendTextMessageAsync(updateBDto.GetUserId(), text);
    }

    [BotView(SmallTalkChatViewField.SmallTalkAnalysisMessageTooManyAnalysisHistoryError)]
    public async Task SmallTalkAnalysisMessageTooManyAnalysisHistoryError(UpdateBDto updateBDto)
    {
        //TODO тянуть из конфига цифру 3 в тексте
        var text = $"🔴 Досягнуто ліміт на аналіз повідомлень. На день доступно 3 аналізи";
        await _botClient.SendTextMessageAsync(updateBDto.GetUserId(), text);
    }

    [BotView(SmallTalkChatViewField.TooManyUserMessagesError)]
    public async Task SmallTalkAnalysisMessageTooManyUserMessagesError(UpdateBDto updateBDto)
    {
        //TODO тянуть из конфига цифру 20 в тексте
        var text = $"🔴 Досягнуто ліміт повідомлень. На день доступно 20 повідомлень";
        await _botClient.SendTextMessageAsync(updateBDto.GetUserId(), text);
    }

    [BotView(SmallTalkChatViewField.SmallTalkAnalysisMessageStartAnalysis)]
    public async Task SmallTalkAnalysisMessageStartAnalysis(UpdateBDto updateBDto)
    {
        var text = $"🟡 Розпочався аналіз";
        await _botClient.SendTextMessageAsync(updateBDto.GetUserId(), text);
    }

    [BotView(SmallTalkChatViewField.SelectGenderAction)]
    public async Task SelectGenderAction(UpdateBDto updateBDto)
    {
        var text =
            "Це анонімний чат 😊, тому ми призначимо вам випадкове ім'я для відображення. Оберіть стать для коректного звернення";

        var inlineButton = new InlineKeyboardMarkup(new[]
        {
            new InlineKeyboardButton[]
            {
                InlineKeyboardButton.WithCallbackData("🙋‍♂️",
                    SmallTalkChatField.SelectGenderCallback + $":{(int)UserGenderEnum.Man}"),
                InlineKeyboardButton.WithCallbackData("🙋‍♀️",
                    SmallTalkChatField.SelectGenderCallback + $":{(int)UserGenderEnum.Woman}")
            }
        });

        await _botClient.SendTextMessageAsync(updateBDto.GetUserId(), BaseField.ChatKeyboard,
            replyMarkup: new ReplyKeyboardRemove());
        await _botClient.SendTextMessageAsync(updateBDto.GetUserId(), text, replyMarkup: inlineButton);
    }

    [BotView(SmallTalkChatViewField.SelectedGenderCallback)]
    public async Task SelectedGenderCallback(SelectedGenderCallbackVDto viewDto)
    {
        var text = $"🟡 Ваше ім'я в чаті буде *\"{viewDto.Name}\"*";
        var messageId = viewDto.Update.GetMessage().MessageId;
        await _botClient.EditMessageTextAsync(viewDto.Update.GetUserId(), messageId, text, ParseMode.Markdown);
    }

    private ReplyKeyboardMarkup GetMenuButtons()
    {
        return new ReplyKeyboardMarkup(new[]
        {
            new KeyboardButton[] { SmallTalkChatField.SearchNewUserKeyboard, SmallTalkChatField.BackKeyboard },
            new KeyboardButton[] { SmallTalkChatField.AnalyzeMessagesKeyboard }
        }) { ResizeKeyboard = true };
    }

    private ReplyKeyboardMarkup GetChatingMenuButtons()
    {
        return new ReplyKeyboardMarkup(new[]
        {
            new KeyboardButton[] { SmallTalkChatField.SearchNextUserKeyboard, SmallTalkChatField.BackKeyboard },
            new KeyboardButton[] { SmallTalkChatField.AnalyzeMessagesKeyboard }
        }) { ResizeKeyboard = true };
    }
}
