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
            "Вітаю! 👋\n" +
            "Цей бот був розроблений,\n" +
            "щоб допомагати людям вивчати англійську. \n" +
            "А саме спростити і автомотизувати процес вивчення слів. \n" +
            "Оскільки багато викладачів рекомендують\n" +
            "вивчати слова в контексті речень,\n" +
            "ми вирішили зробити акцент на цьому підході. \n" +
            "Сподіваємося вам це допоможе 🫡\n\n" +

            "Як користуватися ботам 📋\n" +
            "Вид повторення реалізований у вигляді черги, \n" +
            "схоже на те, як люди повторюють за картками 🎴.\n" +
            "Ваші основні завдання це додавати нові речення і повторювати їх. \n" +
            "Речення додається наступним чином\n" +
            "просто напишіть його,\n" +
            "далі бот сам перекладе і збереже. \n" +
            "Після цього залишається тільки повторювати.\n\n" +

            "Далі ➡️\n";
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
            "*Головне меню* 📺\n\n" +
            "⚪️ Потрібно написати речення в цей чат,\n"+
            "для того щоб додати його.\n\n"+
            
            "*Нове* 🆕 => Всі речення, які ви додали,\n" +
            "не будуть пропонуватися вам в повторі, \n" +
            "оскільки їх немає ще в черзі.\n" +
            "Ця кнопка буде додавати по одному реченню в чергу.\n\n" +
            
            "*Повтор* 🎴 => Кнопка повтору,\n" +
            "після кожного натискання,\n" +
            "вам відобразитися речення і його переклад\n" +
            "в прихованому вигляді, \n" +
            "ваше завдання перевести його,\n" +
            "і після натиснути на ||приховане||, щоб переконатися, \n" +
            "що ви його правельно перевели.\n\n" +

            "*Повтор* ✍️ => Кнопка повтору введенням, \n" +
            "це режим, в якому вам потрібно ввести переклад речення, \n" +
            "щоб визначити чи правильно ви його переклали.\n\n" +

            " *Налаштування*⚙️:\n" +
            "   🔘 *Мова відображення* 🇺🇦 => Дозволяє вибрати, \n" +
            "яка мова має бути ||прихованою|| під час повторення.\n" +
            "   🔘 *Кількість повторень* 🔃⏰=> Дозволяє вибрати, \n" +
            "скільки разів на день вам надсилати повідомлення повтору.\n" +
            "   🔘 *Часовий пояс* 🌐⏰=> Дозволяє встановити часовий пояс,\n" +
            "у якому ви перебуваєте. Це необхідно для визначення\n" +
            "часу надсилання повідомлень повтору.\n";

        await _mainMenuComponent.SendAsync(update, text);
    }
}
