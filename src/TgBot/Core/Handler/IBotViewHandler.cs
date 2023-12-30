namespace DropWord.TgBot.Core.Handler;

public interface IBotViewHandler
{
    /// <summary>
    /// Отправляет сообщение в телеграмм, структура сообщения реализуеться в объектах AViewItem
    /// </summary>
    /// <param name="view">Нейминг сообщения(метода имеющего атрибут ViewAttribute)</param>
    /// <param name="model">Модель для передачи в сообщение динамических данных</param>
    /// <returns></returns>
    Task SendAsync<T>(string view, T model);
}
