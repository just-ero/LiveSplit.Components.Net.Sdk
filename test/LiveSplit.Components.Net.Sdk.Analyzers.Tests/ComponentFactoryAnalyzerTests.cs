using System.Threading.Tasks;

using LiveSplit.Components.Net.Sdk.Testing;
using LiveSplit.Components.Sdk;

using NUnit.Framework;

using Verifier = LiveSplit.Components.Net.Sdk.Testing.AnalyzerVerifier<
    LiveSplit.Components.Net.Sdk.Analyzers.ComponentFactoryAnalyzer,
    LiveSplit.Components.Net.Sdk.Analyzers.Tests.ComponentFactoryAnalyzerTest>;

namespace LiveSplit.Components.Net.Sdk.Analyzers.Tests;

public sealed class ComponentFactoryAnalyzerTests
{
    [Test]
    public async Task ReportsNothing_ForOptionalParameters()
    {
        await Verifier
            .VerifyAnalyzerAsync(
                source: $$"""
                    using LiveSplit.Components.Sdk;
                    using LiveSplit.Model;

                    namespace TestNamespace;

                    [Component(Name = default, Author = default, Version = default, Category = default)]
                    class TestComponent
                        : LiveSplit.Components.Net.Sdk.Testing.ComponentBase
                    {
                        public TestComponent(LiveSplitState state, int optional = 0)
                            : base(state) { }
                    }
                    """,
                expected: [])
            .ConfigureAwait(false);
    }

    [Test]
    public async Task Reports1000_ForStaticClass()
    {
        await Verifier
            .VerifyAnalyzerAsync(
                source: $$"""
                    using LiveSplit.Components.Sdk;
                    using LiveSplit.Model;

                    namespace TestNamespace;

                    [Component(Name = default, Author = default, Version = default, Category = default)]
                    static class TestComponent
                    {

                    }
                    """,
                expected: [
                    Verifier.Diagnostic("LSSDK1000")
                    .WithLocation(6, 2)
                    .WithArguments("TestComponent"),
                ])
            .ConfigureAwait(false);
    }

    [Test]
    public async Task Reports1000_ForAbstractClass()
    {
        await Verifier
            .VerifyAnalyzerAsync(
                source: $$"""
                    using LiveSplit.Components.Sdk;
                    using LiveSplit.Model;

                    namespace TestNamespace;

                    [Component(Name = default, Author = default, Version = default, Category = default)]
                    abstract class TestComponent
                        : LiveSplit.Components.Net.Sdk.Testing.ComponentBase
                    {
                        public TestComponent(LiveSplitState state)
                            : base(state) { }
                    }
                    """,
                expected: [
                    Verifier.Diagnostic("LSSDK1000")
                    .WithLocation(6, 2)
                    .WithArguments("TestComponent"),
                ])
            .ConfigureAwait(false);
    }

    [Test]
    public async Task Reports1001_ForMissingCtor()
    {
        await Verifier
            .VerifyAnalyzerAsync(
                source: $$"""
                    using LiveSplit.Components.Sdk;
                    using LiveSplit.Model;

                    namespace TestNamespace;

                    [Component(Name = default, Author = default, Version = default, Category = default)]
                    class TestComponent
                        : LiveSplit.Components.Net.Sdk.Testing.ComponentBase
                    {
                        public TestComponent()
                            : base(null) { }
                    }
                    """,
                expected: [
                    Verifier.Diagnostic("LSSDK1001")
                    .WithLocation(6, 2)
                    .WithArguments("TestComponent"),
                ])
            .ConfigureAwait(false);
    }

    [Test]
    public async Task Reports1002_ForMissingIComponent()
    {
        await Verifier
            .VerifyAnalyzerAsync(
                source: $$"""
                    using LiveSplit.Components.Sdk;
                    using LiveSplit.Model;

                    namespace TestNamespace;

                    [Component(Name = default, Author = default, Version = default, Category = default)]
                    class TestComponent
                    {
                        public TestComponent(LiveSplitState state) { }
                    }
                    """,
                expected: [
                    Verifier.Diagnostic("LSSDK1002")
                    .WithLocation(6, 2)
                    .WithArguments("TestComponent"),
                ])
            .ConfigureAwait(false);
    }

    [Test]
    public async Task Reports10001001_ForAbstractAndMissingCtor()
    {
        await Verifier
            .VerifyAnalyzerAsync(
                source: $$"""
                    using LiveSplit.Components.Sdk;
                    using LiveSplit.Model;

                    namespace TestNamespace;

                    [Component(Name = default, Author = default, Version = default, Category = default)]
                    abstract class TestComponent
                        : LiveSplit.Components.Net.Sdk.Testing.ComponentBase
                    {
                        protected TestComponent(LiveSplitState state)
                            : base(state) { }
                    }
                    """,
                expected: [
                    Verifier.Diagnostic("LSSDK1000")
                    .WithLocation(6, 2)
                    .WithArguments("TestComponent"),
                    Verifier.Diagnostic("LSSDK1001")
                    .WithLocation(6, 2)
                    .WithArguments("TestComponent"),
                ])
            .ConfigureAwait(false);
    }

    [Test]
    public async Task Reports10001002_ForAbstractAndMissingIComponent()
    {
        await Verifier
            .VerifyAnalyzerAsync(
                source: $$"""
                    using LiveSplit.Components.Sdk;
                    using LiveSplit.Model;

                    namespace TestNamespace;

                    [Component(Name = default, Author = default, Version = default, Category = default)]
                    abstract class TestComponent
                    {
                        public TestComponent(LiveSplitState __) { }
                    }
                    """,
                expected: [
                    Verifier.Diagnostic("LSSDK1000")
                    .WithLocation(6, 2)
                    .WithArguments("TestComponent"),
                    Verifier.Diagnostic("LSSDK1002")
                    .WithLocation(6, 2)
                    .WithArguments("TestComponent"),
                ])
            .ConfigureAwait(false);
    }

    [Test]
    public async Task Reports10011002_ForMissingCtorAndIComponent()
    {
        await Verifier
            .VerifyAnalyzerAsync(
                source: $$"""
                    using LiveSplit.Components.Sdk;
                    using LiveSplit.Model;

                    namespace TestNamespace;

                    [Component(Name = default, Author = default, Version = default, Category = default)]
                    class TestComponent
                    {
                        public TestComponent() { }
                    }
                    """,
                expected: [
                    Verifier.Diagnostic("LSSDK1001")
                    .WithLocation(6, 2)
                    .WithArguments("TestComponent"),
                    Verifier.Diagnostic("LSSDK1002")
                    .WithLocation(6, 2)
                    .WithArguments("TestComponent"),
                ])
            .ConfigureAwait(false);
    }

    [Test]
    public async Task Reports100010011002_ForAbstractAndMissingCtorAndIComponent()
    {
        await Verifier
            .VerifyAnalyzerAsync(
                source: $$"""
                    using LiveSplit.Components.Sdk;
                    using LiveSplit.Model;

                    namespace TestNamespace;

                    [Component(Name = default, Author = default, Version = default, Category = default)]
                    abstract class TestComponent
                    {
                        public TestComponent() { }
                    }
                    """,
                expected: [
                    Verifier.Diagnostic("LSSDK1000")
                    .WithLocation(6, 2)
                    .WithArguments("TestComponent"),
                    Verifier.Diagnostic("LSSDK1001")
                    .WithLocation(6, 2)
                    .WithArguments("TestComponent"),
                    Verifier.Diagnostic("LSSDK1002")
                    .WithLocation(6, 2)
                    .WithArguments("TestComponent"),
                ])
            .ConfigureAwait(false);
    }
}

file sealed class ComponentFactoryAnalyzerTest
    : AnalyzerTest<ComponentFactoryAnalyzer>
{
    public ComponentFactoryAnalyzerTest()
        : base()
    {
        AddReferenceAssembly<System.Drawing.Graphics>();
        AddReferenceAssembly<System.Windows.Forms.Form>();
        AddReferenceAssembly<UI.Components.IComponent>();

        AddReferenceAssembly<ComponentAttribute>();
        AddReferenceAssembly<ComponentBase>();
    }
}
