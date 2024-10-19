using DropWord.Application.Common.Interfaces;
using DropWord.Domain.Entities;
using DropWord.Domain.Exceptions;

namespace DropWord.Application.UseCase.SmallTalkChat.Commands.SearchNewBot;

public record SearchNewBotCommand : IRequest<SearchNewBotDto>
{
    public long UserId { get; set; }
}

public class SearchNewBotCommandValidator : AbstractValidator<SearchNewBotCommand>
{
    public SearchNewBotCommandValidator()
    {
    }
}

public class SearchNewBotCommandHandler : IRequestHandler<SearchNewBotCommand, SearchNewBotDto>
{
    private readonly IApplicationDbContext _context;

    public SearchNewBotCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<SearchNewBotDto> Handle(SearchNewBotCommand request, CancellationToken cancellationToken)
    {
        // Получает значение из базы, есть ли открытый час с ботом у этого пользователя
        var isOpenChat = await _context.AutoChatData
            .Where(x => x.UserId == request.UserId)
            .AnyAsync(x => !x.IsClosed);

        if (isOpenChat)
        {
            throw new OpenedChatWithAnotherException("The user have a open chat with another");
        }

        // Получает данные о пользователе и ботах, которые уже были с ним
        var user = await _context.Users
            .Include(x => x.AutoChatDates)
            .Where(x => x.Id == request.UserId)
            .FirstOrDefaultAsync();

        // Получаем ид всех ботов
        var allBotIds = _context.AutoChatBot.Select(b => b.Id).ToList();

        // Опередяем сколько уже было переписко с каждым из ботов
        var countsOldChatWithBots = allBotIds
            .GroupJoin(
                user!.AutoChatDates,
                botId => botId,
                chat => chat.AutoChatBotId,
                (botId, chats) => new { Id = botId, Count = chats.Count() })
            .OrderBy(x => x.Count)
            .ToList();

        int botIdWithChat = 0;

        // Если есть боты с которыми юзер ещё не переписывался, то выбираем 
        // из них случайного
        if (countsOldChatWithBots.Any(x => x.Count == 0))
        {
            var autoChatBots = countsOldChatWithBots.Where(x => x.Count == 0).ToList();
            ;
            Random random = new();
            int randomIndex = random.Next(autoChatBots.Count);
            botIdWithChat = autoChatBots[randomIndex].Id;
        }
        else
        {
            botIdWithChat = countsOldChatWithBots.FirstOrDefault()!.Id;
        }

        // Открываем переписку с ботом
        await _context.AutoChatData.AddAsync(new AutoChatDataEntity()
        {
            UserId = user.Id, AutoChatBotId = botIdWithChat, IsClosed = false,
        });

        await _context.SaveChangesAsync(cancellationToken);

        var bot = await _context.AutoChatBot
            .Where(x => x.Id == botIdWithChat)
            .FirstOrDefaultAsync();

        return new SearchNewBotDto()
        {
            BotId = botIdWithChat,
            Name = bot!.Name,
            Age = bot.Age,
            Country = bot.Country,
            Interests = bot.Interests,
            Description = bot!.Description
        };
    }
}
