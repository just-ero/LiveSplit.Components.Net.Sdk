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
                    using LiveSplit.UI.Components;

                    namespace TestNamespace;

                    [Component(
                        Name = "Test Component",
                        Version = "1.0",
                        Author = "John Doe",
                        Category = ComponentCategory.Other)]
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
                    using LiveSplit.UI.Components;

                    namespace TestNamespace;

                    [Component(
                        Name = "Test Component",
                        Version = "1.0",
                        Author = "John Doe",
                        Category = ComponentCategory.Other)]
                    static class TestComponent
                    {

                    }
                    """,
                expected: [
                    Verifier.Diagnostic("LSSDK1000")
                        .WithLocation(7, 2)
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
                    using LiveSplit.UI.Components;

                    namespace TestNamespace;

                    [Component(
                        Name = "Test Component",
                        Version = "1.0",
                        Author = "John Doe",
                        Category = ComponentCategory.Other)]
                    abstract class TestComponent
                        : LiveSplit.Components.Net.Sdk.Testing.ComponentBase
                    {
                        public TestComponent(LiveSplitState state)
                            : base(state) { }
                    }
                    """,
                expected: [
                    Verifier.Diagnostic("LSSDK1000")
                        .WithLocation(7, 2)
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
                    using LiveSplit.UI.Components;

                    namespace TestNamespace;

                    [Component(
                        Name = "Test Component",
                        Version = "1.0",
                        Author = "John Doe",
                        Category = ComponentCategory.Other)]
                    class TestComponent
                        : LiveSplit.Components.Net.Sdk.Testing.ComponentBase
                    {
                        public TestComponent()
                            : base(null) { }
                    }
                    """,
                expected: [
                    Verifier.Diagnostic("LSSDK1001")
                        .WithLocation(7, 2)
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
                    using LiveSplit.UI.Components;

                    namespace TestNamespace;

                    [Component(
                        Name = "Test Component",
                        Version = "1.0",
                        Author = "John Doe",
                        Category = ComponentCategory.Other)]
                    class TestComponent
                    {
                        public TestComponent(LiveSplitState state) { }
                    }
                    """,
                expected: [
                    Verifier.Diagnostic("LSSDK1002")
                        .WithLocation(7, 2)
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
                    using LiveSplit.UI.Components;

                    namespace TestNamespace;

                    [Component(
                        Name = "Test Component",
                        Version = "1.0",
                        Author = "John Doe",
                        Category = ComponentCategory.Other)]
                    abstract class TestComponent
                        : LiveSplit.Components.Net.Sdk.Testing.ComponentBase
                    {
                        protected TestComponent(LiveSplitState state)
                            : base(state) { }
                    }
                    """,
                expected: [
                    Verifier.Diagnostic("LSSDK1000")
                        .WithLocation(7, 2)
                        .WithArguments("TestComponent"),
                    Verifier.Diagnostic("LSSDK1001")
                        .WithLocation(7, 2)
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
                    using LiveSplit.UI.Components;

                    namespace TestNamespace;

                    [Component(
                        Name = "Test Component",
                        Version = "1.0",
                        Author = "John Doe",
                        Category = ComponentCategory.Other)]
                    abstract class TestComponent
                    {
                        public TestComponent(LiveSplitState __) { }
                    }
                    """,
                expected: [
                    Verifier.Diagnostic("LSSDK1000")
                        .WithLocation(7, 2)
                        .WithArguments("TestComponent"),
                    Verifier.Diagnostic("LSSDK1002")
                        .WithLocation(7, 2)
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
                    using LiveSplit.UI.Components;

                    namespace TestNamespace;

                    [Component(
                        Name = "Test Component",
                        Version = "1.0",
                        Author = "John Doe",
                        Category = ComponentCategory.Other)]
                    class TestComponent
                    {
                        public TestComponent() { }
                    }
                    """,
                expected: [
                    Verifier.Diagnostic("LSSDK1001")
                        .WithLocation(7, 2)
                        .WithArguments("TestComponent"),
                    Verifier.Diagnostic("LSSDK1002")
                        .WithLocation(7, 2)
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
                    using LiveSplit.UI.Components;

                    namespace TestNamespace;

                    [Component(
                        Name = "Test Component",
                        Version = "1.0",
                        Author = "John Doe",
                        Category = ComponentCategory.Other)]
                    abstract class TestComponent
                    {
                        public TestComponent() { }
                    }
                    """,
                expected: [
                    Verifier.Diagnostic("LSSDK1000")
                        .WithLocation(7, 2)
                        .WithArguments("TestComponent"),
                    Verifier.Diagnostic("LSSDK1001")
                        .WithLocation(7, 2)
                        .WithArguments("TestComponent"),
                    Verifier.Diagnostic("LSSDK1002")
                        .WithLocation(7, 2)
                        .WithArguments("TestComponent"),
                ])
            .ConfigureAwait(false);
    }

    [Test]
    public async Task Reports1010_ForInvalidVersion()
    {
        await Verifier
            .VerifyAnalyzerAsync(
                source: $$"""
                    using LiveSplit.Components.Sdk;
                    using LiveSplit.Model;
                    using LiveSplit.UI.Components;

                    namespace TestNamespace;

                    [Component(
                        Name = "Test Component",
                        Version = "1",
                        Author = "John Doe",
                        Category = ComponentCategory.Other)]
                    class TestComponent
                        : LiveSplit.Components.Net.Sdk.Testing.ComponentBase
                    {
                        public TestComponent(LiveSplitState state)
                            : base(state) { }
                    }
                    """,
                expected: [
                    Verifier.Diagnostic("LSSDK1010")
                        .WithLocation(9, 15)
                        .WithArguments("1"),
                ])
            .ConfigureAwait(false);
    }

    [Test]
    public async Task ReportsNothing_ForValidVersion()
    {
        await Verifier
            .VerifyAnalyzerAsync(
                source: $$"""
                    using LiveSplit.Components.Sdk;
                    using LiveSplit.Model;
                    using LiveSplit.UI.Components;

                    namespace TestNamespace;

                    [Component(
                        Name = "Test Component",
                        Version = "1.0",
                        Author = "John Doe",
                        Category = ComponentCategory.Other)]
                    class TestComponent
                        : LiveSplit.Components.Net.Sdk.Testing.ComponentBase
                    {
                        public TestComponent(LiveSplitState state)
                            : base(state) { }
                    }
                    """,
                expected: [])
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
