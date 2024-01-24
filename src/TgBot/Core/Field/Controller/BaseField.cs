namespace DropWord.TgBot.Core.Field.Controller
{
    public abstract class BaseField
    {
        public const string BaseState = nameof(BaseState);
        public const string BaseAction = nameof(BaseAction);

        public const string ResetCountRepeatSentencesCallback = nameof(ResetCountRepeatSentencesCallback);
        
        public const string RepeatSentenceKeyboard = "Повтор";
        public const string RepeatWordWriteKeyboard = nameof(RepeatWordWriteKeyboard);
        public const string NewSentenceButton = "Нове";
        public const string SwitchLanguageKeyboard = nameof(SwitchLanguageKeyboard);
        public const string SettingsKeyboard = nameof(SettingsKeyboard);
    }
}
