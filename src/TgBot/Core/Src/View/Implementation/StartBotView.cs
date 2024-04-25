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
            "Вітаю! 👋 \n\n" +
            "Одразу вбачаємося за таке довге вітання🫤\n\n" +
            "Цей бот націлений допомогти вам в процесі вивчення англійської мови.\n\n" +
            "Кожне слово, має вагу тільки, коли поряд є інші слова. Запам'ятавши один переклад, ви зіштовхнетеся з тим що в контексті це слово набуває іншого змісту. Не кажучи про те, що запам'ятовувати по одному слово дуже важко. \n\n" +
            "Це наслідок специфічної роботи нашої пам'яті, вона запам’ятовує тільки те що має якусь цінність для нас. А цінність набувається в двох випадках. По-перше, коли ми переживаємо сильні емоції, наприклад, коли ми можемо повторити фразу з фільму, який дивилися давно. В той момент, коли ми переглядали цей фільм, ми пережили сильні відчуття, які надовго залишилися в нашій пам'яті. По-друге, коли ми стикаємося з чимось багаторазово, це також сприяє його запам'ятовуванню.\n\n" +
            "На цьому ми і хочемо зробити акцент." +
            "За допомогою чергової системи повторень, ви зможете додавати тільки ті речення, які вам цікаві, або викликають у вас емоційний відгук. І повторювати в зручний для вас час.\n\n" +
            "Нехай ваше вивчення мови буде ефективним та захоплюючим! 🫡\n\n";

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
