using System.Text;
using DiffMatchPatch;

namespace DropWord.Infrastructure.Sentence;

public class DifferenceSentence
{
    public string GetDifferenceString(string original, string modified)
    {
        diff_match_patch dmp = new diff_match_patch();
        List<Diff> diff = dmp.diff_main(original, modified);
        // Result: [(-1, "Hell"), (1, "G"), (0, "o"), (1, "odbye"), (0, " World.")]
        dmp.diff_cleanupSemantic(diff);
        var result = DiffText(diff);
        // Result: [(-1, "Hello"), (1, "Goodbye"), (0, " World.")]
        // for (int i = 0; i < diff.Count; i++) {
        //     Console.WriteLine(diff[i]);
        // }
        return string.Empty;
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
                    stringBuilder.Append("<u><b>").Append(str).Append("</b></u>");
                    continue;
                case Operation.INSERT:
                    stringBuilder.Append("<s>").Append(str).Append("</s>");
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
}
