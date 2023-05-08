using CSharp09;

namespace SourceGenerator.Tests;

public class SampleGeneratorTest
{
    [Fact]
    public void ToString_Šî–{Œ`()
    {
        var sut = new MyClass { Text = "hoge", Value = 12, Child = new() { Text = "fuga", Value = 92 } };

        sut.ToString().Should().Be("Text:hoge, Value:12, Child:Text:fuga, Value:92, Child:");
    }
}

[GenerateToString]
public partial class MyClass
{
    public string Text { get; set; }

    public int Value { get; set; }

    public MyClass Child { get; set; }
}