using DropWord.Application.UseCase.Sentence.Commands.UpdateStatusShowSentencesForRepeat;
using DropWord.Application.UseCase.Sentence.Queries.GetSentenceRepeatForDay;
using DropWord.Application.UseCase.Sentence.Queries.GetUsersToPushSentencesRepeatForDay;
using DropWord.Domain.Enums;
using DropWord.Domain.Exceptions;
using DropWord.Infrastructure.Utils.RestApiClient;
using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace TgBotWorker.Function;

public class RepeatForDayTrigger
{
    private readonly ISender _sender;
    private readonly IRestApiClient _restApiClient;
    private readonly ILogger _logger;

    private readonly string _apiUrl = String.Empty;
    public RepeatForDayTrigger(ILoggerFactory loggerFactory, ISender sender, IRestApiClient restApiClient, IConfiguration configuration)
    {
        _sender = sender;
        _restApiClient = restApiClient;
        _logger = loggerFactory.CreateLogger<RepeatForDayTrigger>();

        _apiUrl = configuration.GetValue<string>("ApiUrl")!;
    }

    [Function("RepeatForDayTrigger")]
    public async Task Run([TimerTrigger("0 */1 * * * *")] TimerInfo myTimer)
    {
        //TODO добавить Try
        var queryModel = await _sender.Send(new GetUsersToPushSentencesRepeatForDayQuery()
        {
            SentencesForDayMode = new List<SentencesRepeatForDayModeEnum>()
            {
                //TODO добавить конвертор время в режим пушей
                SentencesRepeatForDayModeEnum.Times1InDay
            }
        });
        
        var url = $"{_apiUrl}/api/v1/Sentence/RepeatForDay";
        
        foreach (var user in queryModel.Users)
        {
            try
            {
                var sentencePair = await _sender.Send(new GetSentenceRepeatForDayQuery() { UserId = user.Id });
                await _restApiClient.PostAsync<object>(url, new Dictionary<string, string>(),
                    new { UserId = user.Id, SentenceForRepeatApi = sentencePair });
                await _sender.Send(new UpdateStatusShowSentencesForRepeatCommand() 
                    {UsingSentencesPairId = sentencePair.UsingSentencesPairId});
            }
            catch(EmptyOldUsingSentencesPairException){}
            catch (Exception e)
            {
                _logger.LogError(e.Message);
            }
        }
    }
}
