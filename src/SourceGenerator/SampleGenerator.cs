namespace SourceGenerator
{
    using System;
    using System.Linq;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// 以下をベースに作成している。
    /// cf. https://neue.cc/2022/12/16_IncrementalSourceGenerator.html
    /// </remarks>
    [Generator(LanguageNames.CSharp)]
    public class SampleGenerator : IIncrementalGenerator
    {
        /// <summary>
        /// Called to initialize the generator and register generation steps via callbacks
        /// on the <paramref name="context" />
        /// </summary>
        /// <param name="context">The <see cref="T:Microsoft.CodeAnalysis.IncrementalGeneratorInitializationContext" /> to register callbacks on</param>
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            // メソッドを自動生成する対象のクラスを識別する必要がある。
            // 属性を自動生成して、生成した属性を定義したものを自動生成の対象とすることで生成対象を明確にする方法が一般的。
            // こうしなければ無差別的に全ソースを対象としてしまって、開発効率の低下につながってしまうため。
            context.RegisterPostInitializationOutput(static context =>
            {
                context.AddSource("SampleGeneratedAttribute.g.cs"
                      , @"
namespace CSharp09
{
    using System;

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    internal sealed class GenerateToStringAttribute : Attribute
    {
    }
}
");
            });
            
            // ForAttributeWithMetadataName で指定した属性を持つソースを取得することができる。
            var source = context.SyntaxProvider.ForAttributeWithMetadataName("CSharp09.GenerateToStringAttribute"
                  , static (node,    token) => true
                  , static (context, token) => context);

            // 生成するには Visual Studio の再起動が必要になる。（めんどい）
            context.RegisterSourceOutput(source, Emit);
        }

        /// <summary>
        /// 自動生成を行う。
        /// </summary>
        /// <param name="context"></param>
        /// <param name="source"><seealso cref="Initialize"/> で呼び出した ForAttributeWithMetadataName で取得したソースのシンボル情報</param>
        private static void Emit(SourceProductionContext context, GeneratorAttributeSyntaxContext source)
        {
            // INamedTypeSymbol は ISymbol の系統になる。これは対象コードがコンパイルされた状態の情報が格納されている。
            // TypeDeclarationSyntax は SymbolNode の系統になる。文字列としてのコードの情報が入っていて、警告やコンパイルエラーの破線を出す際にはこっちが必要になる。
            var typeSymbol = (INamedTypeSymbol) source.TargetSymbol;
            var typeNode   = (TypeDeclarationSyntax) source.TargetNode;

            if (typeSymbol.GetMembers("ToString").Length != 0)
            {
                // すでに ToString() が実装されていれば自動生成できないので、コンパイルエラーにする。
                context.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.ExistsOverrideToString
                      , typeNode.Identifier.GetLocation()
                      , typeSymbol.Name));
                return;
            }

            // グローバル名前空間の対応。
            // ここはちょっとよくわからない。
            // typeSymbol.ContainingNamespace.IsGlobalNamespace はいつ true になる？
            var ns = typeSymbol.ContainingNamespace.IsGlobalNamespace
                    ? string.Empty
                    : $"namespace {typeSymbol.ContainingNamespace}";

            // typeSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat) で "global::Csharp09.Nested.Hello" といった文字列が取れる。
            // fully qualified format: 完全修飾形式（"名前空間.クラス名.メソッド名"で表される。今回は、typeSymbol がクラスの型なので"名前空間.クラス名"となる。）
            // これをファイル名として使用するための変更が以下の内容になる。
            // 完全修飾形式だと先頭に"global::"が付く。"::"の部分はファイル名としては使用できないのと、"global"は名前に含める必要性がない。
            // "<"と">"は、ジェネリクスの場合に含まれる。どちらもファイル名としては使えないので変更している。
            var fullType = typeSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)
                                     .Replace("global::", "")
                                     .Replace("<", "_")
                                     .Replace(">", "_");

            // ToString() で戻り値となるフィールドのフォーマットを定義している。
            var publicMembers = typeSymbol.GetMembers()
                                          .Where(x => x is (IFieldSymbol or IPropertySymbol)
                                                           and
                                                           {
                                                               IsStatic             : false
                                                             , DeclaredAccessibility: Accessibility.Public
                                                             , IsImplicitlyDeclared : false
                                                             , CanBeReferencedByName: true
                                                           })
                                          .Select(x => $"{x.Name}:{{{x.Name}}}");
            var toString = string.Join(", ", publicMembers);

            // ここが生成対象となるコード部分。C#11 だと Raw String Literals のお陰でもうちょっと書きやすくなってる。
            // この程度なら文字列で定義してもいいが、複雑なコードを生成する場合は手段（コードで実現するか T4 テンプレートを使うか）を考える必要がある。
            var code = $"// <auto-generated/>\r\n"
                     + $"#nullable enable\r\n\r\n"
                     + $"{ns}\r\n"
                     + $"{{\r\n"
                     + $"    partial class {typeSymbol.Name}\r\n"
                     + $"    {{\r\n"
                     + $"        public override string ToString()\r\n"
                     + $"        {{\r\n"
                     + $"            return $\"{toString}\";\r\n"
                     + $"        }}\r\n"
                     + $"    }}\r\n"
                     + $"}}\r\n";

            context.AddSource($"{fullType}.SampleGenerator.g.cs", code);
        }
    }

    public static class DiagnosticDescriptors
    {
        const string Category = "SampleGenerator";

        public static readonly DiagnosticDescriptor ExistsOverrideToString = new(
                id: "SAMPLE001",
                title: "ToString override",
                messageFormat: "The GenerateToString class '{0}' has ToString override but it is not allowed.",
                category: Category,
                defaultSeverity: DiagnosticSeverity.Error,
                isEnabledByDefault: true);
    }
}