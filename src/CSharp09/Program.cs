// See https://aka.ms/new-console-template for more information

// 以下の手順を試す。
// cf. https://learn.microsoft.com/ja-jp/dotnet/csharp/roslyn-sdk/source-generators-overview#get-started-with-source-generators

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