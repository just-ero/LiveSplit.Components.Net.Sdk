using System.Collections.Immutable;

using Microsoft.CodeAnalysis;

namespace LiveSplit.Components.Net.Sdk.Analyzers;

internal partial class ComponentFactoryAnalyzer
{
    private static readonly DiagnosticDescriptor _rule1000 = new(
        "LSSDK1000",
        "MustBeNonAbstractNonStatic",
        "'{0}' must be a non-abstract, non-static type",
        "ComponentFactoryGenerator",
        DiagnosticSeverity.Error,
        isEnabledByDefault: true);

    private static readonly DiagnosticDescriptor _rule1001 = new(
        "LSSDK1001",
        "MustHaveRequiredConstructor",
        "'{0}' must contain a public or internal constructor accepting a single argument of type 'LiveSplit.Model.LiveSplitState'",
        "ComponentFactoryGenerator",
        DiagnosticSeverity.Error,
        isEnabledByDefault: true);

    private static readonly DiagnosticDescriptor _rule1002 = new(
        "LSSDK1002",
        "MustImplementIComponent",
        "'{0}' must implement 'LiveSplit.UI.Components.IComponent'",
        "ComponentFactoryGenerator",
        DiagnosticSeverity.Error,
        isEnabledByDefault: true);

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => [
        _rule1000,
        _rule1001,
        _rule1002];
}
