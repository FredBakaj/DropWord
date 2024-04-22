namespace DropWord.TgBot.Core.Manager.Info;

public interface IInfoManager
{
    public string TutorialText { get; }

    public Task SendBotCommandToUserAsync();
}
