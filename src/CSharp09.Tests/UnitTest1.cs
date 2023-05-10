namespace CSharp09.Tests
{
    using FluentAssertions;
    using Records;
    using Xunit;

    public class UnitTest1
    {
        [Fact]
        public void Recordの比較()
        {
            var person1 = new Person("John Smith", 20);
            var person2 = new Person("John Smith", 20);

            // 内部的に生成された Equals が呼ばれるので、以下はどれもテスト成功する。
            person1.Equals(person2).Should().BeTrue();
            person1.Should().Be(person2);
            (person1 == person2).Should().BeTrue();
        }
    }
}