using System.Text.RegularExpressions;
using DropWord.Application.Common.Interfaces;
using DropWord.Application.Manager.Ai;
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
    private readonly IAiManager _aiManager;

    private static readonly Regex regexPattern = new Regex(@"GENERATE_TEXT_MESSAGE:\s*(.*)", RegexOptions.Compiled);


    public GenerateReplyToUserMessageCommandHandler(IApplicationDbContext context, IAiManager aiManager)
    {
        _context = context;
        _aiManager = aiManager;
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
        if (chatHistory.Count() > 6)
        {
            prompt = GeneratePromptToContinueChat(autoChatBot!.Name, user!.Name, user.Gender.ToString()!,
                autoChatBot!.Description, chatHistoryText, false);
        }
        else if (chatHistory.Count() > 5)
        {
            prompt = GeneratePromptToContinueChat(autoChatBot!.Name, user!.Name, user.Gender.ToString()!,
                autoChatBot!.Description, chatHistoryText, true);
        }
        else
        {
            prompt = GeneratePromptToStartChat(autoChatBot!.Name, autoChatBot!.Description, chatHistoryText);
        }

        var responseMessage = await _aiManager.QueryToLlmModelAsync(prompt);
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
        return new ReplyToUserMessageDto() { InterlocutorsName = autoChatBot.Name, Message = responseMessage };
    }

    private string GeneratePromptToContinueChat(string botName, string userName, string userGender,
        string botDescription, string chatHistoryText, bool isNeedTopicDiscussion)
    {
        var random = new Random();
        var maxCountMessage = new List<int>
        {
            7,
            13,
            13,
            13,
            13,
            13,
            17,
            25
        };
        var countMessage = maxCountMessage[random.Next(maxCountMessage.Count)];

        var listBehaviorVector =
            new List<string>
            {
                "you have to ask a question",
                "you have to ask a question",
                "you have to make a joke",
                "you have to tell a story",
                "you have to tell a story",
                "you have to tell a story",
            };
        var behaviorVector = listBehaviorVector[random.Next(listBehaviorVector.Count)];

        var randomTopic = "";
        if (isNeedTopicDiscussion)
        {
            var topicForDiscussion = new List<string>()
            {
                "Daily Routine",
                "Hobbies and Interests",
                "Travel and Places",
                "Work and Career",
                "Movies and TV Shows",
                "Books and Reading",
                "Food and Cooking",
                "Technology and Gadgets",
                "Music and Artists",
                "Sports and Fitness",
                "Shopping and Fashion",
                "Current Events and News",
                "Weather and Seasons",
                "Friends and Social Life",
                "Dreams and Goals",
                "Holidays and Celebrations",
                "Transportation and Driving",
                "Pets and Animals",
                "Learning and Education",
                "Funny Stories and Experiences"
            };

            randomTopic = topicForDiscussion[random.Next(topicForDiscussion.Count)];
            randomTopic = $"discussion topic: {randomTopic}";
        }

        var result = $"""
                      ###Instructions
                        Your job is to communicate with the person you're talking to like a real person. {behaviorVector}
                        {randomTopic}
                        you can't write sentences longer than {countMessage} words.
                      ###Chat history
                      {chatHistoryText}

                      ### The format of the answer you should return!
                        GENERATE_TEXT_MESSAGE: Answer for person
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
            "Joy and happiness", "Anxiety and worry", "Anger and irritation", "Satisfaction", "Surprise",
            "Interest and inspiration", "Apathy and indifference", "Enthusiasm and enthusiasm",
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
