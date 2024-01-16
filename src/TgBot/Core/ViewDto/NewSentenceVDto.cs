using DropWord.Application.UseCase.Sentence.Queries.GetNewSentence;

namespace DropWord.TgBot.Core.ViewDto;

public class NewSentenceVDto : BaseVDto
{
    public NewSentenceDto NewSentence { get; set; } = null!;
}
