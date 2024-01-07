using DropWord.TgBot.Core.Field.Controller;
using DropWord.TgBot.Core.Field.View;
using DropWord.TgBot.Core.Handler;
using DropWord.TgBot.Core.Model;

namespace DropWord.TgBot.Core.Src.Controller.Implementation
{
    public class BaseController : IBotController
    {
        private readonly IBotStateTreeHandler _botStateTreeHandler;
        private readonly IBotViewHandler _botViewHandler;
        
        public string Name() => BaseControllerField.BaseState;

        public BaseController(IBotStateTreeHandler botStateTreeHandler, IBotViewHandler botViewHandler)
        {
            _botStateTreeHandler = botStateTreeHandler;
            _botViewHandler = botViewHandler;

            Initialize();
        }

        public async Task Exec(UpdateBDto update)
        {
            await _botStateTreeHandler.ExecuteRoute(update);
        }

        private void Initialize()
        {
            _botStateTreeHandler.AddAction(BaseControllerField.BaseAction, BaseAction);
        }

        private async Task BaseAction(UpdateBDto update)
        {
            await _botViewHandler.SendAsync(BaseViewField.Intro, update);
        }

        
    }
}
