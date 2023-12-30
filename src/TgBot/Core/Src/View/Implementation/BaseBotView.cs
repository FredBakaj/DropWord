using Telegram.Bot;

namespace DropWord.TgBot.Core.Src.View.Implementation
{
    public class BaseBotView : ABotView
    {
        private readonly ITelegramBotClient _botClient;

        public BaseBotView(ITelegramBotClient botClient)
        {
            _botClient = botClient;
        }


        // [BotView(BaseViewField.ShowBaseMenu)]
        // public async Task ShowBaseMenu(UpdateBDto update)
        // {
        //     var text = "Головне меню";
        //
        //     var replyMarkup = new ReplyKeyboardMarkup(new[]
        //     {
        //         new KeyboardButton(BaseControllerField.MyBalanceKeyboard),
        //         new KeyboardButton(BaseControllerField.MyOrdersKeyboard)
        //     }) { ResizeKeyboard = true };
        //
        //     await _botClient.SendTextMessageAsync(update.GetUserId(), text, replyMarkup: replyMarkup);
        // }
    }
}