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

public class StartBotView : ABotView
{
    private readonly ITelegramBotClient _botClient;
    private readonly IMainMenuComponent _mainMenuComponent;

    public StartBotView(ITelegramBotClient botClient, IMainMenuComponent mainMenuComponent)
    {
        _botClient = botClient;
        _mainMenuComponent = mainMenuComponent;
    }

    [BotView(StartViewField.Start)]
    public async Task StartAsync(UpdateBDto update)
    {
        var text =
            "*Вітаю!* 👋 \n" +
            "Цей бот призначений для полегшення процесу вивчення англійської мови, зосереджуючись на використанні слова в контексті речень. \n" +
            "За допомогою чергової системи повторень, подібної вивченню за допомогою карток, ви зможете додавати нові речення та повторювати їх. \n" +
            "Щоб додати речення, просто напишіть його, а бот автоматично перекладе і збереже його для вас. \n" +
            "Потім вам залишається лише повторювати навчальний матеріал. Нехай ваше вивчення мови буде ефективним та захоплюючим! 🫡\n\n";
        var replyMarkup =
            new ReplyKeyboardMarkup(new[] { new KeyboardButton[] { StartField.CompleteStartButton, } })
            {
                ResizeKeyboard = true
            };
        await _botClient.SendTextMessageMarkdown2Async(update.GetUserId(), text, replyMarkup: replyMarkup);
    }

    [BotView(StartViewField.FirstShowMenu)]
    public async Task FirstShowMenu(FirstShowMenuVDto viewDto)
    {
        var text = viewDto.TutorialText;
        await _mainMenuComponent.SendAsync(viewDto.Update, text);
    }
}
