﻿using AutoMapper;
using DropWord.Application.UseCase.Sentence.Commands.AddSentence;
using DropWord.Application.UseCase.Sentence.Commands.DeleteAddedSentence;
using DropWord.Application.UseCase.Sentence.Commands.LearnNewSentence;
using DropWord.Application.UseCase.Sentence.Commands.ParseSentences;
using DropWord.Application.UseCase.Sentence.Commands.RepeatSentence;
using DropWord.Application.UseCase.Sentence.Commands.ResetCountRepeatSentence;
using DropWord.Application.UseCase.Sentence.Commands.UpdateSentence;
using DropWord.Application.UseCase.Sentence.Queries.GetNewSentence;
using DropWord.Application.UseCase.Sentence.Queries.GetSentenceForRepeat;
using DropWord.Application.UseCase.Sentence.Queries.GetSentencesPair;
using DropWord.Application.UseCase.SentencesCollection.Commands.AddCollection;
using DropWord.Application.UseCase.SentencesCollection.Commands.DeleteAddedSentenceCollection;
using DropWord.Domain.Exceptions;
using DropWord.TgBot.Core.Extension;
using DropWord.TgBot.Core.Field.Controller;
using DropWord.TgBot.Core.Field.View;
using DropWord.TgBot.Core.Handler;
using DropWord.TgBot.Core.Manager.RepeatSentence;
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
        private readonly IRepeatSentenceManager _repeatSentenceManager;
        private readonly IMapper _mapper;

        public string Name() => BaseField.BaseState;

        public BaseController(IBotStateTreeHandler botStateTreeHandler, IBotViewHandler botViewHandler, ISender sender,
            IBotStateTreeUserHandler botStateTreeUserHandler,
            IRepeatSentenceManager repeatSentenceManager, IMapper mapper)
        {
            _botStateTreeHandler = botStateTreeHandler;
            _botViewHandler = botViewHandler;
            _sender = sender;
            _botStateTreeUserHandler = botStateTreeUserHandler;
            _repeatSentenceManager = repeatSentenceManager;
            _mapper = mapper;


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
            _botStateTreeHandler.AddKeyboard(BaseField.BaseAction, BaseField.SentencesRepetitionByInputKeyboard,
                SentencesRepetitionByInputKeyboard);
            _botStateTreeHandler.AddCallback(BaseField.BaseAction, BaseField.EditSingleAddedSentenceCallback,
                StartEditAddedSentenceCallbackAsync);
            _botStateTreeHandler.AddCallback(BaseField.BaseAction, BaseField.CancelEditSingleAddedSentenceCallback,
                CancelEditAddedSentenceCallbackAsync);
            _botStateTreeHandler.AddCallback(BaseField.BaseAction, BaseField.SelectEditAddedSentenceLanguageCallback,
                SelectEditAddedSentenceLanguageCallbackAsync);
            _botStateTreeHandler.AddCallback(BaseField.BaseAction, BaseField.DeleteSingleAddedSentenceCallback,
                DeleteAddedSentenceCallbackAsync);
            _botStateTreeHandler.AddCallback(BaseField.BaseAction, BaseField.DeleteAddedSentencesCallback,
                DeleteAddedSentencesCallbackAsync);

            _botStateTreeHandler.AddAction(BaseField.InputEditSentenceAction, InputEditAddedSentenceActionAsync);
            _botStateTreeHandler.AddAction(BaseField.ReloadAction, ReloadAction);
        }

        //Добавление предложений в базу 
        private async Task BaseAction(UpdateBDto update)
        {
            var sentenceParse = await _sender.Send(new ParseSentencesCommand() { Content = update.GetMessage().Text! });

            if (sentenceParse.Sentences.Count() == 1)
            {
                var addedSentence = await _sender.Send(new AddSentenceCommand()
                {
                    UserId = update.GetUserId(), Sentence = sentenceParse.Sentences.First()
                });
                var viewDto = _mapper.Map<AddedSentenceVDto>(addedSentence);
                viewDto.Update = update;
                await _botViewHandler.SendAsync(BaseViewField.AddSentence, viewDto);
            }
            else if (sentenceParse.Sentences.Count() > 1)
            {
                var addedSentences = await _sender.Send(new AddCollectionCommand()
                {
                    UserId = update.GetUserId(), Sentences = sentenceParse.Sentences, Description = "Text"
                });
                var viewDto = _mapper.Map<AddCollectionSentencesVDto>(addedSentences);
                viewDto.Update = update;
                await _botViewHandler.SendAsync(BaseViewField.AddSentences, viewDto);
            }
        }

        private async Task ReloadAction(UpdateBDto updateBDto)
        {
            await _botStateTreeUserHandler.SetStateAndActionAsync(updateBDto, BaseField.BaseState,
                BaseField.BaseAction);
            await _botViewHandler.SendAsync(BaseViewField.Menu, updateBDto);
        }

        private async Task StartEditAddedSentenceCallbackAsync(UpdateBDto updateBDto)
        {
            int sentencesPairId = Convert.ToInt32(updateBDto.CallbackData);
            var sentencesPair = await _sender.Send(new GetSentencesPairQuery()
            {
                UserId = updateBDto.GetUserId(), SentencesPairId = sentencesPairId
            });
            var viewModel = _mapper.Map<EditSentenceVDto>(sentencesPair);
            viewModel.Update = updateBDto;

            await _botViewHandler.SendAsync(BaseViewField.EditSentence, viewModel);
        }

        private async Task CancelEditAddedSentenceCallbackAsync(UpdateBDto updateBDto)
        {
            int sentencesPairId = Convert.ToInt32(updateBDto.CallbackData);
            var sentencesPair = await _sender.Send(new GetSentencesPairQuery()
            {
                UserId = updateBDto.GetUserId(), SentencesPairId = sentencesPairId
            });
            var viewModel = _mapper.Map<CancelEditAddedSentenceVDto>(sentencesPair);
            viewModel.Update = updateBDto;
            await _botViewHandler.SendAsync(BaseViewField.CancelEditAddedSentence, viewModel);
        }

        private async Task SelectEditAddedSentenceLanguageCallbackAsync(UpdateBDto updateBDto)
        {
            int sentenceId = Convert.ToInt32(updateBDto.CallbackData);
            var data = new EditSentenceSDto() { SentenceId = sentenceId };
            await _botStateTreeUserHandler.SetDataAndActionAsync(updateBDto, BaseField.InputEditSentenceAction, data);
            await _botViewHandler.SendAsync(BaseViewField.InputEditSentence, updateBDto);
        }

        private async Task InputEditAddedSentenceActionAsync(UpdateBDto updateBDto)
        {
            int sentenceId = ((await _botStateTreeUserHandler.GetDataAsync<EditSentenceSDto>(updateBDto))!).SentenceId;
            string updateSentenceText = updateBDto.GetMessage().Text!;
            long userId = updateBDto.GetUserId();

            //Возвращаем пользователя в базовое состояние
            await _botStateTreeUserHandler.SetActionAsync(updateBDto, BaseField.BaseAction);
            await _botStateTreeUserHandler.ClearDataAsync(updateBDto);

            // Обновление предложения
            var sentencesPair = await _sender.Send(new UpdateSentenceCommand()
            {
                UserId = userId, SentenceId = sentenceId, Sentence = updateSentenceText
            });

            //Отображаем вюшку обнавленой пары предложений
            var viewModel = _mapper.Map<AddedSentenceVDto>(sentencesPair);
            viewModel.Update = updateBDto;
            await _botViewHandler.SendAsync(BaseViewField.AddSentence, viewModel);
        }

        private async Task DeleteAddedSentenceCallbackAsync(UpdateBDto updateBDto)
        {
            int sentencesPairId = Convert.ToInt32(updateBDto.CallbackData);

            await _sender.Send(new DeleteAddedSentenceCommand()
            {
                UserId = updateBDto.GetUserId(), SentencesPairId = sentencesPairId
            });
            await _botViewHandler.SendAsync(BaseViewField.DeleteAddedSentence, updateBDto);
        }

        private async Task DeleteAddedSentencesCallbackAsync(UpdateBDto updateBDto)
        {
            int sentencesCollectionId = Convert.ToInt32(updateBDto.CallbackData);

            await _sender.Send(new DeleteAddedSentenceCollectionCommand()
            {
                UserId = updateBDto.GetUserId(), CollectionId = sentencesCollectionId
            });
            await _botViewHandler.SendAsync(BaseViewField.DeleteAddedSentenceCollection, updateBDto);
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
            //Оборачиваем отображения пары слов в проверку на кол-во отображонных слов 
            if (await _repeatSentenceManager.CanShowResetCountRepeatSentences(updateBDto))
            {
                await RepeatSentenceAsync(updateBDto);
                await _repeatSentenceManager.ClearShowResetCountRepeatSentencesView(updateBDto);
            }
            else
            {
                var countRepetitionSentences =
                    await _repeatSentenceManager.GetCountRepetitionSentences(updateBDto.GetUserId());
                var viewDto = new ResetCountRepeatSentenceVDto()
                {
                    Update = updateBDto, Count = countRepetitionSentences!.Value
                };
                await _botViewHandler.SendAsync(BaseViewField.ResetCountRepeatSentence, viewDto);
                await _repeatSentenceManager.SaveShowResetCountRepeatSentencesView(updateBDto);
            }
        }

        private async Task RepeatSentenceAsync(UpdateBDto updateBDto)
        {
            try
            {
                var repeatSentenceDto = await _sender.Send(new GetSentenceForRepeatQuery()
                {
                    UserId = updateBDto.GetUserId()
                });

                var viewDto = new RepeatSentenceVDto()
                {
                    Update = updateBDto,
                    SentenceToLearnLabel = repeatSentenceDto.SentenceToLearnLabel,
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

        private async Task SentencesRepetitionByInputKeyboard(UpdateBDto updateBDto)
        {
            //Оборачиваем отображения пары слов в проверку на кол-во отображонных слов 
            if (await _repeatSentenceManager.CanShowResetCountRepeatSentences(updateBDto))
            {
                await StartSentencesRepetitionByInputAsync(updateBDto);
                await _repeatSentenceManager.ClearShowResetCountRepeatSentencesView(updateBDto);
            }
            else
            {
                var countRepetitionSentences =
                    await _repeatSentenceManager.GetCountRepetitionSentences(updateBDto.GetUserId());
                var viewDto = new ResetCountRepeatSentenceVDto()
                {
                    Update = updateBDto, Count = countRepetitionSentences!.Value
                };
                await _botViewHandler.SendAsync(BaseViewField.ResetCountRepeatSentence, viewDto);

                await _repeatSentenceManager.SaveShowResetCountRepeatSentencesView(updateBDto);
            }
        }

        private async Task StartSentencesRepetitionByInputAsync(UpdateBDto updateBDto)
        {
            try
            {
                await _botStateTreeUserHandler.SetStateAndActionAsync(updateBDto,
                    SentencesRepetitionByInputField.State,
                    SentencesRepetitionByInputField.Action);
                //Сохроняет промежуточные данны для сравнения слов между вводами
                var nextSentencePair = await _repeatSentenceManager.GetSentencesPairAndSaveInDataAsync(updateBDto);
                var nextSentence = _repeatSentenceManager.GetNextSentence(nextSentencePair);

                //Отправляет сообщение начало повтора вводом 
                var viewModel =
                    new StartInputVDto() { Update = updateBDto, Sentence = nextSentence };
                await _botViewHandler.SendAsync(SentencesRepetitionByInputViewField.StartInput, viewModel);
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

        //Сбрасывает счетчик повторения слов в начало
        //Для того чтобы пользователь начал повторение слов с начала
        private async Task ResetCountRepeatSentencesCallback(UpdateBDto updateBDto)
        {
            await _sender.Send(new ResetCountRepeatSentenceCommand() { UserId = updateBDto.GetUserId() });
            await _botViewHandler.SendAsync(BaseViewField.ConfirmResetCountRepeatSentence, updateBDto);
        }
    }
}
