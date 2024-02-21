using AutoMapper;
using DropWord.Application.UseCase.SentencesCollection.Commands.AddCollection;

namespace DropWord.TgBot.Core.ViewDto;

public class AddCollectionSentencesVDto : BaseVDto
{
    public int CollectionId { get; set; }
    public IEnumerable<AddedSentenceVDto> Sentences { get; set; } = null!;

    public string FirstLanguageEmoji { get; set; } = null!;
    public string SecondLanguageEmoji { get; set; } = null!;
    
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<SentencesCollectionDto, AddCollectionSentencesVDto>()
                .ForMember(dest => dest.Sentences, opt => opt.MapFrom(src => src.SentencesPairs)); ;
        }
    }

}
