namespace DropWord.TgBot.Core.Field.Controller
{
    public abstract class BaseField
    {
        public const string BaseState = nameof(BaseState);
        //BaseAction
        public const string BaseAction = nameof(BaseAction);

        public const string ResetCountRepeatSentencesCallback = nameof(ResetCountRepeatSentencesCallback);
        
        public const string RepeatSentenceKeyboard = "Повтор";
        public const string RepeatWordWriteKeyboard = nameof(RepeatWordWriteKeyboard);
        public const string NewSentenceButton = "Нове";
        public const string SwitchLanguageKeyboard = nameof(SwitchLanguageKeyboard);
        public const string SettingsKeyboard = "⚙️";
        public const string SentencesRepetitionByInputKeyboard = "Повтор ✍️";
        public const string DeleteSingleAddedSentenceCallback = nameof(DeleteSingleAddedSentenceCallback);
        public const string EditSingleAddedSentenceCallback = nameof(EditSingleAddedSentenceCallback);
        public const string DeleteAddedSentencesCallback = nameof(DeleteAddedSentencesCallback);
        public const string CancelEditSingleAddedSentenceCallback = nameof(CancelEditSingleAddedSentenceCallback);
        public const string SelectEditAddedSentenceLanguageCallback = nameof(SelectEditAddedSentenceLanguageCallback);
        
        
        //InputEditSentenceAction
        public const string InputEditSentenceAction = nameof(InputEditSentenceAction);
        
        //ReloadAction
        public const string ReloadAction = nameof(ReloadAction);

        //Settings
        public const string ChangeLearnSentencesModeCallback = nameof(ChangeLearnSentencesModeCallback);

    }
}
