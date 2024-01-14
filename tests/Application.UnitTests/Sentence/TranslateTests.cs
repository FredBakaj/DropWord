using DropWord.Application.Common.Models.Sentence;
using DropWord.Infrastructure.Sentence;
using FluentAssertions;
using NUnit.Framework;

namespace DropWord.Infrastructure.IntegrationTests;

public class TranslateTests
{
    [Test]
    public async Task TranslateSentenceTest()
    {
        var failures =
            "For many centuries the printing press uses the standard fish text beginning with Lorem ipsum is a passage with no phrases from the work of Roman philosopher Cicero.";
        var originalLang = "en";
        var translateLang = "ru";
        var actual = new Translate();
        var result = await actual.TranslateAsync(failures, originalLang, translateLang);
        result.TranslateSentence.Should()
            .BeEquivalentTo(
                "На протяжении многих столетий в типографии используется стандартный рыбный текст, начинающийся с Lorem ipsum — отрывка без фраз из произведения римского философа Цицерона.");

    }
    [Test]
    public async Task TranslateSentenceListTest()
    {
        var failures = new[]
        {
            "For many centuries the printing press uses the standard fish text beginning with Lorem ipsum is a passage with no phrases from the work of Roman philosopher Cicero.",
            "For many centuries the printing press uses the standard fish text beginning with Lorem ipsum is a passage with no phrases from the work of Roman philosopher Cicero.",
            "For many centuries the printing press uses the standard fish text beginning with Lorem ipsum is a passage with no phrases from the work of Roman philosopher Cicero.",
            "For many centuries the printing press uses the standard fish text beginning with Lorem ipsum is a passage with no phrases from the work of Roman philosopher Cicero.",
            "For many centuries the printing press uses the standard fish text beginning with Lorem ipsum is a passage with no phrases from the work of Roman philosopher Cicero.",
            
        };
        var originalLang = "en";
        var translateLang = "ru";
        var actual = new Translate();
        var result = await actual.TranslateListAsync(failures, originalLang, translateLang);

        result.Count().Should().Be(5);
    }

    [Test]
    public async Task DetectLanguageTest()
    {
        var failures =
            "Lorem Ipsum - це текст-\"риба\", що використовується в друкарстві та дизайні. Lorem Ipsum є, фактично, стандартною \"рибою\" аж з XVI сторіччя, коли невідомий друкар взяв шрифтову гранку та склав на ній підбірку зразків шрифтів. \"Риба$\" не тільки успішно пережила п'ять століть, але й прижилася в електронному верстуванні, залишаючись по суті незмінною. Вона популяризувалась в 60-их роках минулого сторіччя завдяки виданню зразків шрифтів Letraset, які містили уривки з Lorem Ipsum, і вдруге - нещодавно завдяки програмам комп'ютерного верстування на кшталт Aldus Pagemaker, які використовували різні версії Lorem Ipsum.";

        var actual = new Translate();
        var result = await actual.DetectLanguageAsync(failures);

        result.Language.Should().BeEquivalentTo("en");
    }
}
