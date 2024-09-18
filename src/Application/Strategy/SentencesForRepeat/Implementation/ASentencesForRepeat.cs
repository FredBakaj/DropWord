using DropWord.Application.Manager.Sentence;
using DropWord.Application.Manager.Sentence.Implementation.Model;
using DropWord.Domain.Enums;

namespace DropWord.Application.Strategy.SentencesForRepeat.Implementation;

public class ASentencesForRepeat
{
    private readonly ISentenceManager _sentenceManager;

    public ASentencesForRepeat(ISentenceManager sentenceManager)
    {
        _sentenceManager = sentenceManager;
    }
    protected SentenceForRepeatModel CreateResponse(int usingSentencesPairId, string firstSentence,
        string secondSentence, string firstLang, string secondLang,
        LearnSentencesModeEnum learnSentencesModeEnum, bool isLearning)
    {
        var sentenceToLearnLabel = _sentenceManager.DetectSentenceToLearnLabel(isLearning, learnSentencesModeEnum);
        
        return new SentenceForRepeatModel()
        {
            UsingSentencesPairId = usingSentencesPairId,
            FirstSentence = firstSentence,
            SecondSentence = secondSentence,
            FirstLanguage = firstLang,
            SecondLanguage = secondLang,
            SentenceToLearnLabel = sentenceToLearnLabel
        };
    }
}
