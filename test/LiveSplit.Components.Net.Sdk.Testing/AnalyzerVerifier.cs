#pragma warning disable CA1000 // Do not declare static members on generic types
#pragma warning disable CA1062 // Validate arguments of public methods

using System.Threading.Tasks;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Testing;

namespace LiveSplit.Components.Net.Sdk.Testing;

public static class AnalyzerVerifier<TAnalyzer, TTest>
    where TAnalyzer : DiagnosticAnalyzer, new()
    where TTest : AnalyzerTest<TAnalyzer>, new()
{
    public static DiagnosticResult Diagnostic()
    {
        return CSharpAnalyzerVerifier<TAnalyzer, DefaultVerifier>.Diagnostic();
    }

    public static DiagnosticResult Diagnostic(string diagnosticId)
    {
        return CSharpAnalyzerVerifier<TAnalyzer, DefaultVerifier>.Diagnostic(diagnosticId);
    }

    public static DiagnosticResult Diagnostic(DiagnosticDescriptor descriptor)
    {
        return CSharpAnalyzerVerifier<TAnalyzer, DefaultVerifier>.Diagnostic(descriptor);
    }

    public static async Task VerifyAnalyzerAsync(string source, DiagnosticResult[] expected)
    {
        TTest test = new();

        test.AddSource(source);

        foreach (DiagnosticResult diagnostic in expected)
        {
            test.AddExpectedDiagnostic(diagnostic);
        }

        await test.RunAsync().ConfigureAwait(false);
    }
}
