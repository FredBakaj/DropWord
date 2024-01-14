using DropWord.Application.UseCase.Sentence.Commands.AddSentence;

namespace DropWord.TgBot.Core.ViewDto;

public class AddSentencesVDto : BaseVDto
{
    public IEnumerable<AddSentenceDto> Sentences { get; set; } = null!;
}
