using DropWord.TgBot.Core.Attribute;
using DropWord.TgBot.Core.Extension;
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
            "Привіт, я діяч DropWord! Ти можеш знайти людину, яка хочеш знати деталі про або знати більше про мене.";
        var keyboard = GetMenuButton();
        await _botClient.SendTextMessageAsync(updateBDto.GetUserId(), text, replyMarkup: keyboard);
    }
    [BotView(SmallTalkChatViewField.AutoChatAction)]

    public async Task AutoChatAction(UpdateBDto updateBDto)
    {
        var text = $"Натисніть на \"{SmallTalkChatField.SearchNewUserKeyboard}\". Щоб розпочати";
        var keyboard = GetMenuButton();
        await _botClient.SendTextMessageAsync(updateBDto.GetUserId(), text, replyMarkup: keyboard);
    }
    
    [BotView(SmallTalkChatViewField.CancelSearchKeyboard)]

    public async Task CancelSearchKeyboard(UpdateBDto updateBDto)
    {
        var text = $"Скасування пошуку ❌";
        var keyboard = GetMenuButton();
        await _botClient.SendTextMessageAsync(updateBDto.GetUserId(), text, replyMarkup: keyboard);
    }

    [BotView(SmallTalkChatViewField.SearchNewUserKeyboard)]
    public async Task SearchNewUserKeyboard(UpdateBDto updateBDto)
    {
        var text = "Пошук людини";
        var keyboard = new ReplyKeyboardMarkup(new[]
        {
            new KeyboardButton[] {SmallTalkChatField.CancelSearchKeyboard }
        }) { ResizeKeyboard = true };
        await _botClient.SendTextMessageAsync(updateBDto.GetUserId(), text, replyMarkup:keyboard);
    }

    [BotView(SmallTalkChatViewField.SearchNewUserRunning)]
    public async Task SearchNewUserRunning(UpdateBDto updateBDto)
    {
        var text = "Ще шукаемо!";
        await _botClient.SendTextMessageAsync(updateBDto.GetUserId(), text);
    }

    [BotView(SmallTalkChatViewField. SearchNewUserSuccessfulResult)]
    public async Task SearchNewUserSuccessfulResult(SearchNewUserSuccessfulResultVDto viewDto)
    {
        var text = $"🪂 <b>Знайшовся співрозмовник</b> \n" +
                   $"👤 {viewDto.Name} \n\n" +
                   $"📝 {viewDto.Interests}\n";
        var keyboard = GetMenuButton();
        await _botClient.SendTextMessageAsync(viewDto.Update.GetUserId(), text, replyMarkup:keyboard, parseMode:ParseMode.Html);
    }

    [BotView(SmallTalkChatViewField.SearchNewUserBadResult)]
    public async Task SearchNewUserBadResult(UpdateBDto updateBDto)
    {
        var text = $"Виникла помилка";
        var keyboard = GetMenuButton();
        await _botClient.SendTextMessageAsync(updateBDto.GetUserId(), text, replyMarkup:keyboard);
    }

    [BotView(SmallTalkChatViewField.SmallTalkWriteMessage)]
    public async Task SmallTalkWriteMessage(SmallTalkWriteMessageVDto viewDto)
    {
        var text = $"👤 <b>{viewDto.InterlocutorsName}</b>\n" +
                   $"{viewDto.Message}";
        await _botClient.SendTextMessageAsync(viewDto.Update.GetUserId(), text, parseMode:ParseMode.Html);
    }

    [BotView(SmallTalkChatViewField.SmallTalkEndChating)]

    public async Task SmallTalkEndChating(UpdateBDto updateBDto)
    {
        var text = $"Співрозмовник закінчив розмову 🫡";
        var keyboard = GetMenuButton();
        await _botClient.SendTextMessageAsync(updateBDto.GetUserId(), text, replyMarkup: keyboard);
    }
    
    private ReplyKeyboardMarkup GetMenuButton()
    {
        return new ReplyKeyboardMarkup(new[]
        {
            new KeyboardButton[] { SmallTalkChatField.SearchNewUserKeyboard, SmallTalkChatField.BackKeyboard }
        }) { ResizeKeyboard = true };
    }
}
