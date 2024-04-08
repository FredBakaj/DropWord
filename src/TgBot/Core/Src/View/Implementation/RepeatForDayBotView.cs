using DropWord.TgBot.Core.Attribute;
using DropWord.TgBot.Core.Extension;
using DropWord.TgBot.Core.Field.Controller;
using DropWord.TgBot.Core.Field.View;
using DropWord.TgBot.Core.Model;
using DropWord.TgBot.Core.ViewComponent;
using DropWord.TgBot.Core.ViewDto;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace DropWord.TgBot.Core.Src.View.Implementation;

public class RepeatForDayBotView : ABotView
{
    private readonly ITelegramBotClient _botClient;
    private readonly IMainMenuComponent _mainMenuComponent;

    public RepeatForDayBotView(ITelegramBotClient botClient, IMainMenuComponent mainMenuComponent)
    {
        _botClient = botClient;
        _mainMenuComponent = mainMenuComponent;
    }

    [BotView(RepeatForDayViewField.RepeatForDayCard)]
    public async Task RepeatForDayCard(RepeatForDayCardVDto viewModel)
    {
        var text = $"*Повторення дня* ⏰ {viewModel.FirstLangEmoji}{viewModel.SecondLangEmoji}\n\n" +
                   $"{viewModel.FirstSentence}\n\n" +
                   $"||{viewModel.SecondSentence}||\n";
        
        await _botClient.SendTextMessageMarkdown2Async(viewModel.UserId, text);
    }
    
    [BotView(RepeatForDayViewField.StartInputRepeatForDay)]
    public async Task StartInput(RepeatForDayStartInputVDto viewModel)
    {
        var text = "*Повторення дня* ⏰\n\n" +
                   $"*Введіть переклад* ✍️ {viewModel.FirstLangEmoji}{viewModel.SecondLangEmoji} \n" +
                   $"{viewModel.Sentence}";
        
        var replyMarkup = new ReplyKeyboardMarkup(new[]
        {
            new KeyboardButton[] { RepeatForDayField.CancelRepeatForDayKeyboard}
        }) { ResizeKeyboard = true };

        await _botClient.SendTextMessageMarkdown2Async(viewModel.UserId, text, replyMarkup: replyMarkup);
    }
    
    
    [BotView(RepeatForDayViewField.RightInputRepeatForDay)]
    public async Task RightInput(UpdateBDto rightInputVDto)
    {
        var text = $"* Все правельно * 💪 ";
        await _mainMenuComponent.SendAsync(rightInputVDto, text);
    }

    [BotView(RepeatForDayViewField.IncorrectInputRepeatForDay)]
    public async Task IncorrectInput(RepeatForDayIncorrectInputVDto incorrectInputVDto)
    {
        var text = $"*Неправильний переклад* ☹️ \n\n" +
                   $"* Як правельно* 🤔\n {incorrectInputVDto.RightSentence}";
        await _mainMenuComponent.SendAsync(incorrectInputVDto.Update, text);
    }

    [BotView(RepeatForDayViewField.InputWithErrorsRepeatForDay)]
    public async Task InputWithErrors(RepeatForDayInputWithErrorsVDto inputWithErrorsVDto)
    {
        var text = $"*Майже вірно* 🤏 \n\n" +
                   $" *Як правельно* 🤔\n {inputWithErrorsVDto.RightSentence} \n\n" +
                   $" *Де були помилки* 👀\n {inputWithErrorsVDto.CorrectedSentence}";
        await _mainMenuComponent.SendAsync(inputWithErrorsVDto.Update, text);
    }
}
