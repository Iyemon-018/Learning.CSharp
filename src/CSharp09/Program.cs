// See https://aka.ms/new-console-template for more information

// 以下の手順を試す。
// cf. https://learn.microsoft.com/ja-jp/dotnet/csharp/roslyn-sdk/source-generators-overview#get-started-with-source-generators
// こっちも参考になりそう。
// 以下には SourceGenerator のデバッグ手順が記載されているので、こちらは必ず設定すること。
// cf. https://neue.cc/2022/12/16_IncrementalSourceGenerator.html

using CSharp09;
using Csharp09.Nested;

namespace CSharp09
{
    using System;
    using System.Linq;
    using Init;
    using Records;

    public static partial class Program
    {
        public static void Main(string[] args)
        {
            HelloFrom("Generated Code");

            Console.WriteLine(new Hello{Name = "John Smith", Age = 33}.ToString());

            var person = new Person("John Smith", 20);
            Console.WriteLine(person);  // Person { Name = John Smith, Age = 20 }

            var people = Enumerable.Range(1, 3).Select(x => new Person($"No.{x}", x * 10));
            Console.WriteLine(string.Join("/", people.Select(x => x.ToString())));  // Person { Name = No.1, Age = 10 }/Person { Name = No.2, Age = 20 }/Person { Name = No.3, Age = 30 }

            var user = new User(1234, "John Smith", 29, new Birthday(new DateTime(2020, 2, 19)));
            Console.WriteLine(user);    // User { Id = 1234, Birthday = Birthday { Value = 2020/02/19 0:00:00 } }

            var wo = new WeatherObservation
                     {
                         RecordedAt           = DateTime.Now
                       , PressureInMillibars  = 100
                       , TemperatureInCelsius = 10000
                     };
            Console.WriteLine(wo);
        }

        static partial void HelloFrom(string name);
    }

}

namespace Csharp09.Nested
{
    [GenerateToString]
    public partial class Hello
    {
        public string Name { get; set; }

        public int Age { get; set; }
    }
}

namespace CSharp09.Records
{
    using System;

    public record Person(string Name, int Age);

    public record Birthday(DateTime Value);

    // record 型をメンバに持つ record 型も作ることができる。
    // record 型であれば継承もできる。クラスに継承することはできない。  
    public record User(int Id, string Name, int Age, Birthday Birthday): Person(Name, Age);
}

namespace CSharp09.Init
{
    using System;

    // cf. https://learn.microsoft.com/ja-jp/dotnet/csharp/whats-new/csharp-9#init-only-setters
    // プロパティの setter に init を付けることで、プロパティ初期化子で設定した値の再代入ができなくなる。
    // これまではコンストラクタから代入する必要があったが、その必要性がなくなった。
    public record WeatherObservation
    {
        public DateTime RecordedAt { get; init; }

        public decimal TemperatureInCelsius { get; init; }

        public decimal PressureInMillibars { get; init; }
    }
}