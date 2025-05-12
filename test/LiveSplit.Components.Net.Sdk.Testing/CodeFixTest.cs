using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Testing;

namespace LiveSplit.Components.Net.Sdk.Testing;

public abstract class CodeFixTest<TAnalyzer, TCodeFix>
    : CSharpCodeFixTest<TAnalyzer, TCodeFix, DefaultVerifier>
    where TAnalyzer : DiagnosticAnalyzer, new()
    where TCodeFix : CodeFixProvider, new()
{
    protected CodeFixTest()
    {
        ReferenceAssemblies = ReferenceAssemblies.Net.Net90;
    }
}
