using DropWord.Application.StateTree.Commands.GetStateAndAction;
using DropWord.Domain.Entities;
using DropWord.TgBot.Core.Extension;
using DropWord.TgBot.Core.Model;
using MediatR;

namespace DropWord.TgBot.Core.Src.Middleware.Implementation
{
    /// <summary>
    /// Определяет состояние пользователя
    /// </summary>
    public class StateMiddleware : ABotMiddleware
    {
        private readonly ISender _sender;


        public StateMiddleware(ISender sender)
        {
            _sender = sender;
        }

        public override async Task Next(UpdateBDto update)
        {
            if (update.TelegramState == null)
            {
                var userModel = await _sender.Send(new GetStateAndActionCommand(){UserId = update.GetUserId()});
                var userModel_ = userModel as StateTreeEntity;
                update.TelegramState =
                    new StateTreeBDto() { State = userModel_!.State, Action = userModel_!.Action };
            }

            //await base.Next(update);
        }
    }
}