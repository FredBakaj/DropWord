using DropWord.Application.UseCase.SmallTalkChat.Commands.CancelChat;
using DropWord.Application.UseCase.SmallTalkChat.Commands.GenerateReplyToUserMessage;
using DropWord.Application.UseCase.SmallTalkChat.Commands.SearchNewBot;
using DropWord.TgBot.Core.Extension;
using DropWord.TgBot.Core.Field;
using DropWord.TgBot.Core.Field.Controller;
using DropWord.TgBot.Core.Field.View;
using DropWord.TgBot.Core.Handler.BotStateTreeHandler;
using DropWord.TgBot.Core.Handler.BotStateTreeUserHandler;
using DropWord.TgBot.Core.Handler.BotViewHandler;
using DropWord.TgBot.Core.Handler.TaskProcessingHandler;
using DropWord.TgBot.Core.Model;
using DropWord.TgBot.Core.ViewDto;
using MediatR;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace DropWord.TgBot.Core.Src.Controller.Implementation;

public class SmallTalkChatController : IBotController
{
    private readonly IBotStateTreeHandler _botStateTreeHandler;
    private readonly IBotViewHandler _botViewHandler;
    private readonly ISender _sender;
    private readonly IBackgroundTaskHandler _backgroundTaskHandler;
    private readonly IBotStateTreeUserHandler _botStateTreeUserHandler;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<SmallTalkChatController> _logger;
    public string Name() => SmallTalkChatField.SmallTalkChatState;

    public SmallTalkChatController(IBotStateTreeHandler botStateTreeHandler, IBotViewHandler botViewHandler,
        ISender sender,
        IBackgroundTaskHandler backgroundTaskHandler,
        IBotStateTreeUserHandler botStateTreeUserHandler,
        IServiceProvider serviceProvider,
        ILogger<SmallTalkChatController> logger)
    {
        _botStateTreeHandler = botStateTreeHandler;
        _botViewHandler = botViewHandler;
        _sender = sender;
        _backgroundTaskHandler = backgroundTaskHandler;
        _botStateTreeUserHandler = botStateTreeUserHandler;
        _serviceProvider = serviceProvider;
        _logger = logger;

        Initialize();
    }


    public async Task Exec(UpdateBDto update)
    {
        await _botStateTreeHandler.ExecuteRoute(update);
    }

    private void Initialize()
    {
        _botStateTreeHandler.AddAction(SmallTalkChatField.SmallTalkChatAction, OnAutoChatAction);
        _botStateTreeHandler.AddKeyboard(SmallTalkChatField.SmallTalkChatAction, SmallTalkChatField.BackKeyboard,
            OnCancelKeyboard);
        _botStateTreeHandler.AddKeyboard(SmallTalkChatField.SmallTalkChatAction,
            SmallTalkChatField.SearchNewUserKeyboard, OnSearchNewUserKeyboard);

        _botStateTreeHandler.AddAction(SmallTalkChatField.SearchingNewUserAction, OnSearchingNewUserAction);
        _botStateTreeHandler.AddKeyboard(SmallTalkChatField.SearchingNewUserAction,
            SmallTalkChatField.CancelSearchKeyboard, OnCancelSearchKeyboard);

        _botStateTreeHandler.AddAction(SmallTalkChatField.SmallTalkWriteMessageAction, OnSmallTalkWriteMessageAction);
        _botStateTreeHandler.AddKeyboard(SmallTalkChatField.SmallTalkWriteMessageAction,
            SmallTalkChatField.BackKeyboard, OnCancelKeyboard);
        _botStateTreeHandler.AddKeyboard(SmallTalkChatField.SmallTalkWriteMessageAction,
            SmallTalkChatField.SearchNewUserKeyboard, OnSearchNewUserKeyboard);
    }

    private async Task OnAutoChatAction(UpdateBDto updateBDto)
    {
        await _botViewHandler.SendAsync(SmallTalkChatViewField.AutoChatAction, updateBDto);
    }

    private async Task OnSearchingNewUserAction(UpdateBDto updateBDto)
    {
        await _botViewHandler.SendAsync(SmallTalkChatViewField.SearchNewUserRunning, updateBDto);
    }

    private async Task OnCancelKeyboard(UpdateBDto updateBDto)
    {
        await _botStateTreeUserHandler.SetStateAndActionAsync(updateBDto, BaseField.BaseState, BaseField.BaseAction);
        await _botViewHandler.SendAsync(BaseViewField.Menu, updateBDto);
    }

    private async Task OnCancelSearchKeyboard(UpdateBDto updateBDto)
    {
        if (await _backgroundTaskHandler.IsProcessRunningAsync(updateBDto.GetUserId(),
                TaskProcessingField.SearchNewUserMessage))
        {
            await _backgroundTaskHandler.StopProcessAsync(updateBDto.GetUserId(),
                TaskProcessingField.SearchNewUserMessage);
        }

        await _botStateTreeUserHandler.SetStateAndActionAsync(updateBDto, SmallTalkChatField.SmallTalkChatState,
            SmallTalkChatField.SmallTalkChatAction);
        await _botViewHandler.SendAsync(SmallTalkChatViewField.CancelSearchKeyboard, updateBDto);
    }

    private async Task OnSearchNewUserKeyboard(UpdateBDto updateBDto)
    {
        if (await _backgroundTaskHandler.IsProcessRunningAsync(updateBDto.GetUserId(),
                TaskProcessingField.SearchNewUserMessage))
        {
            await _botViewHandler.SendAsync(SmallTalkChatViewField.SearchNewUserRunning, updateBDto);
        }
        else
        {
            var services = _serviceProvider.CreateScope();
            //перед поискок закрываем открытые чаты
            await _sender.Send(new CancelChatCommand() { UserId = updateBDto.GetUserId() });

            await _backgroundTaskHandler.StartProcessAsync(updateBDto.GetUserId(),
                TaskProcessingField.SearchNewUserMessage,
                SearchNewUserAsync, updateBDto, services);

            await _botViewHandler.SendAsync(SmallTalkChatViewField.SearchNewUserKeyboard, updateBDto);
            await _botStateTreeUserHandler.SetActionAsync(updateBDto, SmallTalkChatField.SearchingNewUserAction);
        }
    }

    private async Task SearchNewUserAsync(UpdateBDto updateBDto, IServiceScope serviceScope,
        CancellationToken cancellationToken)
    {
        var sender = serviceScope.ServiceProvider.GetRequiredService<ISender>();
        var botViewHandler = serviceScope.ServiceProvider.GetRequiredService<IBotViewHandler>();
        var botStateTreeUserHandler = serviceScope.ServiceProvider.GetRequiredService<IBotStateTreeUserHandler>();
        var logger = serviceScope.ServiceProvider.GetRequiredService<ILogger<SmallTalkChatController>>();

        Random random = new Random();
        await Task.Delay(random.Next(1, 6) * 1000, cancellationToken);

        try
        {
            var searchNewBot = await sender.Send(new SearchNewBotCommand() { UserId = updateBDto.GetUserId() },
                cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
            var viewDto = new SearchNewUserSuccessfulResultVDto()
            {
                Update = updateBDto,
                Name = searchNewBot.Name,
                Age = searchNewBot.Age,
                Country = searchNewBot.Country,
                Interests = searchNewBot.Interests
            };
            await botViewHandler.SendAsync(SmallTalkChatViewField.SearchNewUserSuccessfulResult, viewDto);
            await botStateTreeUserHandler.SetActionAsync(updateBDto, SmallTalkChatField.SmallTalkWriteMessageAction);
        }
        catch (Exception ex)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await botViewHandler.SendAsync(SmallTalkChatViewField.SearchNewUserBadResult, updateBDto);
            await botStateTreeUserHandler.SetActionAsync(updateBDto, SmallTalkChatField.SmallTalkChatAction);
            logger.LogError(ex, ex.Message);
        }
        finally
        {
            serviceScope.Dispose();
        }
    }

    private async Task OnSmallTalkWriteMessageAction(UpdateBDto updateBDto)
    {
        var services = _serviceProvider.CreateScope();
        if (await _backgroundTaskHandler.IsProcessRunningAsync(updateBDto.GetUserId(),
                TaskProcessingField.GenerateReplyToUserMessage))
        {
            await _backgroundTaskHandler.StopProcessAsync(updateBDto.GetUserId(),
                TaskProcessingField.GenerateReplyToUserMessage);
        }

        await _backgroundTaskHandler.StartProcessAsync(updateBDto.GetUserId(),
            TaskProcessingField.GenerateReplyToUserMessage,
            GenerateReplyToUserMessageAsync, updateBDto, services);
    }

    private async Task GenerateReplyToUserMessageAsync(UpdateBDto updateBDto, IServiceScope serviceScope,
        CancellationToken cancellationToken)
    {
        var sender = serviceScope.ServiceProvider.GetRequiredService<ISender>();
        IBackgroundTaskHandler backgroundTaskHandler =
            serviceScope.ServiceProvider.GetRequiredService<IBackgroundTaskHandler>();
        IBotStateTreeUserHandler botStateTreeUserHandler =
            serviceScope.ServiceProvider.GetRequiredService<IBotStateTreeUserHandler>();

        // ждем перед первым отправлением
        Random random = new Random();
        await Task.Delay(random.Next(1, 6) * 1000);

        // Запускаем отправку события в телеграмм, чтобы в чате с ботом писало, что бот пишет
        await backgroundTaskHandler.StartProcessAsync(updateBDto.GetUserId(),
            TaskProcessingField.SendTypingWhenGenerateReplay,
            SendTypingWhenGenerateReplayAsync, updateBDto, serviceScope);

        try
        {
            var replyToUser = await sender.Send(
                new GenerateReplyToUserMessageCommand()
                {
                    UserId = updateBDto.GetUserId(), Message = updateBDto.GetMessage().Text!
                },
                cancellationToken);

            var viewDto = new SmallTalkWriteMessageVDto()
            {
                Update = updateBDto,
                InterlocutorsName = replyToUser.InterlocutorsName,
                Message = replyToUser.Message,
            };

            await _botViewHandler.SendAsync(SmallTalkChatViewField.SmallTalkWriteMessage, viewDto);
        }
        catch (Exception)
        {
            await _botViewHandler.SendAsync(SmallTalkChatViewField.SmallTalkEndChating, updateBDto);
            await botStateTreeUserHandler.SetActionAsync(updateBDto, SmallTalkChatField.SmallTalkChatAction);
        }


        serviceScope.Dispose();
    }

    // Отправляет юзеру каждые 5 секунд что бот пишет 
    private async Task SendTypingWhenGenerateReplayAsync(UpdateBDto updateBDto, IServiceScope serviceScope,
        CancellationToken cancellationToken)
    {
        IBackgroundTaskHandler backgroundTaskHandler =
            serviceScope.ServiceProvider.GetRequiredService<IBackgroundTaskHandler>();
        ITelegramBotClient botClient = serviceScope.ServiceProvider.GetRequiredService<ITelegramBotClient>();

        while (!cancellationToken.IsCancellationRequested)
        {
            //проверка что генерация ответа продолжаеться
            if (!await backgroundTaskHandler.IsProcessRunningAsync(updateBDto.GetUserId(),
                    TaskProcessingField.GenerateReplyToUserMessage))
            {
                break;
            }

            await botClient.SendChatActionAsync(updateBDto.GetUserId(), ChatAction.Typing);
            await Task.Delay(5000);
        }
    }
}
