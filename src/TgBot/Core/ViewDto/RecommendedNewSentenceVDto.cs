using AutoMapper;
using DropWord.Application.UseCase.Sentence.Queries.GetRecommendedNewSentence;
using DropWord.Domain.Enums;

namespace DropWord.TgBot.Core.ViewDto;

public class RecommendedNewSentenceVDto : BaseVDto
{
    public int Id { get; set; }
    public SentenceToLearnLabelEnum SentenceToLearnLabel { get; set; }
    public string FirstSentence { get; set; } = null!;
    public string SecondSentence { get; set; } = null!;
    public string FirstLanguage { get; set; } = null!;
    public string SecondLanguage { get; set; } = null!;
    
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<RecommendedNewSentenceDto, RecommendedNewSentenceVDto>();
        }
    }
}
