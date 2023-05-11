namespace CSharp09.Tests
{
    using FluentAssertions;
    using Records;
    using Xunit;

    public class UnitTest1
    {
        [Fact]
        public void Record�̔�r()
        {
            var person1 = new Person("John Smith", 20);
            var person2 = new Person("John Smith", 20);

            // �����I�ɐ������ꂽ Equals ���Ă΂��̂ŁA�ȉ��͂ǂ���e�X�g��������B
            person1.Equals(person2).Should().BeTrue();
            person1.Should().Be(person2);
            (person1 == person2).Should().BeTrue();
        }
    }
}