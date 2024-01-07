using AutoMapper;
using DropWord.TgBot.Core.Handler;
using DropWord.TgBot.Core.Model;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace DropWord.TgBot.Core.Service.Implementation;

public class UpdateHandler : IUpdateHandler
{
    private readonly ILogger<UpdateHandler> _logger;
    private readonly IMapper _mapper;
    private readonly IServiceProvider _serviceProvider;


    public UpdateHandler(ILogger<UpdateHandler> logger,
        IMapper mapper,
        IServiceProvider serviceProvider)
    {
        _logger = logger;
        _mapper = mapper;
        _serviceProvider = serviceProvider;
    }

    public async Task HandleUpdateAsync(ITelegramBotClient _, Update update, CancellationToken cancellationToken)
    {
        //Необходим try для непредвиденных исключений, в противном случае бот будет падать
        var scope = _serviceProvider.CreateScope();
        var botMiddlewareHandler = scope.ServiceProvider.GetRequiredService<IBotMiddlewareHandler>();
        try
        {
            UpdateBDto? userUpdate = _mapper.Map<UpdateBDto>(update);
            await botMiddlewareHandler!.Run(userUpdate!);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.ToString());
        }
        finally
        {
            scope.Dispose();
        }
    }

    public async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception,
        CancellationToken cancellationToken)
    {
        Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));

        await Task.CompletedTask;
    }
}
