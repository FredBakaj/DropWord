namespace DropWord.Application.UseCase.Sentence.Commands.AddSentence;

public class AddSentenceDto
{
    public int Id { get; set; }
    public string FirstSentence { get; set; } = null!;
    public string SecondSentence { get; set; } = null!;
}
