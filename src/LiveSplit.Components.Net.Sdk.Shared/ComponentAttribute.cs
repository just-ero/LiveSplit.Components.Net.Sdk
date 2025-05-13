using System;
using System.Diagnostics.CodeAnalysis;

using LiveSplit.UI.Components;

namespace LiveSplit.Components.Sdk;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public sealed class ComponentAttribute : Attribute
{
    public required string Author { get; init; }
    public required string Name { get; init; }
    public required string Version { get; init; }
    public required ComponentCategory Category { get; init; }

    [field: AllowNull]
    public string Description
    {
        get => field ?? "";
        init;
    }

    [field: AllowNull]
    public string RepositoryUrl
    {
        get => field ?? $"https://github.com/{Author}/{Name}";
        init;
    }

    [field: AllowNull]
    public string UpdateUrl
    {
        get => field ?? $"{RepositoryUrl}/releases/latest/download";
        init;
    }
}
