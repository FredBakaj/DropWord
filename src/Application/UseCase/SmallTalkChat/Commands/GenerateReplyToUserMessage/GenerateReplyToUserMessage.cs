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
        var prompt = "";
        if (chatHistory.Count() > 2)
        {
            prompt = GeneratePromptToContinueChat(autoChatBot!.Name, user!.Name, user.Gender.ToString()!, autoChatBot!.Description, chatHistoryText);
        }
        else
        {
            prompt = GeneratePromptToStartChat(autoChatBot!.Name, autoChatBot!.Description, chatHistoryText);
        }
        
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

    private string GeneratePromptToContinueChat(string botName, string userName, string userGender, string botDescription, string chatHistoryText)
    {
        var dateTimeNow = DateTime.UtcNow + TimeSpan.FromHours(2);
        var timeNow = dateTimeNow.ToString("hh:mm tt"); // "tt" will display "AM" or "PM"
        var dateNow = dateTimeNow.ToString("dd.MM.yyyy");
        
        var random = new Random();
        // часто в конце предложения задаёт вопрос, хочеться чтобы переодически юзер сам был инициатором разговора
        // по этому с опередёным шансом будет запрещаться задавать вопросы
        //TODO сделать шанс уникальным для каждого юзера
        var randomNumber = random.Next() % 100;
        var banOnAskingQuestion = randomNumber < 70 ? $"{botName} doesn't like to ask questions" : "";
        // Модель постояно пишет большие предложения нужно часть из них сократить
        var randomNumberBanLongAnswer = random.Next() % 100;
        var banLongAnswer = randomNumberBanLongAnswer < 70 ? $"{botName} doesn't like to write long messages" : "";
        
        
        var result = $"""
                     ### Human Description ({botName})
                     {botDescription}                    
                     The gender of {userName} is {userGender}.

                     ### Human Relationship
                     The person is in a chat room where people want to improve their language skills and communicate on various topics.
                     The time is now {timeNow}.
                     The date is now {dateNow}.
                     
                     ####Instructions
                     Make up a text message on behalf of {botName}. It can be a question or a made up life story. The answer should be based on the chat history, and the description of {botName}. Also, the answers should not be too large 1-2 sentences. Before forming the text of the message, you should briefly analyze what is the best answer and why.
                     {banOnAskingQuestion}
                     {banLongAnswer}
                     ####Chat History
                     {chatHistoryText}
                     
                     ### Response Creation Template
                     BRIEFLY_ANALYZE: Analyze
                     GENERATE_TEXT_MESSAGE: Text

                     """;
        return result;
    }
    
    private string GeneratePromptToStartChat(string botName, string botDescription, string chatHistoryText)
    {
        // Время по ютиси + 2 часа. Как по Украине
        var dateTimeNow = DateTime.UtcNow + TimeSpan.FromHours(2);
        var timeNow = dateTimeNow.ToString("hh:mm tt"); // "tt" will display "AM" or "PM"
        var dateNow = dateTimeNow.ToString("dd.MM.yyyy");

        var moods = new[]
        {
            "Joy and happiness", 
            "Anxiety and worry", 
            "Anger and irritation", 
            "Satisfaction", 
            "Surprise", 
            "Interest and inspiration", 
            "Apathy and indifference", 
            "Enthusiasm and enthusiasm", 
        };

        var randomMood = moods[(new Random()).Next(moods.Length)];
        
        var result = $"""
                     ### Human Relations
                     A man, to improve his social skills, went to an anonymous chat room to meet and discuss topics of interest.
                     The time is now {timeNow}.
                     Date is now {dateNow}.
                     
                     ####Chat History
                     {chatHistoryText}
                     
                     ####Instructions
                     You need to get to know the person you are chatting with as {botName}
                     You should not write large messages.
                     You should take into account that {botName} is in a {randomMood} mood today.
                     
                     ### Response Creation Template
                     SELECT_TYPE_MESSAGE: Type
                     GENERATE_TEXT_MESSAGE: Text
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
