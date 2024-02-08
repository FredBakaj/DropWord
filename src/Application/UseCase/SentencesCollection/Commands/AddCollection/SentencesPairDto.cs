using DropWord.Domain.Entities;

namespace DropWord.Application.UseCase.SentencesCollection.Commands.AddCollection;

public class SentencesPairDto
{
    public int Id { get; set; }
    public SentenceDto FirstSentence { get; set; } = null!;
    public SentenceDto SecondSentence { get; set; } = null!;
    
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<SentencesPairEntity, SentencesPairDto>();
        }
    } 
}
