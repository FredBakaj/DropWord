using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace DropWord.TgBot.Core.Extension;

public static class TelegramBotClientExtension
{
    public static async Task<Message> SendTextMessageMarkdown2Async(this ITelegramBotClient client, long chatId, string message,
        IReplyMarkup? replyMarkup = null)
    {
        message = message.Replace("[", "\\[");
        message = message.Replace("]", "\\]");
        message = message.Replace("(", "\\(");
        message = message.Replace(")", "\\)");
        message = message.Replace("`", "\\`");
        message = message.Replace(">", "\\>");
        message = message.Replace("#", "\\#");
        message = message.Replace("+", "\\+");
        message = message.Replace("-", "\\-");
        message = message.Replace("=", "\\=");
        message = message.Replace("{", "\\{");
        message = message.Replace("}", "\\}");
        message = message.Replace(".", "\\.");
        message = message.Replace("!", "\\!");

        return await client.SendTextMessageAsync(chatId, message, replyMarkup: replyMarkup, parseMode: ParseMode.MarkdownV2);
    }
}
