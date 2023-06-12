namespace Client.Dialogs;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml.MarkupExtensions;
using Client.Exceptions;
using Client.Windows;
using System;

public partial class DialogMessage : Dialog
{
    public delegate void ConfirmEvent();

    public const int BASE_DIALOG_HEIGHT = 115;

    public DialogMessage()
    {
        InitializeComponent();

        if (Design.IsDesignMode)
            return;

        InitializeLayout();
        InitializeEvents();
    }

    public void InitializeLayout()
    {
        m_ConfirmButton[!ContentProperty] = new DynamicResourceExtension("DialogConfirm");
    }

    public void InitializeEvents()
    {
        m_MessageLabel.GetObservable(TextBlock.TextProperty).Subscribe(value =>
        {
            m_ContentBorder.Height = m_MessageLabel.Height + BASE_DIALOG_HEIGHT;
        });
    }

    public void SetMessage(string messageKey)
    {
        if (messageKey.StartsWith("$$") && messageKey.EndsWith("$$"))
            m_MessageLabel[!TextBlock.TextProperty] = new DynamicResourceExtension(messageKey.Trim('$'));
        else
            m_MessageLabel.Text = messageKey;
    }

    public void SetTitle(string titleKey)
    {
        if (titleKey.StartsWith("$$") && titleKey.EndsWith("$$"))
            m_TitleLabel[!TextBlock.TextProperty] = new DynamicResourceExtension(titleKey.Trim('$'));
        else
            m_TitleLabel.Text = titleKey;
    }

    public static DialogMessage Create(string titleKey, string messageKey, ConfirmEvent confirmEvent = null)
    {
        DialogMessage dialog = new DialogMessage();
        dialog.SetTitle(titleKey);
        dialog.SetMessage(messageKey);
        dialog.SetConfirmButton(confirmEvent);

        return dialog;
    }

    public static void Show(string titleKey, string messageKey, ConfirmEvent confirmEvent = null, bool force = true)
    {
        if (MainWindow.Instance == null)
            throw new UninitializedException("DialogMessage", "Show");

        if (Instance != null && !force)
            return;

        Instance?.Close();

        DialogMessage dialog = new DialogMessage();

        dialog.SetTitle(titleKey);
        dialog.SetMessage(messageKey);
        dialog.SetConfirmButton(confirmEvent);

        MainWindow.Instance.BindDialog(dialog);

        Instance = dialog;
    }
}