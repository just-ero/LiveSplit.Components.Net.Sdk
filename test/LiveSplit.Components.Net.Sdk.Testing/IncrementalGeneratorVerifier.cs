#pragma warning disable CA1000 // Do not declare static members on generic types
#pragma warning disable CA1062 // Validate arguments of public methods

using System.Threading.Tasks;

using Microsoft.CodeAnalysis;

namespace LiveSplit.Components.Net.Sdk.Testing;

public static class IncrementalGeneratorVerifier<TGenerator, TTest>
    where TGenerator : IIncrementalGenerator, new()
    where TTest : IncrementalGeneratorTest<TGenerator>, new()
{
    public static async Task VerifyGeneratorAsync(string source, (string FileName, string ExpectedContent)[] expected)
    {
        TTest test = new();

        test.AddSource(source);

        foreach ((string fileName, string expectedContent) in expected)
        {
            test.AddGeneratedSource(fileName, expectedContent);
        }

        await test.RunAsync().ConfigureAwait(false);
    }
}
