namespace CSharp11;

// 汎用属性を使用する。

// C#10 までの書き方
public class BeforeTypeAttribute : Attribute
{
    public BeforeTypeAttribute(Type t)
    {
        ParamType = t;
    }

    public Type ParamType { get; }
}

public class GenericAttribute<T> : Attribute
{
    public GenericAttribute()
    {
        ParamType = typeof(T);
    }

    public Type ParamType { get; set; }
}

public class TypeAttributeUseClass
{
    [BeforeType(typeof(string))]    // 今まではこっち
    [Generic<string>]               // こんな定義ができる
    public string Method() => default;

    // ただし以下のような使い方はコンパイルエラーになる。
    //[Generic<dynamic>]        // [Generic<object>] ならOK
    //[Generic<string?>]        // null 許容型(null 許容参照型も)は NG [Generic<string>] ならOK
    //[Generic<(int id, string name)>]  // Tuple は NG [Generic<ValueTuple<int, string>>] ならOK
    public string Method2() => default;
}

public class GenericClass<T>
{
    // これもコンパイルエラーになる。
    //[Generic<T>]          // 具体的な型を指定する必要がある。
    public T Method() => default;
}