using DropWord.Application.UseCase.Sentence.Commands.UpdateStatusShowSentencesForRepeat;
using DropWord.Application.UseCase.Sentence.Queries.GetSentenceRepeatForDay;
using DropWord.Application.UseCase.Sentence.Queries.GetUsersToPushSentencesRepeatForDay;
using DropWord.Domain.Constants;
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

    public RepeatForDayTrigger(ILoggerFactory loggerFactory, ISender sender, IRestApiClient restApiClient,
        IConfiguration configuration)
    {
        _sender = sender;
        _restApiClient = restApiClient;
        _logger = loggerFactory.CreateLogger<RepeatForDayTrigger>();

        _apiUrl = configuration.GetValue<string>("ApiUrl")!;
    }

    [Function("RepeatForDayTrigger")]
    public async Task Run([TimerTrigger("0 0 * * * *")] TimerInfo myTimer)
    {
        var sentencesForDayTimeZones = new Dictionary<SentencesRepeatForDayTimesModeEnum, TimeSpan[]>()
        {
            { SentencesRepeatForDayTimesModeEnum.Times1InDay, 
                new[]
                {
                    new TimeSpan(11, 0, 0)
                } },
            { SentencesRepeatForDayTimesModeEnum.Times3InDay,
                new[]
                {
                    new TimeSpan(11, 0, 0),
                    new TimeSpan(15, 0, 0), 
                    new TimeSpan(19, 0, 0)
                }
            },
            { SentencesRepeatForDayTimesModeEnum.Times5InDay,
                new[]
                {
                    new TimeSpan(11, 0, 0),
                    new TimeSpan(13, 0, 0), 
                    new TimeSpan(15, 0, 0),
                    new TimeSpan(17, 0, 0),
                    new TimeSpan(19, 0, 0)
                }
            },
            { SentencesRepeatForDayTimesModeEnum.Times10InDay,
                new[]
                {
                    new TimeSpan(10, 0, 0),
                    new TimeSpan(11, 0, 0),
                    new TimeSpan(12, 0, 0),
                    new TimeSpan(13, 0, 0), 
                    new TimeSpan(14, 0, 0), 
                    new TimeSpan(15, 0, 0),
                    new TimeSpan(16, 0, 0),
                    new TimeSpan(17, 0, 0),
                    new TimeSpan(18, 0, 0),
                    new TimeSpan(19, 0, 0)
                }
            },
            
        };
        //все тайм зоны которые может использовать пользователь
        var timeZone = TimeZoneForDayConst.TimeZone;
        for (int i = 0; i < timeZone.Length; i++)
        {
            TimeSpan currentTime = DateTime.UtcNow.TimeOfDay;
            //добавить к ютс времени часовой пояс с который обрабатываеться в этой этерации цикла
            currentTime = currentTime.Add(TimeSpan.FromHours(timeZone[i]));
            //получить все перечисления режима повторов у которых совпадает время с текущим часом
            var keysWithMatchingTime = sentencesForDayTimeZones
                .Where(pair => pair.Value.Any(time => time.Hours == currentTime.Hours))
                .Select(pair => pair.Key).ToList();

            //Рассылка пушей на повтор предложений 
            await PushToUserSentencesForDayAsync(timeZone[i], keysWithMatchingTime);
        }
    }

    private async Task PushToUserSentencesForDayAsync(int timeZone, List<SentencesRepeatForDayTimesModeEnum> modes)
    {
        //TODO добавить Try
        var queryModel = await _sender.Send(new GetUsersToPushSentencesRepeatForDayQuery()
        {
            TimeZone = timeZone, SentencesForDayMode = modes
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
                {
                    UsingSentencesPairId = sentencePair.UsingSentencesPairId
                });
            }
            catch (EmptyOldUsingSentencesPairException) { }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
            }
        }
    }
}
