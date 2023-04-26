namespace SourceGenerator
{
    using Microsoft.CodeAnalysis;

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
        public SampleGenerator()
        {
            System.Diagnostics.Debugger.Launch();
        }

        /// <summary>
        /// Called to initialize the generator and register generation steps via callbacks
        /// on the <paramref name="context" />
        /// </summary>
        /// <param name="context">The <see cref="T:Microsoft.CodeAnalysis.IncrementalGeneratorInitializationContext" /> to register callbacks on</param>
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            //
            context.RegisterPostInitializationOutput(static context =>
            {
                context.AddSource("SampleGenerated.g.cs", "// test");
            });
        }
    }
}