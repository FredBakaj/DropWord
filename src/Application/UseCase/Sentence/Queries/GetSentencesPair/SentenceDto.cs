namespace DropWord.Application.UseCase.Sentence.Queries.GetSentencesPair;

public class SentenceDto
{
    public int Id { get; set; }
    public string Sentence { get; set; } = null!;
    public string Language { get; set; } = null!;
}
