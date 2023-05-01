using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml.MarkupExtensions;
using Avalonia.Media;
using Client.Api;
using Client.Api.Responses;
using Client.Managers;
using Client.Pages;
using Client.ReNote.Data;
using Client.Windows;

namespace Client.Layouts
{
    public partial class UserLayout : Layout
    {
        private Page m_CurrentPage
        {
            get
            {
                if (m_Page.Children.Count == 0)
                    return null;

                if (m_Page.Children[0] is not Page)
                    return null;

                return (Page)m_Page.Children[0];
            }
            set
            {
                m_Page.Children.Clear();
                m_Page.Children.Add(value);
            }
        }
        private string m_ToolbarId;

        public UserLayout()
        {
            InitializeComponent();

#if DEBUG
            if (Design.IsDesignMode)
                return;
#endif

            InitializeLayout();
            InitializeEvents();
        }

        private void InitializeLayout()
        {
            School schoolInfo = ReNote.Client.Instance.SchoolInformation;
            User user = User.Current;

            if (schoolInfo == null || user == null)
                return;

            m_SchoolName.Text = schoolInfo.SchoolName;

            if (!string.IsNullOrWhiteSpace(user.Team.TeamName))
                m_RealName.Text = $"{user.RealName} ({user.Team.TeamName})";
            else
                m_RealName.Text = user.RealName;

            ThemeManager.SetThemeByName(user.Theme);
            LanguageManager.SetLanguage(user.Language);

            Navigate("Home");
        }
        
        private void InitializeEvents()
        {
            m_MenuButton.PointerReleased += (sender, e) =>
            {
                if (e.InitialPressMouseButton != Avalonia.Input.MouseButton.Left)
                    return;

                m_MenuSelector.IsVisible = true;
            };

            m_MenuMask.PointerReleased += (sender, e) =>
            {
                if (e.InitialPressMouseButton != Avalonia.Input.MouseButton.Left)
                    return;

                m_MenuSelector.IsVisible = false;
            };

            m_CloseButton.PointerReleased += (sender, e) =>
            {
                if (e.InitialPressMouseButton != Avalonia.Input.MouseButton.Left)
                    return;

                m_MenuSelector.IsVisible = false;
            };

            m_HomeButton.PointerReleased += (sender, e) => 
            {
                if (e.InitialPressMouseButton != Avalonia.Input.MouseButton.Left)
                    return;
                
                Navigate("Home");
            };

            m_ProfileButton.PointerReleased += (sender, e) =>
            {
                if (e.InitialPressMouseButton != Avalonia.Input.MouseButton.Left)
                    return;

                Navigate("User");
            };

            m_LogOutButton.PointerReleased += async (sender, e) =>
            {
                if (e.InitialPressMouseButton != Avalonia.Input.MouseButton.Left)
                    return;

                await ApiService.LogoutAsync((HttpStatusCode statusCode, Response response) =>
                {
                    User.Delete();
                    MainWindow.Instance.SetLayout(new LogonLayout());
                });
            };
        }

        public void Navigate(string toolbarId)
        {
            Toolbar toolbar = ToolbarManager.GetToolbar(toolbarId);
            if (toolbar == null || toolbar.DefaultPage == null)
                return;

            if (m_CurrentPage != null)
                m_CurrentPage.Destroy();

            Page page = (Page)Activator.CreateInstance(toolbar.DefaultPage);
            m_CurrentPage = page;

            SetToolbar(toolbar);
        }

        public void Navigate(Page page)
        {
            if (m_Page.Children.Count > 0 && m_Page.Children[0].GetType() == page.GetType())
                return;

            if (m_CurrentPage != null)
                m_CurrentPage.Destroy();

            m_CurrentPage = page;

            Toolbar toolbar = ToolbarManager.GetToolbar(page.GetToolbarId());
            if (toolbar == null || m_ToolbarId == toolbar.Id)
                return;

            SetToolbar(toolbar);
        }

        private void SetToolbar(Toolbar toolbar)
        {
            m_Toolbar.Children.Clear();

            double lastButtonWidth = 0;
            FormattedText formatter = new FormattedText();
            for (int i = 0; i < toolbar.Buttons.Count; i++)
            {
                KeyValuePair<string, Type> buttonArgs = toolbar.Buttons.ElementAt(i);

                Button barButton = new Button()
                {
                    Background   = Brushes.Transparent,
                    Margin       = new Thickness(lastButtonWidth, 0),
                    Height       = 30,
                    FontSize     = 13,
                    CornerRadius = new CornerRadius(0)
                };

                barButton[!ContentProperty] = new DynamicResourceExtension(buttonArgs.Key);

                barButton.GetObservable(ContentProperty).Subscribe(value =>
                {
                    formatter.Typeface = new Typeface(barButton.FontFamily, barButton.FontStyle, barButton.FontWeight);
                    formatter.FontSize = barButton.FontSize;
                    formatter.Text = (string)value;

                    lastButtonWidth = formatter.Bounds.Width + 16;
                });

                barButton.Click += (sender, e) =>
                {
                    if (buttonArgs.Value == null)
                        return;

                    Page page = (Page)Activator.CreateInstance(buttonArgs.Value);
                    Navigate(page);
                };

                m_Toolbar.Children.Add(barButton);
            }

            m_ToolbarId = toolbar.Id;
            m_PageName[!TextBlock.TextProperty] = new DynamicResourceExtension(toolbar.Name);
        }
    }
}