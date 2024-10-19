using System.Text.RegularExpressions;
using DropWord.Application.Common.Interfaces;
using DropWord.Application.Common.Interfaces.SmallTalkChat;
using DropWord.Domain.Entities;
using DropWord.Domain.Enums;
using DropWord.Domain.Exceptions;

namespace DropWord.Application.UseCase.SmallTalkChat.Commands.GenerateReplyToUserMessage;

public record GenerateReplyToUserMessageCommand : IRequest<ReplyToUserMessageDto>
{
    public long UserId { get; set; }
    public string Message { get; set; } = null!;
}

public class GenerateReplyToUserMessageCommandValidator : AbstractValidator<GenerateReplyToUserMessageCommand>
{
    public GenerateReplyToUserMessageCommandValidator()
    {
    }
}

public class
    GenerateReplyToUserMessageCommandHandler : IRequestHandler<GenerateReplyToUserMessageCommand, ReplyToUserMessageDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IResponseMessageGenerator _responseMessageGenerator;
    
    private static readonly Regex regexPattern = new Regex(@"GENERATE_TEXT_MESSAGE:\s*(.*)", RegexOptions.Compiled);


    public GenerateReplyToUserMessageCommandHandler(IApplicationDbContext context,
        IResponseMessageGenerator responseMessageGenerator)
    {
        _context = context;
        _responseMessageGenerator = responseMessageGenerator;
    }

    public async Task<ReplyToUserMessageDto> Handle(GenerateReplyToUserMessageCommand request,
        CancellationToken cancellationToken)
    {
        // проверяем есть ли открытый чат. По логике чат должен открыться при завершении поиска собеседника
        var chatData = await _context.AutoChatData
            .Where(x => x.UserId == request.UserId
                        && !x.IsClosed)
            .FirstOrDefaultAsync();

        if (chatData == null)
        {
            throw new NoOpenChatException("Could not find chat data for user " + request.UserId + ".");
        }

        //добавляем новое сообщение в чат
        var autoChatHistoryEntity = new AutoChatHistoryEntity()
        {
            AutoChatDataId = chatData.Id,
            Message = request.Message,
            SenderEnum = AutoChatSenderEnum.User,
            MessageTypeEnum = AutoChatMessageTypeEnum.Text
        };
        await _context.AutoChatHistory.AddAsync(autoChatHistoryEntity);
        await _context.SaveChangesAsync(cancellationToken);
        cancellationToken.ThrowIfCancellationRequested();
        
        // получаем информацию для генерации ответа
        var autoChatBot = await _context.AutoChatBot
            .Where(x => x.Id == chatData.AutoChatBotId)
            .FirstOrDefaultAsync();

        var chatHistory = await _context.AutoChatHistory
            .Where(x => x.AutoChatDataId == chatData.Id)
            .OrderByDescending(x => x.Created)
            //TODO вынести число 6 в конфиг
            .Take(6)
            .OrderBy(x => x.Created)
            .ToListAsync();

        var user = await _context.Users
            .Where(x => x.Id == request.UserId)
            .FirstOrDefaultAsync();

        var chatHistoryText = "";
        foreach (var row in chatHistory)
        {
            var characterName = row.SenderEnum == AutoChatSenderEnum.User ? user!.Name : autoChatBot!.Name;
            chatHistoryText += $"{characterName}: {row.Message}\n";
        }
        
        // Формирование ответа моделью ИИ
        cancellationToken.ThrowIfCancellationRequested();
        var prompt = GeneratePrompt(autoChatBot!.Name, autoChatBot!.Description, chatHistoryText);
        var responseMessage = (await _responseMessageGenerator.GenerateAiMessageAsync(prompt, 120)).Message;
        responseMessage = ParseResponseMessage(responseMessage);
        
        
        // добавляем в базу сгенерированый ответ
        cancellationToken.ThrowIfCancellationRequested();
        var generateBotAutoChatHistoryEntity = new AutoChatHistoryEntity()
        {
            AutoChatDataId = chatData.Id,
            Message = responseMessage,
            SenderEnum = AutoChatSenderEnum.Bot,
            MessageTypeEnum = AutoChatMessageTypeEnum.Text
        };
        await _context.AutoChatHistory.AddAsync(generateBotAutoChatHistoryEntity);
        await _context.SaveChangesAsync(cancellationToken);

        // возвращаем результат
        return new ReplyToUserMessageDto()
        {
            InterlocutorsName = autoChatBot.Name,
            Message = responseMessage
        };

    }

    private string GeneratePrompt(string botName, string botDescription, string chatHistoryText)
    {
        var result = $"""
                     ### Person Description ({botName})
                     {botDescription}                     
                     ### Relationships between people
                     Members of an English club, they are friends and sometimes get together over a cup of coffee to discuss topics of interest.
                     ####History of correspondence
                     {chatHistoryText}
                     ####Instruction
                     The task is to come up with a text that {botName} can say to continue the dialog. These can be different types of messages, e.g:
                     Question - a request for information from the interlocutor that may arise during the conversation.
                     Assertion - a fact or opinion that {botName} wants to express in response.
                     Command - a request or instruction that {botName} may give to the interlocutor.
                     Suggestion - an initiative or invitation to do something together.
                     Doubt/hypothesis - a message in which {botName} expresses an uncertainty or hypothesis.
                     Apology - a situation in which {botName} apologizes for something.
                     Emotional expression - a response from {botName} that conveys emotion.
                     Call to action - a suggestion that {botName} work together.
                     Feedback - feedback or reaction {botName} has to something said or done by the interlocutor.
                     The texts should relate to the context of the current dialog and reflect a possible continuation of the conversation based on what was said earlier.
                     Before forming a response, it is necessary to decide which type of message is best suited. And on the basis of it form a message.
                     Need to generate one variant of the text.
                     It is forbidden to generate a text message that offers to meet in person
                     ### Response Creation Template
                     SELECT_TYPE_MESSAGE: Type
                     GENERATE_TEXT_MESSAGE: Text
                     <break>   
                     """;
        return result;
    }

    private string ParseResponseMessage(string message)
    {
        // Ищем совпадения
        var result = regexPattern.Match(message).Groups[1].Value;
        return result;
    }
    
}
