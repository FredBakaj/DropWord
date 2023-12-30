namespace DropWord.TgBot.Core.Src.View;

public interface IBotView
{
    /// <summary>
    /// Элемент отображения, содержит в себе методы для отправки сообщений в телеграмм
    /// </summary>
    /// <returns></returns>
    Task<Dictionary<string, Func<object[], Task>>> GetMethodsAsync();
}
