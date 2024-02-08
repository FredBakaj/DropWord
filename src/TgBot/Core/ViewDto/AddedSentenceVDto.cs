using AutoMapper;
using DropWord.Application.UseCase.Sentence.Commands.AddSentence;
using DropWord.Application.UseCase.Sentence.Commands.UpdateSentence;
using DropWord.Application.UseCase.SentencesCollection.Commands.AddCollection;

using GetSentencesPair_SentencesPairDto = DropWord.Application.UseCase.Sentence.Queries.GetSentencesPair.SentencesPairDto;
using SentencesCollection_SentencesPairDto = DropWord.Application.UseCase.SentencesCollection.Commands.AddCollection.SentencesPairDto;

namespace DropWord.TgBot.Core.ViewDto;

public class AddedSentenceVDto : BaseVDto
{
    public int SentencePairId { get; set; }
    public SentenceVDto FirstSentence { get; set; } = null!;
    public SentenceVDto SecondSentence { get; set; } = null!;

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<AddSentencePairDto, AddedSentenceVDto>()
                .ForMember(dest => dest.SentencePairId, opt => opt.MapFrom(src => src.Id));
            
            CreateMap<UpdateSentencesPairDto, AddedSentenceVDto>()
                .ForMember(dest => dest.SentencePairId, opt => opt.MapFrom(src => src.Id));
            
            CreateMap<SentencesPairDto, AddedSentenceVDto>()
                .ForMember(dest => dest.SentencePairId, opt => opt.MapFrom(src => src.Id));
            
        }
    }
}
