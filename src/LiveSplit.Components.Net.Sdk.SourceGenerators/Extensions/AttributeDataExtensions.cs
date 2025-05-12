using System.Diagnostics.CodeAnalysis;

using Microsoft.CodeAnalysis;

namespace LiveSplit.Components.Net.Sdk.SourceGenerators.Extensions;

internal static class AttributeDataExtensions
{
    public static bool TryGetNamedArgument<T>(this AttributeData attribute, string name, [NotNullWhen(true)] out T? value)
    {
        foreach (var argument in attribute.NamedArguments)
        {
            if (argument.Key == name && argument.Value.Value is T t)
            {
                value = t;
                return true;
            }
        }

        value = default!;
        return false;
    }
}
