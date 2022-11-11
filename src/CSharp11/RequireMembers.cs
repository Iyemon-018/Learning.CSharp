namespace CSharp11;

using System.Diagnostics.CodeAnalysis;

// 必須メンバー

// C#10 まで
// 例えば Person クラスのような init アクセサのプロパティは、オブジェクト初期化子で値を設定できる。
// ただし、規定のコンストラクタを呼び出した場合、以下のケースでは FirstName プロパティの値がデフォルト値になる。
// 初期化されずにプロパティが呼び出せてしまう状況になるため、危険なコードになる。

public class Person
{
    public Person()
    {
        
    }

    public Person(string firstName)
    {
        FirstName = firstName;
    }
    
    public string FirstName { get; init; }
}

// C#11 から
// 以下のように init アクセサのプロパティに required 修飾子をつける。
// required 修飾子を付けたプロパティはすべてオブジェクト初期化子で値を設定しなければコンパイルエラーになる。
// 要は、オブジェクト初期化子を使って初期化を明示したい場合に有効な手段と言える。
// required を使えば、プロパティを初期化したい場合にコンストラクタの引数をどんどん増やす必要がない（と言うかコンストラクタがいらない）。
//
// SetsRequiredMembers は定義したコンストラクタが required を使用したプロパティをすべて初期化している、というマーク属性になっている。
// SetsRequiredMembers を定義していないコンストラクタでは、required のプロパティが初期化されていないものと判断される。
// そうなった場合、以下の例ではデフォルトコンストラクタを呼び出すと required プロパティが初期化されていないと判断され、コンパイルエラーになる。

public class Person2
{
    public Person2()
    {
    }

    [SetsRequiredMembers]
    public Person2(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName  = lastName;
    }

    public required string FirstName { get; init; }

    public required string LastName { get; init; }
}