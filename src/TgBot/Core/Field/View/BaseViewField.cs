namespace DropWord.TgBot.Core.Field.View;

public abstract class BaseViewField
{
    public const string Menu = nameof(Menu);
    public const string AddSentences = nameof(AddSentences);
    public const string AddSentence = nameof(AddSentence);
    public const string NewSentence = nameof(NewSentence);
    public const string RepeatSentence = nameof(RepeatSentence);
    public const string ResetCountRepeatSentence = nameof(ResetCountRepeatSentence);
    public const string ConfirmResetCountRepeatSentence = nameof(ConfirmResetCountRepeatSentence);
    public const string ResetOutOfSentencesToRepeat = nameof(ResetOutOfSentencesToRepeat);
    public const string EmptyCollectionOfSentencesToRepeat = nameof(EmptyCollectionOfSentencesToRepeat);
    public const string EditSentence = nameof(EditSentence);
    public const string InputEditSentence = nameof(InputEditSentence);
    public const string DeleteAddedSentence = nameof(DeleteAddedSentence);
    public const string CancelEditAddedSentence = nameof(CancelEditAddedSentence);
    public const string DeleteAddedSentenceCollection = nameof(DeleteAddedSentenceCollection);
    public const string MaxCountSentencesException = nameof(MaxCountSentencesException);
    public const string MaxLengthSentenceException = nameof(MaxLengthSentenceException);
    public const string SentencesNotValidForAddException = nameof(SentencesNotValidForAddException);
    public const string LimitAddSentencesExceededException = nameof(LimitAddSentencesExceededException);
    public const string NoNewSentenceException = nameof(NoNewSentenceException);
    public const string DeleteAddedSentenceFailed = nameof(DeleteAddedSentenceFailed);
    public const string InvalidDataException = nameof(InvalidDataException);
    public const string TryAddOneWordException = nameof(TryAddOneWordException);
}
