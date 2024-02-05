using DropWord.Application.UseCase.Sentence.Commands.ResetCountRepeatSentence;
using DropWord.Application.UseCase.Sentence.Commands.SentencesRepetitionByInput;
using DropWord.Application.UseCase.Sentence.Queries.GetDiffSentenceWithMarkup;
using DropWord.Domain.Enums;
using DropWord.Domain.Exceptions;
using DropWord.TgBot.Core.Enum;
using DropWord.TgBot.Core.Extension;
using DropWord.TgBot.Core.Field.Controller;
using DropWord.TgBot.Core.Field.View;
using DropWord.TgBot.Core.Handler;
using DropWord.TgBot.Core.Manager.RepeatSentence;
using DropWord.TgBot.Core.Model;
using DropWord.TgBot.Core.StateDto;
using DropWord.TgBot.Core.ViewDto;
using MediatR;

namespace DropWord.TgBot.Core.Src.Controller.Implementation;

public class SentencesRepetitionByInputController : IBotController
{
    private readonly IBotStateTreeHandler _botStateTreeHandler;
    private readonly IBotViewHandler _botViewHandler;
    private readonly ISender _sender;

    private readonly IBotStateTreeUserHandler _botStateTreeUserHandler;
    private readonly IRepeatSentenceManager _repeatSentenceManager;

    // private static int item = 0;
    public string Name() => SentencesRepetitionByInputField.State;

    public SentencesRepetitionByInputController(
        IBotStateTreeHandler botStateTreeHandler,
        IBotViewHandler botViewHandler,
        ISender sender,
        IBotStateTreeUserHandler botStateTreeUserHandler,
        IRepeatSentenceManager repeatSentenceManager)
    {
        _botStateTreeHandler = botStateTreeHandler;
        _botViewHandler = botViewHandler;
        _sender = sender;
        _botStateTreeUserHandler = botStateTreeUserHandler;
        _repeatSentenceManager = repeatSentenceManager;

        Initialize();
    }

    public async Task Exec(UpdateBDto update)
    {
        await _botStateTreeHandler.ExecuteRoute(update);
    }

    private void Initialize()
    {
        _botStateTreeHandler.AddAction(SentencesRepetitionByInputField.Action, NextSentenceActionAsync);
        _botStateTreeHandler.AddKeyboard(SentencesRepetitionByInputField.Action,
            SentencesRepetitionByInputField.BackKeyboard, BackKeyboardAsync);
        _botStateTreeHandler.AddCallback(SentencesRepetitionByInputField.Action,
            SentencesRepetitionByInputField.ResetCountRepeatSentencesCallback,
            ResetCountRepeatSentencesCallback);
    }

    private async Task ResetCountRepeatSentencesCallback(UpdateBDto updateBDto)
    {
        await _sender.Send(new ResetCountRepeatSentenceCommand() { UserId = updateBDto.GetUserId() });
        var repeatSentenceDto = await _repeatSentenceManager.GetSentencesPairAndSaveInDataAsync(updateBDto);
        //Отправляет сообщение начало повтора вводом 
        var viewModel =
            new StartInputVDto() { Update = updateBDto, Sentence = _repeatSentenceManager.GetNextSentence(repeatSentenceDto) };
        await _botViewHandler.SendAsync(SentencesRepetitionByInputViewField.StartInput, viewModel);
    }

    private async Task NextSentenceActionAsync(UpdateBDto updateBDto)
    {
        //TODO Добавить проверку на кол-во вводов, добавить вюшку для сброса слов.
        var messageText = updateBDto.GetMessage().Text!;
        var dataModel = await _botStateTreeUserHandler.GetDataAsync<SentencesRepetitionByInputSDto>(updateBDto);
        //TODO Добавить проверку на нулл dataModel если он пустой то код ниже не запускать, отправлять сообщение с кнопкой 
        // получить слово, и сетать в дату модель с инфой о том, что пользователь уже пытался получить слово, чтобы 
        // если все слова закончились не переключаться между сообщениями Получить слово и все слова закончились
        if (dataModel != null)
        {
            var responseRepeat = await _sender.Send(new SentencesRepetitionByInputCommand()
            {
                Sentence = messageText,
                UserId = updateBDto.GetUserId(),
                UsingSentencesPairId = dataModel!.UsingSentencesPairId,
                SentenceToLearnLabelEnum = dataModel.SentenceToLearnLabelEnum
            });
            try
            {
                if (responseRepeat == StatusOfSentenceInputEnum.Right)
                {
                    await ProcessRightInputAsync(updateBDto, dataModel);
                }
                else if (responseRepeat == StatusOfSentenceInputEnum.InputWithErrors)
                {
                    await ProcessInputWithErrorsAsync(updateBDto, dataModel, messageText);
                }
                else if (responseRepeat == StatusOfSentenceInputEnum.Incorrect)
                {
                    await ProcessIncorrectInputAsync(updateBDto, dataModel);
                }
            }
            catch (OutOfSentencesToRepeatException)
            {
                await _botStateTreeUserHandler.ClearDataAsync(updateBDto);
            }
        }
        else
        {
            var tempDataModel = await _botStateTreeUserHandler.GetDataAsync<TempSDto>(updateBDto);
            if (tempDataModel != null && tempDataModel.TempData == TempStateUserEnum.EmptyDataForRepeatSentencesByInput)
            {
                //todo Добавить ResetOutOfSentencesToRepeat во вю SentencesRepetitionByInputViewField 
                await _botViewHandler.SendAsync(BaseViewField.ResetOutOfSentencesToRepeat, updateBDto);
            }
            else
            {
                try
                {
                    var nextSentencePair = await _repeatSentenceManager.GetSentencesPairAndSaveInDataAsync(updateBDto);
                    var viewModel = new StartInputVDto()
                    {
                        Update = updateBDto, Sentence = _repeatSentenceManager.GetNextSentence(nextSentencePair),
                    };
                    await _botViewHandler.SendAsync(SentencesRepetitionByInputViewField.StartInput, viewModel);
                }
                catch (OutOfSentencesToRepeatException)
                {
                    tempDataModel = new TempSDto() { TempData = TempStateUserEnum.EmptyDataForRepeatSentencesByInput };
                    await _botStateTreeUserHandler.SetDataAndActionAsync(updateBDto, updateBDto.TelegramState!.Action,
                        tempDataModel);
                    //TODO добавить вю ResetOutOfSentencesToRepeat в SentencesRepetitionByInputViewField
                    await _botViewHandler.SendAsync(BaseViewField.ResetOutOfSentencesToRepeat, updateBDto);
                }
            }
        }
    }

    

    private async Task ProcessRightInputAsync(UpdateBDto updateBDto, SentencesRepetitionByInputSDto data)
    {
        try
        {
            var nextSentencePair = await _repeatSentenceManager.GetSentencesPairAndSaveInDataAsync(updateBDto);
            var nextSentence = _repeatSentenceManager.GetNextSentence(nextSentencePair);
            //Отправка сообщения с информацие что ввод правельный и следующим словом
            var viewModel = new RightInputVDto() { Update = updateBDto, NextSentence = nextSentence };
            await _botViewHandler.SendAsync(SentencesRepetitionByInputViewField.RightInput, viewModel);
        }
        catch (OutOfSentencesToRepeatException)
        {
            await _botViewHandler.SendAsync(SentencesRepetitionByInputViewField.RightInputAndOutOfSentencesToRepeat,
                updateBDto);
            throw;
        }
    }

    private async Task ProcessInputWithErrorsAsync(UpdateBDto updateBDto, SentencesRepetitionByInputSDto data,
        string messageText)
    {
        var originalSentence = _repeatSentenceManager.GetOriginalSentence(data);
        var diffSentenceWithMarkup = await
            _sender.Send(new GetDiffSentenceWithMarkupQuery()
            {
                OldSentence = originalSentence, NewSentence = messageText
            });
        try
        {
            var nextSentencePair = await _repeatSentenceManager.GetSentencesPairAndSaveInDataAsync(updateBDto);
            var nextSentence = _repeatSentenceManager.GetNextSentence(nextSentencePair);
            //Отправка сообщения с информацие что ввод содержит ошибки 
            var viewModel = new InputWithErrorsVDto()
            {
                Update = updateBDto,
                NextSentence = nextSentence,
                CorrectedSentence = diffSentenceWithMarkup.Sentence, //TODO заменить на другое слово
                RightSentence = originalSentence
            };
            await _botViewHandler.SendAsync(SentencesRepetitionByInputViewField.InputWithErrors, viewModel);
        }
        catch (OutOfSentencesToRepeatException)
        {
            var viewModel = new InputWithErrorsAndOutOfSentencesToRepeatVDto()
            {
                Update = updateBDto,
                RightSentence = originalSentence,
                CorrectedSentence = diffSentenceWithMarkup.Sentence
            };
            await _botViewHandler.SendAsync(
                SentencesRepetitionByInputViewField.InputWithErrorsAndOutOfSentencesToRepeat, viewModel);
            throw;
        }
    }

    private async Task ProcessIncorrectInputAsync(UpdateBDto updateBDto, SentencesRepetitionByInputSDto data)
    {
        var originalSentence = _repeatSentenceManager.GetOriginalSentence(data);
        try
        {
            var nextSentencePair = await _repeatSentenceManager.GetSentencesPairAndSaveInDataAsync(updateBDto);
            var nextSentence = _repeatSentenceManager.GetNextSentence(nextSentencePair);
            //Отправка сообщения с информацие что ввод содержит ошибки 
            var viewModel = new IncorrectInputVDto()
            {
                Update = updateBDto, NextSentence = nextSentence, RightSentence = originalSentence
            };
            await _botViewHandler.SendAsync(SentencesRepetitionByInputViewField.IncorrectInput, viewModel);
        }
        catch (OutOfSentencesToRepeatException)
        {
            var viewModel = new IncorrectInputAndOutOfSentencesToRepeatVDto()
            {
                Update = updateBDto, RightSentence = originalSentence
            };

            await _botViewHandler.SendAsync(SentencesRepetitionByInputViewField.IncorrectInputAndOutOfSentencesToRepeat,
                viewModel);
            throw;
        }
    }

    private async Task BackKeyboardAsync(UpdateBDto updateBDto)
    {
        await _botStateTreeUserHandler.SetStateAndActionAsync(updateBDto, BaseField.BaseState, BaseField.BaseAction);
        await _botStateTreeUserHandler.ClearDataAsync(updateBDto);
        await _botViewHandler.SendAsync(BaseViewField.Menu, updateBDto);
    }
}
