using DropWord.Application.UseCase.Sentence.Queries.GetSentenceForRepeat;
using DropWord.Application.UseCase.User.Queries.GetUser;
using DropWord.TgBot.Core.Field.Controller;
using DropWord.TgBot.Core.Field.View;
using DropWord.TgBot.Core.Handler.BotStateTreeUserHandler;
using DropWord.TgBot.Core.Handler.BotViewHandler;
using DropWord.TgBot.Core.Manager.RepeatSentence;
using DropWord.TgBot.Core.StateDto;
using DropWord.TgBot.Core.Utils;
using DropWord.TgBot.Core.ViewDto;
using DropWord.TgBot.Model;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DropWord.TgBot.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class SentenceController : ControllerBase
{
    private readonly IBotStateTreeUserHandler _botStateTreeUserHandler;
    private readonly ISender _sender;
    private readonly IBotViewHandler _botViewHandler;
    private readonly IRepeatSentenceManager _repeatSentenceManager;

    public SentenceController(IBotStateTreeUserHandler botStateTreeUserHandler,
        ISender sender,
        IBotViewHandler botViewHandler,
        IRepeatSentenceManager repeatSentenceManager
    )
    {
        _botStateTreeUserHandler = botStateTreeUserHandler;
        _sender = sender;
        _botViewHandler = botViewHandler;
        _repeatSentenceManager = repeatSentenceManager;
    }

    [HttpPost("[action]")]
    public async Task RepeatForDay([FromBody] RepeatForDayApiModel model)
    {
        var user = await _sender.Send(new GetUserQuery() { UserId = model.UserId });

        SentencesRepetitionByInputSDto stateDto = new()
        {
            UsingSentencesPairId = model.SentenceForRepeatApi.UsingSentencesPairId,
            FirstSentence = model.SentenceForRepeatApi.FirstSentence,
            SecondSentence = model.SentenceForRepeatApi.SecondSentence,
            SentenceToLearnLabelEnum = model.SentenceForRepeatApi.SentenceToLearnLabel
        };


        SentenceForRepeatDto nextSentenceDto = new()
        {
            UsingSentencesPairId = model.SentenceForRepeatApi.UsingSentencesPairId,
            FirstSentence = model.SentenceForRepeatApi.FirstSentence,
            SecondSentence = model.SentenceForRepeatApi.SecondSentence,
            SentenceToLearnLabel = model.SentenceForRepeatApi.SentenceToLearnLabel
        };

        var sentence = _repeatSentenceManager.GetNextSentence(nextSentenceDto);


        RepeatForDayStartInputVDto viewModel = new()
        {
            UserId = model.UserId,
            Sentence = sentence,
            FirstLangEmoji = CustomConvert.LanguageToEmoji(user.UserSettings.MainLanguage),
            SecondLangEmoji = CustomConvert.LanguageToEmoji(user.UserSettings.LearnLanguage),
        };

        await _botViewHandler.SendAsync(RepeatForDayViewField.StartInputRepeatForDay, viewModel);
        
        await _botStateTreeUserHandler.SetStateAndActionAsync(model.UserId, RepeatForDayField.RepeatForDayState,
            RepeatForDayField.CheckInputRepeatForDayAction);
        await _botStateTreeUserHandler.SetDataAsync(model.UserId, stateDto);

        NoContent();
    }
}
