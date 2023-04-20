using Avalonia.Controls;
using Avalonia.Markup.Xaml.MarkupExtensions;
using Client.Windows;

namespace Client.Popups;

public partial class PopupMessage : UserControl
{
    public delegate void ConfirmEvent();

    public PopupMessage()
    {
        InitializeComponent();
    }

    public void SetMessage(string messageKey)
    {
        m_MessageLabel[!TextBlock.TextProperty] = new DynamicResourceExtension(messageKey);
    }

    public void SetConfirmEvent(ConfirmEvent confirmEvent)
    {
        m_ConfirmButton.Click += (sender, e) =>
        {
            if (confirmEvent != null)
                confirmEvent();

            Panel parent = (Panel)Parent;
            if (parent == null)
                return;

            parent.Children.Remove(this);
        };
    }

    public static PopupMessage Show(string messageKey, ConfirmEvent confirmEvent)
    {
        PopupMessage popup = new PopupMessage();
        popup.SetMessage(messageKey);
        popup.SetConfirmEvent(confirmEvent);

        if (MainWindow.Instance == null)
            return popup;

        Panel content = MainWindow.Instance.GetMainContent();
        if (content == null)
            return popup;
        
        content.Children.Add(popup);
        return popup;
    }
}