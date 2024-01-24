using DropWord.Application.UseCase.Sentence.Commands.AddSentence;
using DropWord.Application.UseCase.Sentence.Commands.LearnNewSentence;
using DropWord.Application.UseCase.Sentence.Commands.RepeatSentence;
using DropWord.Application.UseCase.Sentence.Commands.ResetCountRepeatSentence;
using DropWord.Application.UseCase.Sentence.Queries.GetCountRepetitionSentences;
using DropWord.Application.UseCase.Sentence.Queries.GetNewSentence;
using DropWord.Application.UseCase.Sentence.Queries.GetSentenceForRepeat;
using DropWord.Domain.Exceptions;
using DropWord.TgBot.Core.Extension;
using DropWord.TgBot.Core.Field.Controller;
using DropWord.TgBot.Core.Field.View;
using DropWord.TgBot.Core.Handler;
using DropWord.TgBot.Core.Model;
using DropWord.TgBot.Core.StateDto;
using DropWord.TgBot.Core.ViewDto;
using MediatR;

namespace DropWord.TgBot.Core.Src.Controller.Implementation
{
    public class BaseController : IBotController
    {
        private readonly IBotStateTreeHandler _botStateTreeHandler;
        private readonly IBotViewHandler _botViewHandler;
        private readonly ISender _sender;
        private readonly IBotStateTreeUserHandler _botStateTreeUserHandler;

        private readonly int _offerResetCountRepeatSentences;

        public string Name() => BaseField.BaseState;

        public BaseController(IBotStateTreeHandler botStateTreeHandler, IBotViewHandler botViewHandler, ISender sender,
            IConfiguration configuration, IBotStateTreeUserHandler botStateTreeUserHandler)
        {
            _botStateTreeHandler = botStateTreeHandler;
            _botViewHandler = botViewHandler;
            _sender = sender;
            _botStateTreeUserHandler = botStateTreeUserHandler;

            _offerResetCountRepeatSentences =
                Convert.ToInt32(configuration.GetSection("UserSettings")["OfferResetCountRepeatSentences"]);

            Initialize();
        }

        public async Task Exec(UpdateBDto update)
        {
            await _botStateTreeHandler.ExecuteRoute(update);
        }

        private void Initialize()
        {
            _botStateTreeHandler.AddAction(BaseField.BaseAction, BaseAction);
            _botStateTreeHandler.AddKeyboard(BaseField.BaseAction, BaseField.NewSentenceButton,
                NewSentenceButton);
            _botStateTreeHandler.AddKeyboard(BaseField.BaseAction, BaseField.RepeatSentenceKeyboard,
                RepeatSentenceKeyboard);
            _botStateTreeHandler.AddCallback(BaseField.BaseAction,
                BaseField.ResetCountRepeatSentencesCallback,
                ResetCountRepeatSentencesCallback);
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

        //Отправляет новую пару предложений для изучения
        private async Task NewSentenceButton(UpdateBDto update)
        {
            var newSentenceDto = await _sender.Send(new GetNewSentenceQuery() { UserId = update.GetUserId() });

            var viewDto = new NewSentenceVDto() { Update = update, NewSentence = newSentenceDto };
            await _botViewHandler.SendAsync(BaseViewField.NewSentence, viewDto);

            await _sender.Send(new LearnNewSentenceCommand()
            {
                UserId = update.GetUserId(), SentencePairId = newSentenceDto.SentencePairId
            });
        }

        //Отправляет уже изученую пару предложений для повторения
        private async Task RepeatSentenceKeyboard(UpdateBDto updateBDto)
        {
            var countRepetitionSentences = await _sender.Send(new GetCountRepetitionSentencesQuery()
            {
                UserId = updateBDto.GetUserId()
            });

            //Отображалось ли сообщение о сбросе повтора в начало
            ResetCountRepeatSentencesSDto? stateDto =
                await _botStateTreeUserHandler.GetDataAsync<ResetCountRepeatSentencesSDto>(updateBDto);
            bool isShowResetCountRepeatSentences = stateDto != null ? stateDto.IsShow : false;

            if (countRepetitionSentences.Count != null &&
                countRepetitionSentences.Count % _offerResetCountRepeatSentences == 0 &&
                !isShowResetCountRepeatSentences)
            {
                var viewDto = new ResetCountRepeatSentenceVDto()
                {
                    Update = updateBDto, Count = countRepetitionSentences.Count!.Value
                };
                await _botViewHandler.SendAsync(BaseViewField.ResetCountRepeatSentence, viewDto);

                //Записать информацию что сообщение о сбросе повтора отображалось пользователю
                stateDto = new ResetCountRepeatSentencesSDto() { IsShow = true };
                await _botStateTreeUserHandler.SetDataAndActionAsync(updateBDto, updateBDto.TelegramState!.Action,
                    stateDto);
            }
            else
            {
                //Очистка промежуточных данных пользователя
                if (stateDto != null)
                {
                    await _botStateTreeUserHandler.ClearDataAsync(updateBDto);
                }

                try
                {
                    var repeatSentenceDto = await _sender.Send(new GetSentenceForRepeatQuery()
                    {
                        UserId = updateBDto.GetUserId()
                    });

                    var viewDto = new RepeatSentenceVDto()
                    {
                        Update = updateBDto,
                        HideSentenceEnum = repeatSentenceDto.HideSentenceEnum,
                        FirstSentence = repeatSentenceDto.FirstSentence,
                        SecondSentence = repeatSentenceDto.SecondSentence
                    };
                    await _botViewHandler.SendAsync(BaseViewField.RepeatSentence, viewDto);

                    await _sender.Send(new RepeatSentenceCommand()
                    {
                        UserId = updateBDto.GetUserId(),
                        IsLearn = false,
                        UsingSentencesPairId = repeatSentenceDto.UsingSentencesPairId
                    });
                }
                catch (OutOfSentencesToRepeatException)
                {
                    await _botViewHandler.SendAsync(BaseViewField.ResetOutOfSentencesToRepeat, updateBDto);
                }
                catch (EmptyCollectionOfSentencesToRepeatException)
                {
                    await _botViewHandler.SendAsync(BaseViewField.EmptyCollectionOfSentencesToRepeat, updateBDto);
                }
            }
        }

        //Сбрасывает счетчик повторения слов в начало
        //Для того чтобы пользователь начал повторение слов с начала
        private async Task ResetCountRepeatSentencesCallback(UpdateBDto updateBDto)
        {
            await _sender.Send(new ResetCountRepeatSentenceCommand() { UserId = updateBDto.GetUserId() });
            await _botViewHandler.SendAsync(BaseViewField.ConfirmResetCountRepeatSentence, updateBDto);
        }
    }
}
