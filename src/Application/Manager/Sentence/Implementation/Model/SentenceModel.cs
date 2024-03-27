using DropWord.Domain.Entities;

namespace DropWord.Application.Manager.Sentence.Implementation.Model;

public class SentenceModel
{
    public string Sentence { get; set; } = null!;
    public string Language { get; set; }= null!;
    
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<SentenceEntity, SentenceModel>();
        }
    }
}
