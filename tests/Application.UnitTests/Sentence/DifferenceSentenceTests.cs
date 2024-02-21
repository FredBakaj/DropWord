using DropWord.Infrastructure.Sentence;
using FluentAssertions;
using NUnit.Framework;

namespace DropWord.Application.UnitTests.Sentence;

public class DifferenceSentenceTests
{
    [Test]
    public void GetDiff72PercentTest()
    {
        var text1 =
            "Масштабирование и управление: Azure SignalR Service автоматически масштабируется в зависимости от нагрузки вашего приложения";
        var text2 =
            "Масштабttирование и управление: Azure Signa Service автоматически  в зависимости от  вашего ";
        var act = new DifferenceSentence();
        var result = act.DiffSentencePercent(text1, text2);
        result.Should().Be(72);
    }
    
    [Test]
    public void GetDiff1PercentTest()
    {
        var text1 =
            "Масштабирование и управление: Azure SignalR Service автоматически масштабируется в зависимости от нагрузки вашего приложения";
        var text2 =
            "ьцщузйшаоьм уткшпзшугe wefwкоьубацуа";
        var act = new DifferenceSentence();
        var result = act.DiffSentencePercent(text1, text2);
        result.Should().Be(1);
    }
}
 
