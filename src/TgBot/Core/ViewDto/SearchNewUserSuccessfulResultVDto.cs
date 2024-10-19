namespace DropWord.TgBot.Core.ViewDto;

public class SearchNewUserSuccessfulResultVDto: BaseVDto
{
    public string InterlocutorsName { get; set; } = null!;
    public string Name { get; set; } = null!;
    public int Age { get; set; }
    public string Country { get; set; } = null!;
    public string Interests { get; set; } = null!;
}
