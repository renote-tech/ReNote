namespace Client.Dialogs;

using Avalonia.Controls;

internal class Dialog : UserControl
{
    public static Dialog Instance { get; set; }

    public void Close()
    {
        Panel parentPanel = (Panel)Parent;
        if (parentPanel == null)
            return;

        parentPanel.Children.Remove(this);
        Instance = null;
    }
}