using Telegram.Bot.Types.ReplyMarkups;

namespace DropWord.TgBot.Core.Field.Controller;

public abstract class SentencesRepetitionByInputField
{
    public const string State = "SentencesRepetitionByInputState";
    public const string Action = "SentencesRepetitionByInputAction";

    public const string BackKeyboard = "Закінчити ⬅️";
    
    public const string ResetCountRepeatSentencesCallback = nameof(ResetCountRepeatSentencesCallback);
}
