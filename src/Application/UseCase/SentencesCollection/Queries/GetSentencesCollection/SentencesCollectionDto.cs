using DropWord.Domain.Entities;

namespace DropWord.Application.UseCase.SentencesCollection.Queries.GetSentencesCollection;

public class SentencesCollectionDto
{
    public int Id { get; set; }
    public string Description { get; set; } = null!;
    public string FirstLanguage { get; set; } = null!;
    public string SecondLanguage { get; set; } = null!;

    public long UserId { get; set; }
    public DateTimeOffset Created { get; set; }


    private class Mapper : Profile
    {
        public Mapper()
        {
            CreateMap<UserSentencesCollectionEntity, SentencesCollectionDto>();
        }
    }
}
