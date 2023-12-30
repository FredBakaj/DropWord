namespace DropWord.TgBot.Core.Factory;

public interface IFactory<IT>
{
    /// <summary>
    /// Метод создания обекта
    /// </summary>
    /// <param name="value">моделя с динамичискими параметрами для создания обекта</param>
    /// <returns></returns>
    Task<IT> CreateAsync(object value);
}
