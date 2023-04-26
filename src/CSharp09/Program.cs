// See https://aka.ms/new-console-template for more information

// 以下の手順を試す。
// cf. https://learn.microsoft.com/ja-jp/dotnet/csharp/roslyn-sdk/source-generators-overview#get-started-with-source-generators
// こっちも参考になりそう。
// 以下には SourceGenerator のデバッグ手順が記載されているので、こちらは必ず設定すること。
// cf. https://neue.cc/2022/12/16_IncrementalSourceGenerator.html
namespace CSharp09
{
    public static partial class Program
    {
        public static void Main(string[] args)
        {
            HelloFrom("Generated Code");
        }

        static partial void HelloFrom(string name);
    }
}