namespace Client.Dialogs;

using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml.MarkupExtensions;

using Client.Exceptions;
using Client.Windows;

internal partial class DialogMessage : Dialog
{
    public delegate void ConfirmEvent();

    public DialogMessage()
    {
        InitializeComponent();
        InitializeLayout();
        InitializeEvents();
    }

    public void InitializeLayout()
    {
        m_ConfirmButton[!ContentProperty] = new DynamicResourceExtension("DialogConfirm");
    }

    public void InitializeEvents()
    {
        MainWindow.Instance.KeyUp += OnDialogKeyUp;
    }

    public void SetMessage(string messageKey)
    {
        if (messageKey.StartsWith("$$") && messageKey.EndsWith("$$"))
            m_MessageLabel[!TextBlock.TextProperty] = new DynamicResourceExtension(messageKey.Trim('$'));
        else
            m_MessageLabel.Text = messageKey;
    }

    public void SetConfirmEvent(ConfirmEvent confirmEvent)
    {
        m_ConfirmButton.Click += (sender, e) =>
        {
            confirmEvent?.Invoke();

            MainWindow.Instance.KeyUp -= OnDialogKeyUp;

            Close();
        };
    }

    private void OnDialogKeyUp(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
            m_ConfirmButton.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
    }

    public static DialogMessage Show(string messageKey, ConfirmEvent confirmEvent = null, bool alawysShow = true)
    {
        if (MainWindow.Instance == null)
            throw new UninitializedException("DialogMessage", "Show");

        if (Instance != null && !alawysShow)
            return null;

        if (Instance != null)
            Instance.Close();

        DialogMessage dialog = new DialogMessage();

        dialog.SetMessage(messageKey);
        dialog.SetConfirmEvent(confirmEvent);

        MainWindow.Instance.AddDialog(dialog);

        Instance = dialog;

        return dialog;
    }
}