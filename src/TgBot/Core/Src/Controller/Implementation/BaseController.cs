using DropWord.Application.UseCase.Sentence.Commands.AddSentence;
using DropWord.Application.UseCase.Sentence.Commands.LearnNewSentence;
using DropWord.Application.UseCase.Sentence.Queries.GetNewSentence;
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
            _botStateTreeHandler.AddKeyboard(BaseControllerField.BaseAction, BaseControllerField.NewSentenceButton, NewSentenceButton);
        }

        //Добавление предложений в базу 
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
            else if (addedSentences.Count() > 1)
            {
                await _botViewHandler.SendAsync(BaseViewField.AddSentences, viewDto);
            }
        }

        private async Task NewSentenceButton(UpdateBDto update)
        {
            var newSentenceDto = await _sender.Send(new GetNewSentenceQuery()
            {
                UserId = update.GetUserId()
            });

            var viewDto = new NewSentenceVDto() { Update = update, NewSentence = newSentenceDto };
            await _botViewHandler.SendAsync(BaseViewField.NewSentence, viewDto);

            await _sender.Send(new LearnNewSentenceCommand()
            {
                UserId = update.GetUserId(), SentencePairId = newSentenceDto.SentencePairId
            });
        }
    }
}
