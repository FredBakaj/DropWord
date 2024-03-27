using DropWord.Domain.Entities;

namespace DropWord.Application.Manager.Sentence.Implementation.Model;

public class SentencesPairModel
{
    public string FirstLanguage { get; set; } = null!;
    public string SecondLanguage { get; set; } = null!;

    public long UserId { get; set; }
    public int FirstSentenceId { get; set; }
    public int SecondSentenceId { get; set; }
    public SentenceModel FirstSentence { get; set; } = null!;
    public SentenceModel SecondSentence { get; set; } = null!;

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<SentencesPairEntity, SentencesPairModel>();
        }
    }
}
