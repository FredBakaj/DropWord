using DropWord.Application.Common.Interfaces;
using DropWord.Application.Common.Interfaces.SmallTalkChat;
using DropWord.Application.Manager.Ai;
using DropWord.Domain.Entities;
using DropWord.Domain.Enums;
using DropWord.Domain.Exceptions;

namespace DropWord.Application.UseCase.SmallTalkChat.Commands.AnalysisTalkMessages;

public record AnalysisTalkMessagesCommand : IRequest<AnalysisTalkMessagesDto>
{
    public long UserId { get; set; }
}

public class AnalysisTalkMessagesCommandValidator : AbstractValidator<AnalysisTalkMessagesCommand>
{
    public AnalysisTalkMessagesCommandValidator()
    {
    }
}

public class AnalysisTalkMessagesCommandHandler : IRequestHandler<AnalysisTalkMessagesCommand, AnalysisTalkMessagesDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IResponseMessageGenerator _responseMessageGenerator;
    private readonly IAiManager _aiManager;

    public AnalysisTalkMessagesCommandHandler(IApplicationDbContext context,
        IResponseMessageGenerator responseMessageGenerator, IAiManager aiManager)
    {
        _context = context;
        _responseMessageGenerator = responseMessageGenerator;
        _aiManager = aiManager;
    }

    public async Task<AnalysisTalkMessagesDto> Handle(AnalysisTalkMessagesCommand request,
        CancellationToken cancellationToken)
    {
        var AnalysisHistorysFor24Hours = await _context.AutoChatAnalysisHistory
            .Where(x => x.UserId == request.UserId
                        && x.Created >= DateTime.Now.AddHours(-24))
            .ToListAsync();

        //TODO Вынести 6 в конфиг
        if (AnalysisHistorysFor24Hours.Count > 6)
        {
            throw new TooManyAnalysisHistoryException(
                "Too many analysis historys for 24 hours, please try again later");
        }

        var chatData = await _context.AutoChatData
            .Where(x => x.UserId == request.UserId)
            .OrderByDescending(x => x.Id)
            .FirstOrDefaultAsync();

        var chatHistory = await _context.AutoChatHistory
            .Where(x => x.AutoChatDataId == chatData!.Id)
            .OrderByDescending(x => x.Created)
            //TODO вынести число 6 в конфиг
            .Take(6)
            .OrderBy(x => x.Created)
            .ToListAsync();

        if (chatHistory.Count == 0)
        {
            throw new NoTalkMessagesException("no talk messages found, for analysis");
        }

        var lastAnalysisHistory = await _context.AutoChatAnalysisHistory
            .Where(x => x.UserId == request.UserId)
            .OrderByDescending(x => x.Id)
            .FirstOrDefaultAsync();

        if (lastAnalysisHistory?.AutoChatDataId == chatData!.Id
            && chatHistory.Select(x => x.Id).Contains(lastAnalysisHistory.LastAnalyzeAutoChatHistoryId))
        {
            throw new ReanalysisTalkMessagesException("for these messages have already been analyzed");
        }

        var autoChatBot = await _context.AutoChatBot
            .Where(x => x.Id == chatData.AutoChatBotId)
            .FirstOrDefaultAsync();

        var user = await _context.Users
            .Where(x => x.Id == request.UserId)
            .FirstOrDefaultAsync();

        var chatHistoryText = "";
        foreach (var row in chatHistory)
        {
            var characterName = row.SenderEnum == AutoChatSenderEnum.User ? user!.Name : autoChatBot!.Name;
            chatHistoryText += $"{characterName}: {row.Message}\n";
        }

        cancellationToken.ThrowIfCancellationRequested();

        var prompt = GeneratePrompt(user!.Name, chatHistoryText);
        var responseMessage = await _aiManager.QueryToLlmModelAsync(prompt);
        responseMessage = ParseResponseMessage(responseMessage);

        var newAnalysisHistoryRecord = new AutoChatAnalysisHistoryEntity()
        {
            UserId = request.UserId,
            AutoChatDataId = chatData.Id,
            LastAnalyzeAutoChatHistoryId = chatHistory.LastOrDefault()!.Id,
            TextAnalysis = responseMessage
        };

        await _context.AutoChatAnalysisHistory.AddAsync(newAnalysisHistoryRecord);
        await _context.SaveChangesAsync(cancellationToken);

        return new AnalysisTalkMessagesDto() { TextAnalysis = responseMessage, };
    }

    private string GeneratePrompt(string userName, string chatHistory)
    {
        var result = $"""
                      #ChatHistory
                      {chatHistory}
                      #Insturction
                      Перевірити на помилки повідомленя які писав {userName}, як бы це зробыв вчитель англійскої мови
                      Потрібно скорочено описати граматичні та синтаксичні помилки. Також важливо зробити акцент на правельності написання слів.
                      Також потрібно писати коротке пояснення помилок.
                      Опис зауважень повинен бути на українськй мові.
                      Результат не повинен мати Markdown розмітку або будь-яку іншу.
                      """;

        return result;
    }

    private string ParseResponseMessage(string responseMessage)
    {
        return responseMessage;
    }
}
