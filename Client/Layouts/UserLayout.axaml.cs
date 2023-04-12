using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using Client.Api;
using Client.Pages;
using Client.ReNote;
using Client.Windows;

namespace Client.Layouts
{
    public partial class UserLayout : Layout
    {
        public UserSession Session { get; set; }

        private string m_ToolBarName;

        public UserLayout()
        {
            InitializeComponent();

#if DEBUG
            if (!Design.IsDesignMode)
                Initialized += OnLayoutInitialized;
#else
                Initialized += OnLayoutInitialized;
#endif
        }

        private void SetPage(Page page)
        {
            if (!string.IsNullOrWhiteSpace(m_ToolBarName) && m_ToolBarName == page.ToolBar.Name)
                return;

            ClearToolbar(page.ToolBar);

            double lastButtonWidth = 0;
            FormattedText formatter = new FormattedText();
            for (int i = 0; i < page.ToolBar.Buttons.Count; i++)
            {
                KeyValuePair<string, UserControl> keyValuePair = page.ToolBar.Buttons.ElementAt(i);

                Button toolBarButton = new Button()
                {
                    Content      = keyValuePair.Key,
                    Background   = Brushes.Transparent,
                    Margin       = new Thickness(lastButtonWidth, 0),
                    Height       = 30,
                    FontSize     = 13,
                    CornerRadius = new CornerRadius(0)
                };

                formatter.Typeface = new Typeface(toolBarButton.FontFamily, toolBarButton.FontStyle, toolBarButton.FontWeight);
                formatter.FontSize = toolBarButton.FontSize;
                formatter.Text     = toolBarButton.Content.ToString();

                lastButtonWidth = formatter.Bounds.Width + 22;

                m_ToolBar.Children.Add(toolBarButton);
            }

            m_Page.Children.Add(page);
        }

        private void GoToHome()
        {
            if (HomePage.Instance == null)
                HomePage.Instance = new HomePage();

            SetPage(HomePage.Instance);
        }

        private void ClearToolbar(ToolBar toolbar)
        {
            m_Page.Children.Clear();
            m_ToolBar.Children.Clear();

            m_ToolBarName = toolbar.Name;
            m_PageName.Text = m_ToolBarName;
        }

        private void OnLayoutInitialized(object sender, EventArgs e)
        {
            School schoolInfo = ReNote.Client.Instance.SchoolInformation;
            if (schoolInfo == null || Session == null)
                return;

            m_SchoolName.Text = schoolInfo.SchoolName;

            if (!string.IsNullOrWhiteSpace(Session.Team.TeamName))
                m_RealName.Text = $"{Session.RealName} ({Session.Team.TeamName})";
            else
                m_RealName.Text = Session.RealName;

            GoToHome();
        }

        private void OnMenuButtonClicked(object sender, PointerReleasedEventArgs e)
        {
            m_MenuSelector.IsVisible = !m_MenuSelector.IsVisible;
        }

        private void OnHomeButtonClicked(object sender, PointerReleasedEventArgs e)
        {
            if (m_MenuSelector.IsVisible)
                m_MenuSelector.IsVisible = false;

            GoToHome();
        }

        private void OnProfilePictureClicked(object sender, PointerReleasedEventArgs e)
        {
            if (m_MenuSelector.IsVisible)
                m_MenuSelector.IsVisible = false;

            if (AccountPage.Instance == null)
                AccountPage.Instance = new AccountPage();

            AccountPage.Instance.SetSession(Session);
            SetPage(AccountPage.Instance);
        }

        private async void OnLogoutButtonClicked(object sender, PointerReleasedEventArgs e)
        {
            await ApiService.SendLogoutAsync(Session.SessionId, Session.AuthToken, null);

            HomePage.Instance = null;
            AccountPage.Instance = null;

            MainWindow.Instance.SetWindowContent(new LogonLayout());
        }
    }
}