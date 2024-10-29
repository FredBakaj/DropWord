namespace DropWord.TgBot.Core.Manager.Info;

public interface IInfoManager
{
    public string HelpText { get; }
    public string HelpRepeatSentencesText { get; }
    public string HelpNewSentencesText { get; }
    public string HelpChatText { get; }
    public string HelpSettingsText { get; }

    public Task SendBotCommandToUserAsync();
}
