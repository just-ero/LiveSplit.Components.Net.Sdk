#pragma warning disable CA1062 // Validate arguments of public methods

using System;

using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Testing;

namespace LiveSplit.Components.Net.Sdk.Testing;

public abstract class AnalyzerTest<TAnalyzer>
    : CSharpAnalyzerTest<TAnalyzer, DefaultVerifier>
    where TAnalyzer : DiagnosticAnalyzer, new()
{
    protected AnalyzerTest()
    {
        ReferenceAssemblies = ReferenceAssemblies.Net.Net90;
    }

    protected void AddReferenceAssembly<T>()
    {
        TestState.AdditionalReferences.Add(typeof(T).Assembly);
    }

    protected void AddReferenceAssembly(Type type)
    {
        TestState.AdditionalReferences.Add(type.Assembly);
    }

    internal void AddSource(string source)
    {
        TestState.Sources.Add(source);
    }

    internal void AddExpectedDiagnostic(DiagnosticResult expected)
    {
        TestState.ExpectedDiagnostics.Add(expected);
    }
}
