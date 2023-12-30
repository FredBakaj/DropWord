using DropWord.TgBot.Core.Service;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;

namespace DropWord.TgBot.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class BotWebhookController : ControllerBase
{
    
    private readonly IUpdateHandler _updateHandler;
    private readonly ILogger<BotWebhookController> _logger;
    private readonly ITelegramBotClient _botClient;

    public BotWebhookController(IUpdateHandler updateHandler, ILogger<BotWebhookController> logger, ITelegramBotClient client)
    {
        _updateHandler = updateHandler;
        _logger = logger;
        _botClient = client;
    }

    [HttpPost]
    public async Task<IActionResult> Post()
    {
        try
        {
            Update update;
            if (Request.Body != null)
            {
                var originalBodyStream = Request.Body;

                using (var test = new StreamReader(originalBodyStream))
                {
                    var body = await test.ReadToEndAsync();

                    try
                    {
                        update = JsonConvert.DeserializeObject<Update>(body)!;

                        if (update != null)
                        {
                            CancellationTokenSource cts = new CancellationTokenSource();
                            var token = cts.Token;
                            await _updateHandler.HandleUpdateAsync(_botClient ,update, token);
                        }
                    }
                    catch (JsonException ex)
                    {
                        _logger.LogError(ex.Message);
                    }
                }
            }
        }
        catch (ApiRequestException ex)
        {
            _logger.LogError(ex.Message);
        }

        return Ok();
    }
}
