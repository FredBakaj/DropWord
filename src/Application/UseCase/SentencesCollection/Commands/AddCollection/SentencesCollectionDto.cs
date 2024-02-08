namespace DropWord.Application.UseCase.SentencesCollection.Commands.AddCollection;

public class SentencesCollectionDto
{
    public int CollectionId { get; set; }
    public IEnumerable<SentencesPairDto> SentencesPairs { get; set; } = null!;
}
