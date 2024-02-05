using DropWord.Application.UseCase.Sentence.Queries.GetCountRepetitionSentences;
using DropWord.Application.UseCase.Sentence.Queries.GetSentenceForRepeat;
using DropWord.Domain.Enums;
using DropWord.TgBot.Core.Enum;
using DropWord.TgBot.Core.Extension;
using DropWord.TgBot.Core.Field.Controller;
using DropWord.TgBot.Core.Handler;
using DropWord.TgBot.Core.Model;
using DropWord.TgBot.Core.StateDto;
using DropWord.TgBot.Core.ViewDto;
using MediatR;

namespace DropWord.TgBot.Core.Manager.RepeatSentence.Implementation;

public class RepeatSentenceManager : IRepeatSentenceManager
{
    private readonly ISender _sender;
    private readonly IBotStateTreeHandler _botStateTreeHandler;
    private readonly IBotStateTreeUserHandler _botStateTreeUserHandler;
    private readonly IBotViewHandler _botViewHandler;
    private readonly int _offerResetCountRepeatSentences;

    public RepeatSentenceManager(ISender sender, IBotStateTreeHandler botStateTreeHandler,
        IBotStateTreeUserHandler botStateTreeUserHandler, IBotViewHandler botViewHandler, IConfiguration configuration)
    {
        _sender = sender;
        _botStateTreeHandler = botStateTreeHandler;
        _botStateTreeUserHandler = botStateTreeUserHandler;
        _botViewHandler = botViewHandler;
        _offerResetCountRepeatSentences =
            Convert.ToInt32(configuration.GetSection("UserSettings")["OfferResetCountRepeatSentences"]);
    }

    public async Task<bool> TryShowRepeatResetToStartAsync(UpdateBDto updateBDto, string resetCountFieldView)
    {
        var countRepetitionSentences = await _sender.Send(new GetCountRepetitionSentencesQuery()
        {
            UserId = updateBDto.GetUserId()
        });

        //Отображалось ли сообщение о сбросе повтора в начало
        TempSDto? stateDto =
            await _botStateTreeUserHandler.GetDataAsync<TempSDto>(updateBDto);
        bool isShowResetCountRepeatSentences = stateDto != null ? stateDto.TempData == TempStateUserEnum.ShowResetCountRepeatSentences : false;

        if (countRepetitionSentences.Count != null &&
            countRepetitionSentences.Count % _offerResetCountRepeatSentences == 0 &&
            !isShowResetCountRepeatSentences)
        {
            var viewDto = new ResetCountRepeatSentenceVDto()
            {
                Update = updateBDto, Count = countRepetitionSentences.Count!.Value
            };
            await _botViewHandler.SendAsync(resetCountFieldView, viewDto);

            //Записать информацию что сообщение о сбросе повтора отображалось пользователю
            stateDto = new TempSDto() { TempData = TempStateUserEnum.ShowResetCountRepeatSentences};
            await _botStateTreeUserHandler.SetDataAndActionAsync(updateBDto, updateBDto.TelegramState!.Action,
                stateDto);

            return true;
        }
        else
        {
            //Очистка промежуточных данных пользователя
            if (stateDto != null)
            {
                await _botStateTreeUserHandler.ClearDataAsync(updateBDto);
            }

            return false;
        }
    }
    
    public async Task<SentenceForRepeatDto> GetSentencesPairAndSaveInDataAsync(UpdateBDto updateBDto)
    {
        var repeatSentenceDto = await _sender.Send(new GetSentenceForRepeatQuery() { UserId = updateBDto.GetUserId() });
        //Сохроняет промежуточные данны для сравнения слов между вводами
        var dataModel = new SentencesRepetitionByInputSDto()
        {
            UsingSentencesPairId = repeatSentenceDto.UsingSentencesPairId,
            FirstSentence = repeatSentenceDto.FirstSentence,
            SecondSentence = repeatSentenceDto.SecondSentence,
            SentenceToLearnLabelEnum = SentenceToLearnLabelEnum.First //TODO должно подставляться из repeatSentenceDto
        };
        await _botStateTreeUserHandler.SetDataAndActionAsync(updateBDto,
            SentencesRepetitionByInputField.Action,
            dataModel);

        return repeatSentenceDto;
    }

    public string GetNextSentence(SentenceForRepeatDto sentenceForRepeatDto)
    {
        if (sentenceForRepeatDto.SentenceToLearnLabel == SentenceToLearnLabelEnum.First)
        {
            return sentenceForRepeatDto.SecondSentence;
        }
        else if (sentenceForRepeatDto.SentenceToLearnLabel == SentenceToLearnLabelEnum.Second)
        {
            return sentenceForRepeatDto.FirstSentence;
        }

        throw new ArgumentException("Convert enum error");
    }

    public string GetOriginalSentence(SentencesRepetitionByInputSDto sentenceForRepeatDto)
    {
        if (sentenceForRepeatDto.SentenceToLearnLabelEnum == SentenceToLearnLabelEnum.First)
        {
            return sentenceForRepeatDto.FirstSentence;
        }
        else if (sentenceForRepeatDto.SentenceToLearnLabelEnum == SentenceToLearnLabelEnum.Second)
        {
            return sentenceForRepeatDto.SecondSentence;
        }

        throw new ArgumentException("Convert enum error");
    }
}
