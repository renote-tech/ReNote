namespace Client.Controls;

using Avalonia.Controls;
using Avalonia.Styling;
using System;

internal class LoadingButton : Button, IStyleable
{
    Type IStyleable.StyleKey => typeof(Button);

    private const int RING_SIZE = 16;
    private object m_PreviousContent;

    public void SimulateClick()
    {
        OnClick();
    }

    public void RestoreContent()
    {
        Content = m_PreviousContent;
    }

    protected override void OnClick()
    {
        m_PreviousContent = Content;
        Content = new ProgressRing() { Width = RING_SIZE, Height = RING_SIZE };

        base.OnClick();
    }
}