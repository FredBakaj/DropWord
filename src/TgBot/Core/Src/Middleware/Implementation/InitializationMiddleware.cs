using DropWord.TgBot.Core.Model;

namespace DropWord.TgBot.Core.Src.Middleware.Implementation
{
    /// <summary>
    /// Первичная инициализация нового телеграмм аккаунта
    /// </summary>
    public class InitializationMiddleware : ABotMiddleware
    {
        
        public override async Task Next(UpdateBDto update)
        {
            await base.Next(update);
        }
    }
}