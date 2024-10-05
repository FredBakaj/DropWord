using DropWord.TgBot.Core.Attribute;
using DropWord.TgBot.Core.Extension;
using DropWord.TgBot.Core.Field.Controller;
using DropWord.TgBot.Core.Field.View;
using DropWord.TgBot.Core.Model;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace DropWord.TgBot.Core.Src.View.Implementation;

public class SmallTalkChatBotView: ABotView
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
        var text = "Привіт, я діяч DropWord! Ти можеш знайти людину, яка хочеш знати деталі про або знати більше про мене.";
        var keyboard = new ReplyKeyboardMarkup(new[]
        {
            new KeyboardButton[]
            {
                SmallTalkChatField.SearchNewUserKeyboard, SmallTalkChatField.BackKeyboard
            }
        }) { ResizeKeyboard = true };
        await _botClient.SendTextMessageAsync(updateBDto.GetUserId(), text, replyMarkup:keyboard);
    }
    [BotView(SmallTalkChatViewField.SearchNewUserKeyboard)]
    public async Task SearchNewUserKeyboard(UpdateBDto updateBDto)
    {
        var text = "Пошук людини";
        await _botClient.SendTextMessageAsync(updateBDto.GetUserId(), text);
    }

}
