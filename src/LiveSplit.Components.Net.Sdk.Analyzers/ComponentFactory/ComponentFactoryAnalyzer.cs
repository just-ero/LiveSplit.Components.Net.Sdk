using System;
using System.Linq;

using LiveSplit.Components.Net.Sdk.Analyzers.Extensions;

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

        if (attribute.ApplicationSyntaxReference?.GetSyntax(context.CancellationToken) is not AttributeSyntax attributeSyntax)
        {
            return;
        }

        if (symbol.IsAbstract || symbol.IsStatic)
        {
            var diagnostic = Diagnostic.Create(
                _rule1000,
                attributeSyntax.GetLocation(),
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
                attributeSyntax.GetLocation(),
                context.Symbol.Name);

            context.ReportDiagnostic(diagnostic);
        }

        if (!ImplementsIComponent(context, symbol))
        {
            var diagnostic = Diagnostic.Create(
                _rule1002,
                attributeSyntax.GetLocation(),
                context.Symbol.Name);

            context.ReportDiagnostic(diagnostic);
        }

        if (attribute.TryGetNamedArgument("Version", out string? version))
        {
            if (!Version.TryParse(version, out _))
            {
                var argument = attributeSyntax.ArgumentList!.Arguments
                    .First(a => a.NameEquals!.Name.Identifier.Text == "Version")
                    .Expression.GetLocation();

                var diagnostic = Diagnostic.Create(
                    _rule1010,
                    argument,
                    version);

                context.ReportDiagnostic(diagnostic);
            }
        }
    }

    private static AttributeData? GetAttribute(SymbolAnalysisContext context)
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

        return attribute;
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
