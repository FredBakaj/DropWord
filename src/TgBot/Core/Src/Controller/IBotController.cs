using DropWord.TgBot.Core.Model;

namespace DropWord.TgBot.Core.Src.Controller
{
    public interface IBotController
    {
        /// <summary>
        /// Возвращает имя состояния к которому относиться контролер
        /// </summary>
        /// <returns></returns>
        public  string Name();
        /// <summary>
        /// Входная точка для обработки логики в контролере
        /// </summary>
        /// <param name="update"></param>
        /// <returns></returns>
        public Task Exec(UpdateBDto update);
    }
}
