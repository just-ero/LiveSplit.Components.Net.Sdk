#pragma warning disable CA1062 // Validate arguments of public methods

using System;
using System.Text;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Text;

namespace LiveSplit.Components.Net.Sdk.Testing;

public abstract class IncrementalGeneratorTest<TGenerator>
    : CSharpSourceGeneratorTest<TGenerator, DefaultVerifier>
    where TGenerator : IIncrementalGenerator, new()
{
    protected IncrementalGeneratorTest()
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

    internal void AddGeneratedSource(string fileName, string content)
    {
        TestState.GeneratedSources.Add((typeof(TGenerator), fileName, SourceText.From(content, Encoding.UTF8)));
    }
}
