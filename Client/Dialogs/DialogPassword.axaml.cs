namespace Client.Dialogs;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using Client.Api;
using Client.Api.Requests;
using Client.Windows;
using System;

public partial class DialogPassword : Dialog
{
    private static DialogMessage s_DialogMessage;

    public DialogPassword()
    {
        InitializeComponent();

        if (Design.IsDesignMode)
            return;

        InitializeLayout();
        InitializeEvents();
    }

    private void InitializeLayout()
    {
        s_DialogMessage = DialogMessage.Create("$$DialogInformation$$", string.Empty);

        m_ConfirmButton.IsEnabled = false;
    }

    private void InitializeEvents()
    {
        FormattedText formatter = new FormattedText();
        m_CancelButton.GetObservable(ContentProperty).Subscribe(value =>
        {
            formatter.Typeface = new Typeface(m_CancelButton.FontFamily, m_CancelButton.FontStyle, m_CancelButton.FontWeight);
            formatter.FontSize = m_CancelButton.FontSize;
            formatter.Text = (string)value;

            m_CancelButton.Width = formatter.Bounds.Width + 8;
        });

        m_CurrentPassTextBox.GetObservable(TextBox.TextProperty).Subscribe(value =>
        {
            m_ConfirmButton.IsEnabled = !string.IsNullOrWhiteSpace(m_CurrentPassTextBox.Text) &&
                                        !string.IsNullOrWhiteSpace(m_NewPassTextBox.Text) &&
                                        !string.IsNullOrWhiteSpace(m_ConfirmNewPassTextBox.Text);
        });

        m_NewPassTextBox.GetObservable(TextBox.TextProperty).Subscribe(value =>
        {
            m_ConfirmButton.IsEnabled = !string.IsNullOrWhiteSpace(m_CurrentPassTextBox.Text) &&
                                        !string.IsNullOrWhiteSpace(m_NewPassTextBox.Text) &&
                                        !string.IsNullOrWhiteSpace(m_ConfirmNewPassTextBox.Text);
        });

        m_ConfirmNewPassTextBox.GetObservable(TextBox.TextProperty).Subscribe(value =>
        {
            m_ConfirmButton.IsEnabled = !string.IsNullOrWhiteSpace(m_CurrentPassTextBox.Text) &&
                                        !string.IsNullOrWhiteSpace(m_NewPassTextBox.Text) &&
                                        !string.IsNullOrWhiteSpace(m_ConfirmNewPassTextBox.Text);
        });

        m_ConfirmNewPassTextBox.KeyUp += (sender, e) =>
        {
            if (e.Key == Key.Enter && m_ConfirmButton.IsEnabled)
                m_ConfirmButton.SimulateClick();
        };

        SetCancelButton(null);
        SetConfirmButton(async () =>
        {
            if (m_NewPassTextBox.Text != m_ConfirmNewPassTextBox.Text)
            {
                s_DialogMessage.SetTitle("$$DialogInformation$$");
                s_DialogMessage.SetMessage("$$DialogPasswordNotMatch$$");
                s_DialogMessage.Show(this);

                m_ConfirmButton.RestoreContent();
                return;
            }

            m_CancelButton.IsEnabled = false;
            m_ConfirmButton.IsEnabled = false;

            await ApiService.ChangePasswordAsync(new PasswordRequest(m_CurrentPassTextBox.Text, m_NewPassTextBox.Text), (requestStatus, response) =>
            {
                m_CancelButton.IsEnabled = true;

                if (response.Status != 200)
                {
                    s_DialogMessage.SetTitle("$$DialogError$$");
                    s_DialogMessage.SetMessage($"$${response.Message}$$");
                    s_DialogMessage.Show(this);
                    return;
                }

                s_DialogMessage.SetTitle("$$DialogInformation$$");
                s_DialogMessage.SetMessage("$$DialogLoginAgain$$");
                s_DialogMessage.SetConfirmButton(async () =>
                {
                    await ApiService.LogoutAsync((requestStatus, response) => MainWindow.Instance.SetLogonUI());
                });

                s_DialogMessage.Show();

                m_ConfirmButton.RestoreContent();
                Close();
            });
        }, false);
    }

    public override void Show()
    {
        m_CurrentPassTextBox.Clear();
        m_NewPassTextBox.Clear();
        m_ConfirmNewPassTextBox.Clear();

        base.Show();
    }
}