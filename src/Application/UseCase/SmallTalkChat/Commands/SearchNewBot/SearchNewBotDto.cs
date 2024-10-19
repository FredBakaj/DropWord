namespace DropWord.Application.UseCase.SmallTalkChat.Commands.SearchNewBot;

public class SearchNewBotDto
{
    public int BotId { get; set; }
    public string Name { get; set; } = null!;
    
    public int Age { get; set; }
    public string Country { get; set; } = null!;
    public string Interests { get; set; } = null!;
    public string Description { get; set; } = null!;
}
