namespace DropWord.TgBot.Core.ViewDto;

public class SmallTalkWriteMessageVDto: BaseVDto
{
    public string InterlocutorsName { get; set; } = null!;
    public string Message { get; set; } = null!;
}
