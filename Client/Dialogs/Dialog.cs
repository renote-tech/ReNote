namespace Client.Dialogs;

using Avalonia.Controls;
using Avalonia.Interactivity;
using Client.Exceptions;
using Client.Windows;
using System;

public class Dialog : UserControl
{
    public static Dialog Instance { get; set; }

    private const string CancelButtonName = "m_CancelButton";
    private const string ConfirmButtonName = "m_ConfirmButton";

    private Button CancelButton;
    private Button ConfirmButtion;
    private Dialog m_Child;

    public void SetCancelButton(Delegate action, bool autoClose = true)
    {
        CancelButton = this.FindControl<Button>(CancelButtonName);
        if (CancelButton == null)
            throw new Exception($"Cancel button '{CancelButtonName}' was not found");

        CancelButton.Click += (sender, e) =>
        {
            action?.DynamicInvoke();

            if (autoClose)
                Close();
        };
    }

    public void SetConfirmButton(Delegate action, bool autoClose = true)
    {
        ConfirmButtion = this.FindControl<Button>(ConfirmButtonName);
        if (ConfirmButtion == null)
            throw new Exception($"Confirm button '{ConfirmButtonName}' was not found");

        ConfirmButtion.Click += (sender, e) =>
        {
            action?.DynamicInvoke();

            if (autoClose)
                Close();
        };
    }

    public void Cancel()
    {
        if (CancelButton == null || !CancelButton.IsEnabled)
            return;

        CancelButton.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
    }

    public void Confirm()
    {
        if (ConfirmButtion == null || !ConfirmButtion.IsEnabled)
            return;

        ConfirmButtion.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
    }

    public virtual void Show()
    {
        if (MainWindow.Instance == null)
            throw new UninitializedException("DialogPassword", "Show");

        m_Child = null;
        Instance?.Close();

        MainWindow.Instance.BindDialog(this);
        Instance = this;
    }

    public void Show(Dialog child)
    {
        if (MainWindow.Instance == null)
            throw new UninitializedException("DialogPassword", "Show");

        m_Child = child;
        MainWindow.Instance.UnbindDialog(m_Child);

        Instance?.Close();

        MainWindow.Instance.BindDialog(this);
        Instance = this;
    }

    public void Close()
    {
        if (m_Child != null)
            MainWindow.Instance.BindDialog(m_Child);

        MainWindow.Instance.UnbindDialog(this);
        Instance = null;
    }
}