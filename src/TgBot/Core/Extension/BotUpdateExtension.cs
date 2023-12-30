using System.Diagnostics;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace DropWord.TgBot.Core.Extension
{
    public static class BotUpdateExtension
    {
        public static Message GetMessage(this Update update)
        {
            if (update.Message != null)
            {
                return update.Message;
            }
            else if (update.CallbackQuery is not null && update.CallbackQuery.Message is not null)
            {
                return update.CallbackQuery.Message;
            }
            throw new NullReferenceException("Update not have message");
        }

        public static bool IsBot(this Update update)
        {
            if (update.Message != null)
            {
                return update.Message.From!.IsBot;
            }
            else if (update.CallbackQuery != null)
            {
                return update.CallbackQuery.Message!.From!.IsBot;
            }
            else if (update.MyChatMember?.NewChatMember != null)
            {
                return update.MyChatMember.NewChatMember.User.IsBot;
            }
            throw new NullReferenceException("Update not have message");
        }

        public static ChatType GetChatType(this Update update)
        {
            if (update.Message != null)
            {
                return update.Message.Chat.Type;
            }
            else if (update.CallbackQuery != null)
            {
                if (update.CallbackQuery.Message != null)
                {
                    return update.CallbackQuery.Message.Chat.Type;
                }
            }
            else if (update.MyChatMember?.Chat.Type != null)
            {
                return update.MyChatMember.Chat.Type;
            }
            throw new NullReferenceException("Update not have message");
        }
        public static long GetUserId(this Update update)
        {
            if (update.Message != null)
            {
                Debug.Assert(update.Message.From != null, "update.Message.From != null");
                return update.Message.From.Id;
            }
            else if (update.CallbackQuery != null)
            {
                return update.CallbackQuery.From.Id;
            }
            else if (update.MyChatMember != null)
            {
                return update.MyChatMember.From.Id;
            }
            throw new NullReferenceException("Update not have user id");
        }
        public static long GetChatId(this Update update)
        {
            if (update.Message != null)
            {
                return update.Message.Chat.Id;
            }
            else if (update.CallbackQuery != null)
            {
                if (update.CallbackQuery.Message != null)
                {
                    return update.CallbackQuery.Message.Chat.Id;
                }
            }
            else if (update.MyChatMember != null)
            {
                return update.MyChatMember.Chat.Id;
            }
            throw new NullReferenceException("Update not have chat id");
        }
    }
}
