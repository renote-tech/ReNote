namespace Client.Pages;

using Avalonia.Controls;

public class Page : UserControl
{
    public virtual void Destroy()
    { }

    public virtual string GetToolbarId()
    {
        return null;
    }
}