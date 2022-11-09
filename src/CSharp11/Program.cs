// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");

// cf.
//    https://learn.microsoft.com/ja-jp/dotnet/csharp/whats-new/csharp-11
//    https://ufcpp.net/study/csharp/cheatsheet/ap_ver11/

using System.Reflection;
using CSharp11;

var attributes = typeof(TypeAttributeUseClass)
                .GetMethod(nameof(TypeAttributeUseClass.Method))
                .GetCustomAttributes();
Console.WriteLine($"{typeof(BeforeTypeAttribute)}: {attributes.OfType<BeforeTypeAttribute>().First().ParamType}");
Console.WriteLine($"{typeof(GenericAttribute<>)}: {attributes.OfType<GenericAttribute<string>>().First().ParamType}");

// 文字列補間中の改行(Newlines in string interpolations)
// 今までだと $"{}" で{}の補間をする際は改行ができなかった。
// なので以下のような書き方になっていた。
Console.WriteLine($"(Before) Now:{DateTime.Now:yyyy-MM-dd HH:mm:ss}");
// C#11 からは以下のように {} 内で改行することができる。
Console.WriteLine($"(C#11) Now:{
    DateTime.Now
}");
// 本来の目的は以下のように LINQ を使うような場面で改行したほうが可読性が高いよね、という話らしい。
Console.WriteLine($"{
    new[] { 1, 2, 3 }.Select(x => x * 2)
                   .Select(x => $"{x}")
                   .ToArray()
}");
// ただし、以下のような書式設定は使えない。書式設定使う場合は1行にする必要がある。
//Console.WriteLine($"(C#11) Now:{
//    DateTime.Now:yyyy-MM-dd
//}");
// 
// C#10 でも $@ を使うことで {} 内で改行することはできていた。けど、$ だけで改行できる方が楽。
Console.WriteLine($@"(C#10) Now:{
    DateTime.Now
}");

