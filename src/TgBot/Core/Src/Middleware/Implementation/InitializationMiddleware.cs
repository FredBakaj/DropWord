using DropWord.Application.UseCase.User.Commands.InitializeUser;
using DropWord.TgBot.Core.Extension;
using DropWord.TgBot.Core.Field.Controller;
using DropWord.TgBot.Core.Model;
using MediatR;

namespace DropWord.TgBot.Core.Src.Middleware.Implementation
{
    /// <summary>
    /// Первичная инициализация нового телеграмм аккаунта
    /// </summary>
    public class InitializationMiddleware : ABotMiddleware
    {
        private readonly ISender _sender;
        private readonly IConfiguration _configuration;

        public InitializationMiddleware(ISender sender, IConfiguration configuration)
        {
            _sender = sender;
            _configuration = configuration;
        }
        
        public override async Task Next(UpdateBDto update)
        {
            var interfaceLanguage = update.GetMessage()!.From!.LanguageCode;
            if (interfaceLanguage == null)
            {
                interfaceLanguage = _configuration.GetSection("UserSettings")["DefaultInterfaceLanguage"]!;
            }
            await _sender.Send(new InitializeUserCommand()
            {
                UserId = update.GetUserId(),
                State = StartControllerField.StartState,
                Action = StartControllerField.StartAction,
                InterfaceLanguage = interfaceLanguage
            });
            await base.Next(update);
        }
    }
}