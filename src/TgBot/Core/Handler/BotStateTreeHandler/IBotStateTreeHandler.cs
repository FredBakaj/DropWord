using DropWord.TgBot.Core.Model;

namespace DropWord.TgBot.Core.Handler.BotStateTreeHandler
{
    /// <summary>
    /// Отвечает за вызов нужного метода в контролерах состяний
    /// </summary>
    public interface IBotStateTreeHandler
    {
        /// <summary>
        /// Определяет тип события и по нему вызывает нужный метод
        /// </summary>
        /// <param name="updateBDto"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public Task ExecuteRoute(UpdateBDto updateBDto);
        /// <summary>
        /// Биндит метод для отработки Дейсвия в котором находиться пользователь
        /// </summary>
        /// <param name="action"></param>
        /// <param name="func"></param>
        public void AddAction(string action, Func<UpdateBDto, Task> func);
        /// <summary>
        /// Биндит метод для кнопок под сообщениями в телеграмме
        /// </summary>
        /// <param name="action"></param>
        /// <param name="callback"></param>
        /// <param name="func"></param>
        public void AddCallback(string action, string callback, Func<UpdateBDto, Task> func);
        /// <summary>
        /// Биндит метод для кнопок под полем ввода текста в телеграмме
        /// </summary>
        /// <param name="action"></param>
        /// <param name="keyboard"></param>
        /// <param name="func"></param>
        public void AddKeyboard(string action, string keyboard, Func<UpdateBDto, Task> func);
    }
}
