using System;
using System.Collections.Generic;
using System.Drawing;
using System.Xml;

using LiveSplit.Components.Sdk;
using LiveSplit.Model;
using LiveSplit.UI;
using LiveSplit.UI.Components;

namespace LiveSplit.Components.Net.Sdk.Sample;

[Component(
    Author = "Your Name",
    Version = "1.0.0",
    Name = "Sample Component",
    Category = ComponentCategory.Timer
)]
#pragma warning disable CA1063 // Implement IDisposable Correctly
public sealed class SampleComponent : IComponent
#pragma warning restore CA1063 // Implement IDisposable Correctly
{
    public SampleComponent(LiveSplitState state)
    {
        // Constructor logic here
    }

    string IComponent.ComponentName => "Sample Component";

    float IComponent.HorizontalWidth => default;

    float IComponent.MinimumHeight => default;

    float IComponent.VerticalHeight => default;

    float IComponent.MinimumWidth => default;

    float IComponent.PaddingTop => default;

    float IComponent.PaddingBottom => default;

    float IComponent.PaddingLeft => default;

    float IComponent.PaddingRight => default;

    IDictionary<string, Action> IComponent.ContextMenuControls => new Dictionary<string, Action>();

#pragma warning disable CA1063 // Implement IDisposable Correctly
#pragma warning disable CA1816 // Dispose methods should call SuppressFinalize
    void IDisposable.Dispose()
#pragma warning restore CA1816 // Dispose methods should call SuppressFinalize
#pragma warning restore CA1063 // Implement IDisposable Correctly
    {

    }

    void IComponent.DrawHorizontal(Graphics g, LiveSplitState state, float height, Region clipRegion)
    {

    }

    void IComponent.DrawVertical(Graphics g, LiveSplitState state, float width, Region clipRegion)
    {

    }

#pragma warning disable CA1033 // Interface methods should be callable by child types
    XmlNode? IComponent.GetSettings(XmlDocument document)
#pragma warning restore CA1033 // Interface methods should be callable by child types
    {
        return null;
    }

#pragma warning disable CA1033 // Interface methods should be callable by child types
    System.Windows.Forms.Control? IComponent.GetSettingsControl(LayoutMode mode)
#pragma warning restore CA1033 // Interface methods should be callable by child types
    {
        return null;
    }

    void IComponent.SetSettings(XmlNode settings)
    {

    }

    void IComponent.Update(IInvalidator invalidator, LiveSplitState state, float width, float height, LayoutMode mode)
    {

    }
}
