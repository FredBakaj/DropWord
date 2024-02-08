using DropWord.Domain.Enums;

namespace DropWord.Application.UseCase.Sentence.Commands.ParseSentences;

public class ParseSentencesDto
{
    public ParseSentenceLabelEnum ParseSentenceLabel { get; set; }
    public IEnumerable<string> Sentences { get; set; } = null!;
}
