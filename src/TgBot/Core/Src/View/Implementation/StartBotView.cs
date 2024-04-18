using DropWord.TgBot.Core.Attribute;
using DropWord.TgBot.Core.Extension;
using DropWord.TgBot.Core.Field.Controller;
using DropWord.TgBot.Core.Field.View;
using DropWord.TgBot.Core.Model;
using DropWord.TgBot.Core.ViewComponent;
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
            "Потім вам залишається лише повторювати навчальний матеріал. Нехай ваше вивчення мови буде ефективним та захоплюючим! 🫡\n\n" +

            "*Далі* ➡️";
        var replyMarkup =
            new ReplyKeyboardMarkup(new[] { new KeyboardButton[] { StartField.CompleteStartButton, } })
            {
                ResizeKeyboard = true
            };
        await _botClient.SendTextMessageMarkdown2Async(update.GetUserId(), text, replyMarkup: replyMarkup);
    }
    
    [BotView(StartViewField.FirstShowMenu)]
    public async Task FirstShowMenu(UpdateBDto update)
    {
        var text =
            "*Головне меню* 📺\n\n"+

            "⚪️ Щоб додати нове речення, просто напишіть його в чаті. Бот автоматично перекладе та збереже його для вас, а ви зможете повторювати матеріал.\n\n"+

            $"*{BaseField.NewSentenceButton}* => Всі речення, які ви додали, не будуть пропонуватися вам в повторі, оскільки їх немає ще в черзі."+
            "Ця кнопка буде додавати по одному реченню в чергу.\n\n"+

            $"*{BaseField.RepeatSentenceKeyboard}* => Кнопка повтору, після кожного натискання, вам відобразитися речення і його переклад в прихованому вигляді, ваше завдання перевести його, і після натиснути на ||приховане||, щоб переконатися, що ви його правельно перевели.\n\n"+

            $"*{BaseField.SentencesRepetitionByInputKeyboard}* => Кнопка повтору введенням, це режим, в якому вам потрібно ввести переклад речення, щоб визначити чи правильно ви його переклали.\n\n"+

            "*Налаштування*⚙️:\n"+
            "   🔘 *Мова відображення* 🇺🇦 => Дозволяє вибрати, яка мова має бути ||прихованою|| під час повторення.\n"+
            "   🔘 *Кількість повторень* 🔃⏰ => Дозволяє вибрати, скільки разів на день вам надсилати повідомлення повтору.\n"+
            "   🔘 *Часовий пояс* 🌐⏰ => Дозволяє встановити часовий пояс, у якому ви перебуваєте. Це необхідно для визначення часу надсилання повідомлень повтору.\n";

        await _mainMenuComponent.SendAsync(update, text);
    }
}
