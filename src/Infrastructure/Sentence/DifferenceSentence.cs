using System.Text;
using DiffMatchPatch;
using DropWord.Application.Common.Interfaces.Sentence;

namespace DropWord.Infrastructure.Sentence;

public class DifferenceSentence : IDifferenceSentence
{
    public string DiffSentenceWithMarkup(string original, string modified)
    {
        diff_match_patch dmp = new diff_match_patch();
        List<Diff> diff = dmp.diff_main(original, modified);
        dmp.diff_cleanupSemantic(diff);
        var result = DiffText(diff);
        return result;
    }
    

    public double DiffSentencePercent(string firstSentence, string secondSentence)
    {
        var result = CalculateSimilarity(firstSentence, secondSentence);
        return result;
    }
    private string DiffText(List<Diff> diffs)
    {
        StringBuilder stringBuilder = new StringBuilder();
        foreach (Diff diff in diffs)
        {
            string str = diff.text.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\n", "&para;<br>");
            switch (diff.operation)
            {
                case Operation.DELETE:
                    stringBuilder.Append("*__").Append(str).Append("__*");
                    continue;
                case Operation.INSERT:
                    stringBuilder.Append("*~").Append(str).Append("~*");
                    continue;
                case Operation.EQUAL:
                    stringBuilder.Append(str);
                    continue;
                default:
                    continue;
            }
        }
        return stringBuilder.ToString();
    }
    private double CalculateSimilarity(string str1, string str2)
    {
        diff_match_patch dmp = new diff_match_patch();
        List<Diff> diff = dmp.diff_main(str1, str2);
        
        string diffTextEquals = string.Empty;
        foreach (var item in diff.Where(x => x.operation == Operation.EQUAL && x.text.Length > 1).Select(x => x.text).ToList())
        {
            diffTextEquals += item;
        }

        var maxLeng = Math.Max(str1.Length, diffTextEquals.Length);
        var minLeng = Math.Min(str1.Length, diffTextEquals.Length);
        
        double result = minLeng * 100 / maxLeng;
        
        return result;
    }
}
