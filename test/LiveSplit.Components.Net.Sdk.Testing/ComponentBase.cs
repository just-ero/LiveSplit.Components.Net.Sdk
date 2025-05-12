#pragma warning disable CA1063 // Implement IDisposable Correctly
#pragma warning disable CA1816 // Dispose methods should call SuppressFinalize

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Xml;

using LiveSplit.Model;
using LiveSplit.UI;
using LiveSplit.UI.Components;

namespace LiveSplit.Components.Net.Sdk.Testing;

public abstract class ComponentBase : IComponent
{
    protected ComponentBase(LiveSplitState __) { }

    string IComponent.ComponentName => throw new NotImplementedException();

    float IComponent.HorizontalWidth => throw new NotImplementedException();
    float IComponent.VerticalHeight => throw new NotImplementedException();

    float IComponent.MinimumWidth => throw new NotImplementedException();
    float IComponent.MinimumHeight => throw new NotImplementedException();

    float IComponent.PaddingLeft => throw new NotImplementedException();
    float IComponent.PaddingRight => throw new NotImplementedException();
    float IComponent.PaddingTop => throw new NotImplementedException();
    float IComponent.PaddingBottom => throw new NotImplementedException();

    IDictionary<string, Action> IComponent.ContextMenuControls => throw new NotImplementedException();

    void IDisposable.Dispose()
    {
        throw new NotImplementedException();
    }

    void IComponent.DrawHorizontal(Graphics g, LiveSplitState state, float height, Region clipRegion)
    {
        throw new NotImplementedException();
    }

    void IComponent.DrawVertical(Graphics g, LiveSplitState state, float width, Region clipRegion)
    {
        throw new NotImplementedException();
    }

    XmlNode IComponent.GetSettings(XmlDocument document)
    {
        throw new NotImplementedException();
    }

    System.Windows.Forms.Control IComponent.GetSettingsControl(LayoutMode mode)
    {
        throw new NotImplementedException();
    }

    void IComponent.SetSettings(XmlNode settings)
    {
        throw new NotImplementedException();
    }

    void IComponent.Update(IInvalidator invalidator, LiveSplitState state, float width, float height, LayoutMode mode)
    {
        throw new NotImplementedException();
    }
}
