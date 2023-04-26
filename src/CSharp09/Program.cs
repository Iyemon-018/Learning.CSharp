// See https://aka.ms/new-console-template for more information

// 以下の手順を試す。
// cf. https://learn.microsoft.com/ja-jp/dotnet/csharp/roslyn-sdk/source-generators-overview#get-started-with-source-generators

using System;

internal class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
    }
    static partial void HelloFrom(string name);
}
