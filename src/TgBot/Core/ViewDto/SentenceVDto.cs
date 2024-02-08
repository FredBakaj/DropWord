using AutoMapper;
using AddSentences_SentenceDto = DropWord.Application.UseCase.Sentence.Commands.AddSentence.SentenceDto;
using UpdateSentence_SentenceDto = DropWord.Application.UseCase.Sentence.Commands.UpdateSentence.SentenceDto;
using GetSentencesPair_SentenceDto = DropWord.Application.UseCase.Sentence.Queries.GetSentencesPair.SentenceDto;
using AddCollection_SentenceDto = DropWord.Application.UseCase.SentencesCollection.Commands.AddCollection.SentenceDto;

namespace DropWord.TgBot.Core.ViewDto;

public class SentenceVDto
{
    public int Id { get; set; }
    public string Sentence { get; set; } = null!;
    public string Language { get; set; } = null!;

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<AddSentences_SentenceDto, SentenceVDto>();
            CreateMap<UpdateSentence_SentenceDto, SentenceVDto>();
            CreateMap<GetSentencesPair_SentenceDto, SentenceVDto>();
            CreateMap<AddCollection_SentenceDto, SentenceVDto>();
        }
    }
}
