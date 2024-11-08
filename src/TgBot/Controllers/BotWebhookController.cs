using DropWord.TgBot.Core.Service;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
    private readonly IConfiguration _configuration;

    public BotWebhookController(IUpdateHandler updateHandler, ILogger<BotWebhookController> logger, ITelegramBotClient client, IConfiguration configuration)
    {
        _updateHandler = updateHandler;
        _logger = logger;
        _botClient = client;
        _configuration = configuration;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Update update, [FromServices] ITelegramBotClient bot, CancellationToken ct)
    {
        if (Request.Headers["X-Telegram-Bot-Api-Secret-Token"] != _configuration.GetSection("CommonSettings")["WebHookSecretToken"]!)
            return Forbid();
        try
        {
            await _updateHandler.HandleUpdateAsync(_botClient ,update, ct);
        }
        catch (Exception exception)
        {
            await _updateHandler.HandleErrorAsync(_botClient, exception, ct);
        }
        return Ok();
    }
   
}
