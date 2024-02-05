using DropWord.Infrastructure.Sentence;
using FluentAssertions;
using NUnit.Framework;

namespace DropWord.Application.UnitTests.Sentence;

public class DifferenceSentenceTests
{
    [Test]
    public void GetDifferenceStringTest()
    {
        // var originalText =
        //     "This C# code is using the null-forgiving operator (!)" +
        //     " in combination with the null-coalescing assignment" +
        //     " (= null!) to specify that the property SecondSentence" +
        //     " can bett assigned a null value and that it should not" +
        //     " trigger nullable reference type warnings.";
        // var newText =
        //     "This C# code is using the null-forgiving the operator (!)" +
        //     " in comnation with the nule-coalescing assignmment" +
        //     " (= null!) to specify that the property SecondSentence" +
        //     " can be assigned a nule valuee and thattt it should not" +
        //     " trigger nullable reference types a warnings.";
        var originalText = "Эта команда может быть полезна, например, для переупорядочивания коммитов, коррекции сообщений коммитов, объединения нескоjльких коммитов в два и т.д. Однако, будьте осторожны при использовании git rebase,";
        var newText = "Эта команда может быть полезна, напрмер, для переупорядlочивания коммитов, коррекiiции сообщений коммитов, обединения нескольких коммив в один и т.д. Однако, будьте хорошо при  git rebase,";

        var actual = new DifferenceSentence();

        var result = actual.DiffSentencePercent(originalText, newText);
        
        result.Should().BeInRange(0, 1);
    }
}
 