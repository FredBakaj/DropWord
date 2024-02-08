using DropWord.Domain.Entities;

namespace DropWord.Application.UseCase.Sentence.Queries.GetSentencesPair;

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
            CreateMap<SentenceEntity, SentenceDto>();
        }
    }
}

