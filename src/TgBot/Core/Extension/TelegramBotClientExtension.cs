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
        message = MessageReplaceSymbols(message);
        return await client.SendMessage(chatId, message, replyMarkup: replyMarkup, parseMode: ParseMode.MarkdownV2);
    }

    public static async Task<Message> EditTextMessageMarkdown2Async(this ITelegramBotClient client, long chatId,
        int messageId,
        string message,
        InlineKeyboardMarkup? replyMarkup = null)
    {
        message = MessageReplaceSymbols(message);
        return await client.EditMessageText(chatId, messageId, message, replyMarkup: replyMarkup, parseMode: ParseMode.MarkdownV2);

    }

    private static string MessageReplaceSymbols(string message)
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
        return message;
    }
}
