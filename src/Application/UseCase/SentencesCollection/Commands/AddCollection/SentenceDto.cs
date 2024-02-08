using DropWord.Domain.Entities;

namespace DropWord.Application.UseCase.SentencesCollection.Commands.AddCollection;

public class SentenceDto
{
    public int Id { get; set; }
    public string Sentence { get; set; } = null!;
    public string Language { get; set; } = null!;
    
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<SentenceEntity, SentenceDto>();
        }
    } 
}
