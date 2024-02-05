using DropWord.TgBot.Core.Attribute;
using DropWord.TgBot.Core.Extension;
using DropWord.TgBot.Core.Field.Controller;
using DropWord.TgBot.Core.Field.View;
using DropWord.TgBot.Core.Model;
using DropWord.TgBot.Core.ViewDto;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace DropWord.TgBot.Core.Src.View.Implementation;

public class SentencesRepetitionByInputBotView : ABotView
{
    private readonly ITelegramBotClient _botClient;

    public SentencesRepetitionByInputBotView(ITelegramBotClient botClient)
    {
        _botClient = botClient;
    }

    [BotView(SentencesRepetitionByInputViewField.StartInput)]
    public async Task StartInput(StartInputVDto startInputVDto)
    {
        var text = $"*Введіть переклад* ✍️\n\n {startInputVDto.Sentence}";
        var languageChangeModeIcon = SentencesRepetitionByInputField.LanguageChangeModeDynamicKeyboard;

        var replyMarkup = new ReplyKeyboardMarkup(new[]
        {
            new KeyboardButton[] { languageChangeModeIcon, SentencesRepetitionByInputField.BackKeyboard }
        }) { ResizeKeyboard = true };
        await _botClient.SendTextMessageMarkdown2Async(startInputVDto.Update.GetUserId(), text,
            replyMarkup: replyMarkup);
    }

    [BotView(SentencesRepetitionByInputViewField.RightInput)]
    public async Task RightInput(RightInputVDto rightInputVDto)
    {
        var text = $"* Все правельно * 💪 \n\n " +
                   $"*Наступне речення* ✍️\n {rightInputVDto.NextSentence}";
        await _botClient.SendTextMessageMarkdown2Async(rightInputVDto.Update.GetUserId(), text);
    }

    [BotView(SentencesRepetitionByInputViewField.IncorrectInput)]
    public async Task IncorrectInput(IncorrectInputVDto incorrectInputVDto)
    {
        var text = $"*Неправильний переклад* ☹️ \n\n" +
                   $"* Як правельно* 🤔\n {incorrectInputVDto.RightSentence} \n\n" +
                   $" *Наступне речення* ✍️\n ||{incorrectInputVDto.NextSentence}||";
        await _botClient.SendTextMessageMarkdown2Async(incorrectInputVDto.Update.GetUserId(), text);
    }

    [BotView(SentencesRepetitionByInputViewField.InputWithErrors)]
    public async Task InputWithErrors(InputWithErrorsVDto inputWithErrorsVDto)
    {
        var text = $"*Майже вірно* 🤏 \n\n" +
                   $" *Як правельно* 🤔\n {inputWithErrorsVDto.RightSentence} \n\n" +
                   $" *Де були помилки* 👀\n {inputWithErrorsVDto.CorrectedSentence}\n\n " +
                   $"*Наступне речення* ✍️\n ||{inputWithErrorsVDto.NextSentence}||";
        await _botClient.SendTextMessageMarkdown2Async(inputWithErrorsVDto.Update.GetUserId(), text);
    }

    [BotView(SentencesRepetitionByInputViewField.InputSameSentence)]
    public async Task InputSameSentence(InputSameSentenceVDto sameSentenceVDto)
    {
        var text = $"*Вам потрібно було це перевести* 🙃 \n\n" +
                   $"{sameSentenceVDto.TranslationSentence}";
        await _botClient.SendTextMessageMarkdown2Async(sameSentenceVDto.Update.GetUserId(), text);
    }

    [BotView(SentencesRepetitionByInputViewField.RightInputAndOutOfSentencesToRepeat)]
    public async Task RightInputAndOutOfSentencesToRepeat(UpdateBDto updateBDto)
    {
        var text = $"* Все правельно * 💪 \n\n " +
                   $"*Наразі у вас закінчились речення для повтору. Повернутись на початок?* ↩️\n";

        await ResetRepeatSentencesAsync(updateBDto, text);
    }

    [BotView(SentencesRepetitionByInputViewField.RightInputAndResetCountSentence)]
    public async Task RightInputAndResetCountSentence(RightInputAndResetCountSentenceVDto resetCountSentenceVDto)
    {
        var text = $"* Все правельно * 💪 \n\n " +
                   $"*Ви повторили {resetCountSentenceVDto.CountSentence} речень, повернутись на початок?*\n";

        await ResetRepeatSentencesAsync(resetCountSentenceVDto.Update, text);
    }

    [BotView(SentencesRepetitionByInputViewField.InputWithErrorsAndOutOfSentencesToRepeat)]
    public async Task InputWithErrorsAndOutOfSentencesToRepeat(
        InputWithErrorsAndOutOfSentencesToRepeatVDto inputWithErrorsVDto)
    {
        var text = $"*Майже вірно* 🤏 \n\n" +
                   $" *Як правельно* 🤔\n {inputWithErrorsVDto.RightSentence} \n\n" +
                   $" *Де були помилки* 👀\n {inputWithErrorsVDto.CorrectedSentence}\n\n " +
                   $"*Наразі у вас закінчились речення для повтору. Повернутись на початок?* ↩️\n";

        await ResetRepeatSentencesAsync(inputWithErrorsVDto.Update, text);
    }

    [BotView(SentencesRepetitionByInputViewField.InputWithErrorsAndResetCountSentence)]
    public async Task InputWithErrorsAndResetCountSentence(InputWithErrorsAndResetCountSentenceVDto inputWithErrorsVDto)
    {
        var text = $"*Майже вірно* 🤏 \n\n" +
                   $" *Як правельно* 🤔\n {inputWithErrorsVDto.RightSentence} \n\n" +
                   $" *Де були помилки* 👀\n {inputWithErrorsVDto.CorrectedSentence}\n\n " +
                   $"*Наступне речення* ✍️\n ||{inputWithErrorsVDto.NextSentence}||" +
                   $"*Ви повторили {inputWithErrorsVDto.CountSentence} речень, повернутись на початок?*\n";
        await ResetRepeatSentencesAsync(inputWithErrorsVDto.Update, text);
    }

    [BotView(SentencesRepetitionByInputViewField.IncorrectInputAndOutOfSentencesToRepeat)]
    public async Task IncorrectInputAndOutOfSentencesToRepeat(
        IncorrectInputAndOutOfSentencesToRepeatVDto incorrectInputVDto)
    {
        var text = $"*Неправильний переклад* ☹️ \n\n" +
                   $"* Як правельно* 🤔\n {incorrectInputVDto.RightSentence} \n\n" +
                   $"*Наразі у вас закінчились речення для повтору. Повернутись на початок?* ↩️\n";

        await ResetRepeatSentencesAsync(incorrectInputVDto.Update, text);
    }

    [BotView(SentencesRepetitionByInputViewField.IncorrectInputAndResetCountSentence)]
    public async Task IncorrectInputAndResetCountSentence(IncorrectInputAndResetCountSentenceVDto incorrectInputVDto)
    {
        var text = $"*Неправильний переклад* ☹️ \n\n" +
                   $"* Як правельно* 🤔\n {incorrectInputVDto.RightSentence} \n\n" +
                   $" *Наступне речення* ✍️\n ||{incorrectInputVDto.NextSentence}||" +
                   $"*Ви повторили {incorrectInputVDto.CountSentence} речень, повернутись на початок?*\n";

        await _botClient.SendTextMessageMarkdown2Async(incorrectInputVDto.Update.GetUserId(), text);
    }

    private async Task ResetRepeatSentencesAsync(UpdateBDto updateBDto, string text)
    {
        InlineKeyboardMarkup inlineKeyboard = new(new[]
        {
            // first row
            new[]
            {
                InlineKeyboardButton.WithCallbackData(text: "Повернутись",
                    callbackData: SentencesRepetitionByInputField.ResetCountRepeatSentencesCallback),
            }
        });
        await _botClient.SendTextMessageMarkdown2Async(updateBDto.GetUserId(), text, replyMarkup: inlineKeyboard);
    }
}
