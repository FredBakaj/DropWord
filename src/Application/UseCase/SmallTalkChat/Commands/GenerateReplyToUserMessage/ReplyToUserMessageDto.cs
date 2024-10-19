namespace DropWord.Application.UseCase.SmallTalkChat.Commands.GenerateReplyToUserMessage;

public class ReplyToUserMessageDto
{
    public string InterlocutorsName { get; set; } = null!;

    public string Message { get; set; } = null!;
}
