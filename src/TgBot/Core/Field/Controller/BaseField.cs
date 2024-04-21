namespace DropWord.TgBot.Core.Field.Controller
{
    public abstract class BaseField
    {
        public const string BaseState = nameof(BaseState);
        //BaseAction
        public const string BaseAction = nameof(BaseAction);

        public const string ResetCountRepeatSentencesCallback = nameof(ResetCountRepeatSentencesCallback);
        
        public const string RepeatSentenceKeyboard = "Повтор 🎴";
        public const string NewSentenceButton = "Нове 🆕";
        public const string SettingsKeyboard = "Налаштування ⚙️";
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
        public const string OpenChangeLearnLanguagePairCallback = nameof(OpenChangeLearnLanguagePairCallback);
        public const string ChangeLearnLanguagePairCallback = nameof(ChangeLearnLanguagePairCallback);
        public const string BackToSettingsMenuCallback = nameof(BackToSettingsMenuCallback);
        public const string OpenChangeTimeZoneCallback = nameof(OpenChangeTimeZoneCallback);
        public const string OpenChangeTimesForDayCallback = nameof(OpenChangeTimesForDayCallback);
        public const string ChangeTimeZoneCallback = nameof(ChangeTimeZoneCallback);
        public const string ChangeTimesForDayCallback = nameof(ChangeTimesForDayCallback);
        public const string InputFeedbackAction = nameof(InputFeedbackAction);
        public const string CancelInputFeedbackKeyboard = "Скасувати ❌";
        public const string InputFeedbackCallback = nameof(InputFeedbackCallback);
    }
}
