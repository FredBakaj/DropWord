using DropWord.TgBot.Core.Field;
using DropWord.TgBot.Core.Field.Controller;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace DropWord.TgBot.Core.Manager.Info.Implementation;

public class InfoManager : IInfoManager
{
    private readonly ITelegramBotClient _client;

    public InfoManager(ITelegramBotClient client)
    {
        _client = client;
    }
    public string TutorialText => "*Головне меню* 📺\n\n"+

                                  "⚪️ Щоб додати нове речення, просто напишіть його в чаті. Бот автоматично перекладе та збереже його для вас, а ви зможете повторювати матеріал.\n\n"+

                                  $"*{BaseField.NewSentenceButton}* => Всі речення, які ви додали, не будуть пропонуватися вам в повторі, оскільки їх немає ще в черзі."+
                                  "Ця кнопка буде додавати по одному реченню в чергу.\n\n"+

                                  $"*{BaseField.RepeatSentenceKeyboard}* => Кнопка повтору, після кожного натискання, вам відобразитися речення і його переклад в прихованому вигляді, ваше завдання перевести його, і після натиснути на ||приховане||, щоб переконатися, що ви його правельно перевели.\n\n"+

                                  $"*{BaseField.SentencesRepetitionByInputKeyboard}* => Кнопка повтору введенням, це режим, в якому вам потрібно ввести переклад речення, щоб визначити чи правильно ви його переклали.\n\n"+

                                  "*Налаштування*⚙️:\n"+
                                  "   🔘 *Мова відображення* 🇺🇦 => Дозволяє вибрати, яка мова має бути ||прихованою|| під час повторення.\n"+
                                  "   🔘 *Кількість повторень* 🔃⏰ => Дозволяє вибрати, скільки разів на день вам надсилати повідомлення повтору.\n"+
                                  "   🔘 *Часовий пояс* 🌐⏰ => Дозволяє встановити часовий пояс, у якому ви перебуваєте. Це необхідно для визначення часу надсилання повідомлень повтору.\n";

    public async Task SendBotCommandToUserAsync()
    {
        List<BotCommand> commands = new List<BotCommand>()
        {
            new BotCommand(){Command = CommandField.Reload, Description = "Перезавантажити бот"},
            new BotCommand(){Command = CommandField.Tutorial, Description = "Керівництво керування ботом"}
        };
        await _client.SetMyCommandsAsync(commands);
    }
}
