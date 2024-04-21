using DropWord.Application.Manager.Sentence.Implementation.Model;
using DropWord.Domain.Enums;

namespace DropWord.Application.Strategy.SentencesForRepeat.Implementation;

public class ASentencesForRepeat
{
    protected SentenceForRepeatModel CreateResponse(int usingSentencesPairId, string firstSentence,
        string secondSentence, string firstLang, string secondLang,
        LearnSentencesModeEnum learnSentencesModeEnum, bool isLearning)
    {
        var sentenceToLearnLabel = DetectSentenceToLearnLabel(isLearning, learnSentencesModeEnum);
        
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

    private SentenceToLearnLabelEnum DetectSentenceToLearnLabel(bool isLearning,
        LearnSentencesModeEnum learnSentencesModeEnum)
    {
        if (learnSentencesModeEnum == LearnSentencesModeEnum.MainLanguage)
        {
            return SentenceToLearnLabelEnum.Second;
        }
        else if (learnSentencesModeEnum == LearnSentencesModeEnum.LearnLanguage)
        {
            return SentenceToLearnLabelEnum.First;
        }
        else if (learnSentencesModeEnum == LearnSentencesModeEnum.Random)
        {
            List<SentenceToLearnLabelEnum> label = new List<SentenceToLearnLabelEnum>()
            {
                SentenceToLearnLabelEnum.First, SentenceToLearnLabelEnum.Second
            };
            Random random = new Random();

            // Генерируем случайный индекс
            int randomIndex = random.Next(0, label.Count);

            // Получаем элемент списка по случайному индексу
            SentenceToLearnLabelEnum randomElement = label[randomIndex];

            return randomElement;
        }
        else if (learnSentencesModeEnum == LearnSentencesModeEnum.Learned)
        {
            if (isLearning)
            {
                return SentenceToLearnLabelEnum.Second;
            }
            else
            {
                return SentenceToLearnLabelEnum.First;
            }
        }

        throw new ArgumentException("not correct learnSentencesModeEnum value");
    }
}
