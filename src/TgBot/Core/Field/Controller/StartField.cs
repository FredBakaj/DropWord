namespace DropWord.TgBot.Core.Field.Controller;

public abstract class StartField
{
    public const string StartState = "StartState";
    public const string StartAction = "StartAction";
    
    public const string CompleteStartAction = nameof(CompleteStartAction);

    
    // public const string SelectLanguageAction = "SelectLanguageAction";
    //
    // public const string UkrainianEnglishLanguageButton = "🇺🇦 🇺🇸";
    // public const string UkrainianGermanLanguageButton = "🇺🇦 🇩🇪";
    // public const string UkrainianPolishLanguageButton = "🇺🇦 🇵🇱";
    // public const string UkrainianFrenchLanguageButton = "🇺🇦 🇫🇷";

    public const string CompleteStartButton = "Далі ➡️";
}
