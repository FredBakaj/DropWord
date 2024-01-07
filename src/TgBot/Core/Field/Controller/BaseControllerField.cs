namespace DropWord.TgBot.Core.Field.Controller
{
    public abstract class BaseControllerField
    {
        public const string BaseState = nameof(BaseState);
        public const string BaseAction = nameof(BaseAction);

        public const string RepeatWordKeyboard = nameof(RepeatWordKeyboard);
        public const string RepeatWordWriteKeyboard = nameof(RepeatWordWriteKeyboard);
        public const string NewWordKeyboard = nameof(NewWordKeyboard);
        public const string SwitchLanguageKeyboard = nameof(SwitchLanguageKeyboard);
        public const string SettingsKeyboard = nameof(SettingsKeyboard);
    }
}
