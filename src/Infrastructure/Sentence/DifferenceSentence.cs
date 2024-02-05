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
        // Приведение строк к нижнему регистру (регистронезависимый анализ)
        str1 = str1.ToLower();
        str2 = str2.ToLower();

        // Вычисление расстояния Левенштейна
        int levenshteinDistance = ComputeLevenshteinDistance(str1, str2);

        // Вычисление максимальной длины строки
        int maxLength = Math.Max(str1.Length, str2.Length);

        // Вычисление процента совпадения
        double similarityPercentage = ((double)(maxLength - levenshteinDistance) / maxLength) * 100;

        return similarityPercentage;
    }

    private int ComputeLevenshteinDistance(string str1, string str2)
    {
        int[,] distanceMatrix = new int[str1.Length + 1, str2.Length + 1];

        for (int i = 0; i <= str1.Length; i++)
        {
            for (int j = 0; j <= str2.Length; j++)
            {
                if (i == 0)
                {
                    distanceMatrix[i, j] = j;
                }
                else if (j == 0)
                {
                    distanceMatrix[i, j] = i;
                }
                else
                {
                    int cost = (str1[i - 1] == str2[j - 1]) ? 0 : 1;
                    distanceMatrix[i, j] = Math.Min(Math.Min(
                            distanceMatrix[i - 1, j] + 1,
                            distanceMatrix[i, j - 1] + 1),
                        distanceMatrix[i - 1, j - 1] + cost);
                }
            }
        }

        return distanceMatrix[str1.Length, str2.Length];
    }
}
