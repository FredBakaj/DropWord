using AutoMapper;
using DropWord.Application.UseCase.Sentence.Queries.GetSentencesPair;

namespace DropWord.TgBot.Core.ViewDto;

public class EditSentenceVDto : BaseVDto
{
    public int Id { get; set; }
    public SentenceVDto FirstSentence { get; set; } = null!;
    public SentenceVDto SecondSentence { get; set; } = null!;
    
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<SentencesPairDto, EditSentenceVDto>();
            CreateMap<SentenceDto, SentenceVDto>();
        }
    }
}
