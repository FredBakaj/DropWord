namespace DropWord.TgBot.Core.Field.Controller;

public abstract class SmallTalkChatField
{
    public const string SmallTalkChatState = nameof(SmallTalkChatState);
    public const string SmallTalkChatAction = nameof(SmallTalkChatAction);
    public const string BackKeyboard = "Повернутися 🍔";
    public const string SearchNewUserKeyboard = "Пошук 👤";
    public const string SearchNextUserKeyboard = "Пошук 👤🔄";
    public const string AnalyzeMessagesKeyboard = "Проаналізувати переписку 🤔";
    public const string CancelSearchKeyboard = "Відмінити пошук ❌";
    
    public const string SmallTalkWriteMessageAction = nameof(SmallTalkWriteMessageAction);
    public const string SearchingNewUserAction = nameof(SearchingNewUserAction);
}
