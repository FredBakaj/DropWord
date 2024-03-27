using DropWord.Application.UseCase.Sentence.Commands.SentencesRepeatForDay;
using DropWord.Application.UseCase.Sentence.Queries.GetDiffSentenceWithMarkup;
using DropWord.Domain.Enums;
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

public class RepeatForDayController : IBotController
{
    private readonly IBotStateTreeHandler _botStateTreeHandler;
    private readonly IBotViewHandler _botViewHandler;
    private readonly ISender _sender;
    private readonly IBotStateTreeUserHandler _botStateTreeUserHandler;
    private readonly IRepeatSentenceManager _repeatSentenceManager;
    public string Name() => RepeatForDayField.RepeatForDayState;

    public RepeatForDayController(IBotStateTreeHandler botStateTreeHandler,
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
        _botStateTreeHandler.AddKeyboard(RepeatForDayField.CheckInputRepeatForDayAction, RepeatForDayField.CancelRepeatForDayKeyboard,
            CancelKeyboard);

        _botStateTreeHandler.AddAction(RepeatForDayField.CheckInputRepeatForDayAction, CheckInputAction);
    }


    private async Task CancelKeyboard(UpdateBDto updateBDto)
    {
        await _botStateTreeUserHandler.SetStateAndActionAsync(updateBDto, BaseField.BaseState, BaseField.BaseAction);
        await _botStateTreeUserHandler.ClearDataAsync(updateBDto);
        await _botViewHandler.SendAsync(BaseViewField.Menu, updateBDto);
    }

    private async Task CheckInputAction(UpdateBDto updateBDto)
    {
        var messageText = updateBDto.GetMessage().Text!;
        var dataModel = await _botStateTreeUserHandler.GetDataAsync<SentencesRepetitionByInputSDto>(updateBDto);
        if (dataModel != null)
        {
            var responseRepeat = await _sender.Send(new SentencesRepeatForDayCommand()
            {
                Sentence = messageText,
                UserId = updateBDto.GetUserId(),
                UsingSentencesPairId = dataModel!.UsingSentencesPairId,
                SentenceToLearnLabelEnum = dataModel.SentenceToLearnLabelEnum
            });

            if (responseRepeat == StatusOfSentenceInputEnum.Right)
            {
                await ProcessRightInputAsync(updateBDto);
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

        await _botStateTreeUserHandler.SetStateAndActionAsync(updateBDto, BaseField.BaseState, BaseField.BaseAction);
        await _botStateTreeUserHandler.ClearDataAsync(updateBDto);
    }

    private async Task ProcessRightInputAsync(UpdateBDto updateBDto)
    {
        //Отправка сообщения с информацие что ввод правельный и следующим словом
        await _botViewHandler.SendAsync(RepeatForDayViewField.RightInputRepeatForDay, updateBDto);
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

        //Отправка сообщения с информацие что ввод содержит ошибки 
        var viewModel = new RepeatForDayInputWithErrorsVDto()
        {
            Update = updateBDto,
            CorrectedSentence = diffSentenceWithMarkup.Sentence,
            RightSentence = originalSentence
        };
        await _botViewHandler.SendAsync(RepeatForDayViewField.InputWithErrorsRepeatForDay, viewModel);
    }

    private async Task ProcessIncorrectInputAsync(UpdateBDto updateBDto, SentencesRepetitionByInputSDto data)
    {
        var originalSentence = _repeatSentenceManager.GetOriginalSentence(data);
        
        //Отправка сообщения с информацие что ввод содержит ошибки 
        var viewModel = new RepeatForDayIncorrectInputVDto()
        {
            Update = updateBDto, RightSentence = originalSentence
        };
        await _botViewHandler.SendAsync(RepeatForDayViewField.IncorrectInputRepeatForDay, viewModel);
    }
}
