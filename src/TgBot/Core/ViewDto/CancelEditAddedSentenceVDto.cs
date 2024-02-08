using AutoMapper;
using DropWord.Application.UseCase.Sentence.Queries.GetSentencesPair;
using SentenceDto = DropWord.Application.UseCase.Sentence.Commands.UpdateSentence.SentenceDto;

namespace DropWord.TgBot.Core.ViewDto;

public class CancelEditAddedSentenceVDto : BaseVDto
{
    public int SentencePairId { get; set; }
    public SentenceVDto FirstSentence { get; set; } = null!;
    public SentenceVDto SecondSentence { get; set; } = null!;
    
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<SentencesPairDto, CancelEditAddedSentenceVDto>()
                .ForMember(dest => dest.SentencePairId, opt => opt.MapFrom(src => src.Id));
            CreateMap<SentenceDto, SentenceVDto>();
            
        }
    }
}
