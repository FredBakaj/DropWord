using AutoMapper;
using DropWord.TgBot.Core.Handler;
using DropWord.TgBot.Core.Model;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace DropWord.TgBot.Core.Service.Implementation;

public class UpdateHandler : IUpdateHandler
{
    private readonly ILogger<UpdateHandler> _logger;
    private readonly IBotMiddlewareHandler _botMiddlewareHandler;
    private readonly IMapper _mapper;


    public UpdateHandler( ILogger<UpdateHandler> logger,
        IBotMiddlewareHandler botMiddlewareHandler, IMapper mapper)
    {
        _logger = logger;
        _botMiddlewareHandler = botMiddlewareHandler;
        _mapper = mapper;
    }

    public async Task HandleUpdateAsync(ITelegramBotClient _, Update update, CancellationToken cancellationToken)
    {
        //Необходим try для непредвиденных исключений, в противном случае бот будет падать
        try
        {
            UpdateBDto? userUpdate = _mapper.Map<UpdateBDto>(update);
            await _botMiddlewareHandler.Run(userUpdate!);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.ToString());
        }
    }

    public async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception,
        CancellationToken cancellationToken)
    {
        Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));

        await Task.CompletedTask;
    }
}
