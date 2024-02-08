using DropWord.Domain.Entities;

namespace DropWord.Application.UseCase.Sentence.Commands.UpdateSentence;

public class UpdateSentencesPairDto
{
    public int Id { get; set; }
    public SentenceDto FirstSentence { get; set; } = null!;
    public SentenceDto SecondSentence { get; set; } = null!;
    
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<SentencesPairEntity, UpdateSentencesPairDto>();
            CreateMap<SentenceEntity, SentenceDto>();
        }
    }  
}
