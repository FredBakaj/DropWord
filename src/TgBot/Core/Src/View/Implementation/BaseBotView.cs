using DropWord.TgBot.Core.Attribute;
using DropWord.TgBot.Core.Extension;
using DropWord.TgBot.Core.Field.View;
using DropWord.TgBot.Core.Model;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace DropWord.TgBot.Core.Src.View.Implementation
{
    public class BaseBotView : ABotView
    {
        private readonly ITelegramBotClient _botClient;

        public BaseBotView(ITelegramBotClient botClient)
        {
            _botClient = botClient;
        }

        private async Task MainMenu(UpdateBDto update, string text)
        {
            var replyMarkup = new ReplyKeyboardMarkup(new[]
            {
                new KeyboardButton[]{"Повтор", "Повтор ✍️" ,"Нове"},
                new KeyboardButton[]{"🇬🇧    🔃 🇬🇧", "⚙️"}
            }) { ResizeKeyboard = true };

            await _botClient.SendTextMessageAsync(update.GetUserId(), text, replyMarkup: replyMarkup);
        }

        [BotView(BaseViewField.Intro)]
        public async Task Intro(UpdateBDto update)
        {
            var text = "Головне меню";
            await MainMenu(update, text);
        }
    }
}
