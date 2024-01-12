using DropWord.Application.Common.Models.Sentence;
using DropWord.Infrastructure.Sentence;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace DropWord.Application.UnitTests.Sentence;

public class ParseTests
{
    [Test]
    public async Task ParseSingleSentenceTest()
    {
        var failures =
            "Lorem ipsum dolor sit amet consectetur adipiscing elit eget, lobortis curae ornare torquent class urna neque";
        var fileName = "appsettings.json";
        IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile(fileName, optional: true, reloadOnChange: true)
            .Build();
        var actual = new Parse(configuration);

        var result = await actual.ParseAsync(failures);
        result.Should().BeEquivalentTo(new List<ParseSentenceModel>()
        {
            new ParseSentenceModel() { Sentence = failures }
        });
    }

    [Test]
    public async Task ParseSentencesListTest()
    {
        var failures =
            "Lorem ipsum dolor sit amet consectetur adipiscing elit eget, lobortis curae ornare torquent class urna neque. Hac parturient mi sapien maecenas scelerisque dolor lacus mattis urna, convallis ac magna donec quam adipiscing fames netus, amet tortor fringilla dui velit feugiat suspendisse molestie. Nam ligula eleifend mus bibendum ridiculus rutrum, volutpat platea tempor torquent sem et dolor, mattis hac per tellus posuere.";
        var fileName = "D:\\Project\\C#\\DropWord\\src\\TgBot\\appsettings.json";
        IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile(fileName, optional: true, reloadOnChange: true)
            .Build();
        var actual = new Parse(configuration);

        var result = await actual.ParseAsync(failures);
        result.Should().BeEquivalentTo(new List<ParseSentenceModel>()
        {
            new ParseSentenceModel() { Sentence = "Lorem ipsum dolor sit amet consectetur adipiscing elit eget, lobortis curae ornare torquent class urna neque" },
            new ParseSentenceModel() { Sentence = "Hac parturient mi sapien maecenas scelerisque dolor lacus mattis urna, convallis ac magna donec quam adipiscing fames netus, amet tortor fringilla dui velit feugiat suspendisse molestie" },
            new ParseSentenceModel() { Sentence = "Nam ligula eleifend mus bibendum ridiculus rutrum, volutpat platea tempor torquent sem et dolor, mattis hac per tellus posuere" },
            
        });
    }
}
