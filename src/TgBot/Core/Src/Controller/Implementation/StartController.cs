using DropWord.Application.UseCase.Language.Commands.ChangeLanguagePair;
using DropWord.Domain.Constants;
using DropWord.TgBot.Core.Extension;
using DropWord.TgBot.Core.Field.Controller;
using DropWord.TgBot.Core.Field.View;
using DropWord.TgBot.Core.Handler;
using DropWord.TgBot.Core.Model;
using MediatR;

namespace DropWord.TgBot.Core.Src.Controller.Implementation;

public class StartController : IBotController
{
    private readonly IBotStateTreeHandler _botStateTreeHandler;
    private readonly IBotStateTreeUserHandler _botStateTreeUserHandler;
    private readonly IBotViewHandler _botViewHandler;
    private readonly ISender _sender;
    public string Name() => StartControllerField.StartState;

    public StartController(
        IBotStateTreeHandler botStateTreeHandler,
        IBotStateTreeUserHandler botStateTreeUserHandler,
        IBotViewHandler botViewHandler,
        ISender sender)
    {
        _botStateTreeHandler = botStateTreeHandler;
        _botStateTreeUserHandler = botStateTreeUserHandler;
        _botViewHandler = botViewHandler;
        _sender = sender;

        Initialize();
    }

    private void Initialize()
    {
        _botStateTreeHandler.AddAction(StartControllerField.StartAction, StartActionAsync);

        _botStateTreeHandler.AddKeyboard(StartControllerField.SelectLanguageAction,
            StartControllerField.UkrainianEnglishLanguageButton, UkrainianEnglishLanguageButtonAsync);
        _botStateTreeHandler.AddKeyboard(StartControllerField.SelectLanguageAction,
            StartControllerField.UkrainianGermanLanguageButton, UkrainianGermanLanguageButtonAsync);
        _botStateTreeHandler.AddKeyboard(StartControllerField.SelectLanguageAction,
            StartControllerField.UkrainianPolishLanguageButton, UkrainianPolishLanguageButtonAsync);
        _botStateTreeHandler.AddKeyboard(StartControllerField.SelectLanguageAction,
            StartControllerField.UkrainianFrenchLanguageButton, UkrainianFrenchLanguageButtonAsync);
    }

    public async Task Exec(UpdateBDto update)
    {
        await _botStateTreeHandler.ExecuteRoute(update);
    }

    private async Task StartActionAsync(UpdateBDto update)
    {
        await _botViewHandler.SendAsync(StartViewField.Start, update);
        await _botStateTreeUserHandler.SetActionAsync(update, StartControllerField.SelectLanguageAction);
    }


    private async Task UkrainianEnglishLanguageButtonAsync(UpdateBDto update)
    {
        var secondLanguage = LanguageConst.English;
        await ChangeLanguagePair(update, secondLanguage);
    }

    private async Task UkrainianGermanLanguageButtonAsync(UpdateBDto update)
    {
        var secondLanguage = LanguageConst.German;
        await ChangeLanguagePair(update, secondLanguage);
    }

    private async Task UkrainianPolishLanguageButtonAsync(UpdateBDto update)
    {
        var secondLanguage = LanguageConst.Polish;
        await ChangeLanguagePair(update, secondLanguage);
    }

    private async Task UkrainianFrenchLanguageButtonAsync(UpdateBDto update)
    {
        var secondLanguage = LanguageConst.French;
        await ChangeLanguagePair(update, secondLanguage);
    }

    private async Task ChangeLanguagePair(UpdateBDto update, string secondLanguage)
    {
        var firstLanguage = LanguageConst.Ukrainian;
        await _sender.Send(new ChangeLanguagePairCommand()
        {
            UserId = update.GetUserId(), FirstLanguage = firstLanguage, SecondLanguage = secondLanguage
        });

        await _botStateTreeUserHandler.SetStateAndActionAsync(update, BaseControllerField.BaseState,
            BaseControllerField.BaseAction);
        await _botViewHandler.SendAsync(BaseViewField.Intro, update);
    }
}
