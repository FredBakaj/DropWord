using DropWord.TgBot.Core.Model;

namespace DropWord.TgBot.Core.ViewDto;

public abstract class BaseVDto
{
    public UpdateBDto Update { get; set; } = null!;
}
