using DropWord.Application.UseCase.Sentence.Commands.SentenceRepeatForDayCard;
using DropWord.Application.UseCase.Sentence.Queries.GetSentenceForRepeat;
using DropWord.Application.UseCase.User.Queries.GetUser;
using DropWord.Domain.Enums;
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
/// <summary>
/// Controller for handling sentence-related operations, specifically for repeating sentences.
/// </summary>
public class SentenceController : ControllerBase
{
    private readonly IBotStateTreeUserHandler _botStateTreeUserHandler;
    private readonly ISender _sender;
    private readonly IBotViewHandler _botViewHandler;
    private readonly IRepeatSentenceManager _repeatSentenceManager;

    /// <summary>
    /// Constructor for the SentenceController. Initializes the dependencies.
    /// </summary>
    /// <param name="botStateTreeUserHandler">Handles the state tree for users.</param>
    /// <param name="sender">Sends queries to the application layer.</param>
    /// <param name="botViewHandler">Handles the views for the bot.</param>
    /// <param name="repeatSentenceManager">Manages the repetition of sentences.</param>
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

    /// <summary>
    /// Handles the request to repeat a sentence for a day.
    /// </summary>
    /// <param name="model">Contains the necessary information for repeating a sentence.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    [HttpPost("[action]")]
    public async Task RepeatForDay([FromBody] RepeatForDayApiModel model)
    {
        // Get the user data from the sender.
        UserDto user = await _sender.Send(new GetUserQuery() { UserId = model.UserId });

        // Check the user's settings for the mode of repeating sentences.
        if (user.UserSettings.SentencesRepeatForDayModeEnum == SentencesRepeatForDayModeEnum.Card)
        {
            // If the mode is card, call the method to handle repeating sentences in card mode.
            await RepeatForDayCardMode(model, user);
        }
        else if (user.UserSettings.SentencesRepeatForDayModeEnum == SentencesRepeatForDayModeEnum.Write)
        {
            // If the mode is write, call the method to handle repeating sentences in write mode.
            await RepeatForDayInputMode(model, user);
        }

        // Return a NoContent result.
        NoContent();
    }

    /// <summary>
    /// Handles the request to repeat a sentence for a day in write mode.
    /// </summary>
    /// <param name="model">Contains the necessary information for repeating a sentence.</param>
    /// <param name="user">Contains the user data.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task RepeatForDayInputMode(RepeatForDayApiModel model, UserDto user)
    {
        // Create a new SentenceForRepeatDto object with the necessary information.
        SentenceForRepeatDto nextSentenceDto = new()
        {
            UsingSentencesPairId = model.SentenceForRepeatApi.UsingSentencesPairId,
            FirstSentence = model.SentenceForRepeatApi.FirstSentence,
            SecondSentence = model.SentenceForRepeatApi.SecondSentence,
            SentenceToLearnLabel = model.SentenceForRepeatApi.SentenceToLearnLabel
        };

        // Get the next sentence using the repeat sentence manager.
        string sentence = _repeatSentenceManager.GetNextSentence(nextSentenceDto);

        // Create a new SentencesRepetitionByInputSDto object with the necessary information.
        SentencesRepetitionByInputSDto stateDto = new()
        {
            UsingSentencesPairId = model.SentenceForRepeatApi.UsingSentencesPairId,
            FirstSentence = model.SentenceForRepeatApi.FirstSentence,
            SecondSentence = model.SentenceForRepeatApi.SecondSentence,
            SentenceToLearnLabelEnum = model.SentenceForRepeatApi.SentenceToLearnLabel
        };

        // Create a new RepeatForDayStartInputVDto object with the necessary information.
        RepeatForDayStartInputVDto viewModel = new()
        {
            UserId = model.UserId,
            Sentence = sentence,
            FirstLangEmoji = CustomConvert.LanguageToEmoji(user.UserSettings.MainLanguage),
            SecondLangEmoji = CustomConvert.LanguageToEmoji(user.UserSettings.LearnLanguage),
        };

        // Send the view model to the bot view handler.
        await _botViewHandler.SendAsync(RepeatForDayViewField.StartInputRepeatForDay, viewModel);

        // Set the state and action for the user.
        await _botStateTreeUserHandler.SetStateAndActionAsync(model.UserId, RepeatForDayField.RepeatForDayState,
            RepeatForDayField.CheckInputRepeatForDayAction);

        // Set the data for the user.
        await _botStateTreeUserHandler.SetDataAsync(model.UserId, stateDto);
    }

    /// <summary>
    /// Handles the request to repeat a sentence for a day in card mode.
    /// </summary>
    /// <param name="model">Contains the necessary information for repeating a sentence.</param>
    /// <param name="user">Contains the user data.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task RepeatForDayCardMode(RepeatForDayApiModel model, UserDto user)
    {
        SentenceForRepeatDto nextSentenceDto = new()
        {
            UsingSentencesPairId = model.SentenceForRepeatApi.UsingSentencesPairId,
            FirstSentence = model.SentenceForRepeatApi.FirstSentence,
            SecondSentence = model.SentenceForRepeatApi.SecondSentence,
            SentenceToLearnLabel = model.SentenceForRepeatApi.SentenceToLearnLabel
        };

        // Get the next sentence using the repeat sentence manager.
        string firstSentence = _repeatSentenceManager.GetNextSentence(nextSentenceDto);
        string secondSentence = _repeatSentenceManager.GetOriginalSentence(nextSentenceDto);

        // Create a new RepeatForDayCardVDto object with the necessary information.
        var viewModel = new RepeatForDayCardVDto()
        {
            UserId = model.UserId,
            FirstSentence = firstSentence,
            SecondSentence = secondSentence,
            FirstLangEmoji = CustomConvert.LanguageToEmoji(user.UserSettings.MainLanguage),
            SecondLangEmoji = CustomConvert.LanguageToEmoji(user.UserSettings.LearnLanguage),
        };

        // Send the view model to the bot view handler.
        await _botViewHandler.SendAsync(RepeatForDayViewField.RepeatForDayCard, viewModel);
        await _sender.Send(new SentenceRepeatForDayCardCommand()
        {
            UserId = model.UserId, UsingSentencesPairId = model.SentenceForRepeatApi.UsingSentencesPairId
        });
    }
}
