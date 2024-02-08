namespace DropWord.Application.UseCase.Sentence.Commands.AddSentence;

public class AddSentencePairDto
{
    public int Id { get; set; }
    public SentenceDto FirstSentence { get; set; } = null!;
    public SentenceDto SecondSentence { get; set; } = null!;
}
