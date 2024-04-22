using AutoMapper;
using DropWord.TgBot.Core.Handler.MiddlewareHandler;
using DropWord.TgBot.Core.Model;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace DropWord.TgBot.Core.Service.Implementation;

public class UpdateHandler : IUpdateHandler
{
    private readonly ILogger<UpdateHandler> _logger;
    private readonly IMapper _mapper;
    private readonly IServiceProvider _serviceProvider;
    private readonly bool _isWebhook;


    public UpdateHandler(ILogger<UpdateHandler> logger,
        IMapper mapper,
        IServiceProvider serviceProvider,
        IConfiguration configuration)
    {
        _logger = logger;
        _mapper = mapper;
        _serviceProvider = serviceProvider;
        _isWebhook = Convert.ToBoolean(configuration.GetSection("CommonSettings")["IsWebHook"]);

    }

    public async Task HandleUpdateAsync(ITelegramBotClient _, Update update, CancellationToken cancellationToken)
    {
        //Необходим try для непредвиденных исключений, в противном случае бот будет падать
        IBotMiddlewareHandler botMiddlewareHandler = null!;
        if (!_isWebhook)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            botMiddlewareHandler = scope.ServiceProvider.GetRequiredService<IBotMiddlewareHandler>();
            UpdateBDto? userUpdate = _mapper.Map<UpdateBDto>(update);
            await botMiddlewareHandler!.Run(userUpdate!);
        }
        else
        {
            botMiddlewareHandler = _serviceProvider.GetRequiredService<IBotMiddlewareHandler>();
            UpdateBDto? userUpdate = _mapper.Map<UpdateBDto>(update);
            await botMiddlewareHandler!.Run(userUpdate!);
        }
        try
        {
            
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
