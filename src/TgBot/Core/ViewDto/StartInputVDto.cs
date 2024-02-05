using DropWord.Domain.Enums;

namespace DropWord.TgBot.Core.ViewDto;

public class StartInputVDto : BaseVDto
{
    public string Sentence { get; set; } = null!;
}
