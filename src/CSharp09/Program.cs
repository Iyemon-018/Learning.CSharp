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

    public static partial class Program
    {
        public static void Main(string[] args)
        {
            HelloFrom("Generated Code");

            Console.WriteLine(new Hello{Name = "John Smith", Age = 33}.ToString());
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