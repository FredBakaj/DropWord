using DropWord.Application.UseCase.Sentence.Commands.AddSentence;
using DropWord.TgBot.Core.Extension;
using DropWord.TgBot.Core.Field.Controller;
using DropWord.TgBot.Core.Field.View;
using DropWord.TgBot.Core.Handler;
using DropWord.TgBot.Core.Model;
using DropWord.TgBot.Core.ViewDto;
using MediatR;

namespace DropWord.TgBot.Core.Src.Controller.Implementation
{
    public class BaseController : IBotController
    {
        private readonly IBotStateTreeHandler _botStateTreeHandler;
        private readonly IBotViewHandler _botViewHandler;
        private readonly ISender _sender;

        public string Name() => BaseControllerField.BaseState;

        public BaseController(IBotStateTreeHandler botStateTreeHandler, IBotViewHandler botViewHandler, ISender sender)
        {
            _botStateTreeHandler = botStateTreeHandler;
            _botViewHandler = botViewHandler;
            _sender = sender;

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
            var addedSentences = await _sender.Send(new AddSentenceCommand()
            {
                UserId = update.GetUserId(), Content = update.GetMessage().Text!
            });
            var viewDto = new AddSentencesVDto() { Update = update, Sentences = addedSentences };
            if (addedSentences.Count() == 1)
            {
                await _botViewHandler.SendAsync(BaseViewField.AddSentence, viewDto);
                
            }
            else if(addedSentences.Count() > 1)
            {
                await _botViewHandler.SendAsync(BaseViewField.AddSentences, viewDto);
            }
        }
    }
}
