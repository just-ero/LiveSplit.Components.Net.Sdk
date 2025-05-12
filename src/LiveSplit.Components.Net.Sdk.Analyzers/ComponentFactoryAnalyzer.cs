using System.Collections.Immutable;
using System.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace LiveSplit.Components.Net.Sdk.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
internal sealed partial class ComponentFactoryAnalyzer
    : DiagnosticAnalyzer
{
    private const string AttributeMetadataName = "LiveSplit.Components.Sdk.ComponentAttribute";

    private const string InterfaceMetadataName = "LiveSplit.UI.Components.IComponent";
    private const string ArgMetadataName = "LiveSplit.Model.LiveSplitState";

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

    public override void Initialize(AnalysisContext context)
    {
        context.EnableConcurrentExecution();
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);

        context.RegisterSymbolAction(Analyze, SymbolKind.NamedType);
    }

    private static void Analyze(SymbolAnalysisContext context)
    {
        if (context.Symbol is not INamedTypeSymbol symbol)
        {
            return;
        }

        if (GetAttribute(context) is not { } attribute)
        {
            return;
        }

        if (symbol.IsAbstract || symbol.IsStatic)
        {
            var diagnostic = Diagnostic.Create(
                _rule1000,
                attribute.GetLocation(),
                context.Symbol.Name);

            context.ReportDiagnostic(diagnostic);

            if (symbol.IsStatic)
            {
                return;
            }
        }

        if (!HasRequiredConstructor(context, symbol))
        {
            var diagnostic = Diagnostic.Create(
                _rule1001,
                attribute.GetLocation(),
                context.Symbol.Name);

            context.ReportDiagnostic(diagnostic);
        }

        if (!ImplementsIComponent(context, symbol))
        {
            var diagnostic = Diagnostic.Create(
                _rule1002,
                attribute.GetLocation(),
                context.Symbol.Name);

            context.ReportDiagnostic(diagnostic);
        }
    }

    private static AttributeSyntax? GetAttribute(SymbolAnalysisContext context)
    {
        if (context.Compilation.GetTypeByMetadataName(AttributeMetadataName) is not { } attributeSymbol)
        {
            return null;
        }

        var attribute = context
            .Symbol
            .GetAttributes()
            .FirstOrDefault(a => SymbolEqualityComparer.Default.Equals(a.AttributeClass, attributeSymbol));

        if (attribute is null)
        {
            return null;
        }

        return attribute
            .ApplicationSyntaxReference?
            .GetSyntax(context.CancellationToken) as AttributeSyntax;
    }

    private static bool HasRequiredConstructor(SymbolAnalysisContext context, INamedTypeSymbol symbol)
    {
        if (context.Compilation.GetTypeByMetadataName(ArgMetadataName) is not { } argSymbol)
        {
            return false;
        }

        return symbol
            .InstanceConstructors
            .Any(ctor =>
            {
                return ctor.DeclaredAccessibility is Accessibility.Internal or Accessibility.ProtectedOrInternal or Accessibility.Public
                    && ctor.Parameters is [{ Type: { } paramType }, ..]
                    && SymbolEqualityComparer.Default.Equals(paramType, argSymbol)
                    && ctor.Parameters.Skip(1).All(p => p.IsOptional);
            });
    }

    private static bool ImplementsIComponent(SymbolAnalysisContext context, INamedTypeSymbol symbol)
    {
        if (context.Compilation.GetTypeByMetadataName(InterfaceMetadataName) is not { } interfaceSymbol)
        {
            return false;
        }

        return symbol
            .AllInterfaces
            .Any(i => SymbolEqualityComparer.Default.Equals(i, interfaceSymbol));
    }
}
