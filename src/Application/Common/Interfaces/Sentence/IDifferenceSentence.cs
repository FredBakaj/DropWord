namespace DropWord.Application.Common.Interfaces.Sentence;

public interface IDifferenceSentence
{
    double DiffSentencePercent(string firstSentence, string secondSentence);
    string DiffSentenceWithMarkup(string oldSentence, string newSentence);
}
