namespace Client.Layouts;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Markup.Xaml.MarkupExtensions;
using Avalonia.Media;
using Client.Api;
using Client.Api.Responses;
using Client.Managers;
using Client.Pages;
using Client.ReNote.Data;
using Client.Windows;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class UserLayout : Layout
{
    private static MenuInfo[] s_MenuInfo;
    private string m_ToolbarId;

    private const string TOOLBAR_HOME = "Home";
    private const string TOOLBAR_USER = "User";

    private Page CurrentPage
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

    public UserLayout()
    {
        InitializeComponent();

#if DEBUG
        if (Design.IsDesignMode)
            return;
#endif

        InitializeLayout();
        InitializeMenu();
        InitializeEvents();
    }

    public static void SetGlobalMenu(MenuInfo[] menuInfo)
    {
        s_MenuInfo = menuInfo;
    }

    private void InitializeLayout()
    {
        School schoolInfo = School.Instance;
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

        Navigate(TOOLBAR_HOME);
    }

    public void InitializeMenu()
    {
        if (s_MenuInfo == null)
            return;

        double lastButtonMargin = 65;
        for (int i = 0; i < s_MenuInfo.Length; i++)
        {
            string toolbarId = s_MenuInfo[i].Id;
            Button button = new Button()
            {
                Width = 225,
                Height = 45,
                Margin = new Thickness(0, lastButtonMargin),
                CornerRadius = new CornerRadius(0),
                VerticalAlignment = VerticalAlignment.Top,
            };

            button.Classes.Add("MENU_BUTTON");

            Panel buttonContent = new Panel();

            TextBlock contentIcon = new TextBlock()
            {
                Margin = new Thickness(10),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
                FontFamily = FontFamily.Parse("Segoe Fluent Icons", new Uri("avares://Client/Assets/segfluicons.ttf")),
                FontSize = 16,
                Width = 16,
                Height = 16,
                Text = s_MenuInfo[i].Icon
            };

            TextBlock contentText = new TextBlock()
            {
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            contentText[!TextBlock.TextProperty] = new DynamicResourceExtension(s_MenuInfo[i].Name);

            buttonContent.Children.Add(contentIcon);
            buttonContent.Children.Add(contentText);

            button.Content = buttonContent;
            button.Click += (sender, e) => Navigate(toolbarId);

            m_MenuBar.Children.Add(button);

            lastButtonMargin += 45;
        }

        m_MenuAccountItemText[!TextBlock.TextProperty] = new DynamicResourceExtension("UserTitle");
        m_MenuAccountItem.Click += (sender, e) => Navigate("User");
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

            Navigate(TOOLBAR_HOME);
        };

        m_ProfileButton.PointerReleased += (sender, e) =>
        {
            if (e.InitialPressMouseButton != Avalonia.Input.MouseButton.Left)
                return;

            Navigate(TOOLBAR_USER);
        };

        m_LogOutButton.PointerReleased += async (sender, e) =>
        {
            if (e.InitialPressMouseButton != Avalonia.Input.MouseButton.Left)
                return;

            await ApiService.LogoutAsync((requestStatus, response) => MainWindow.Instance.SetLogonUI());
        };
    }

    public void Navigate(string toolbarId)
    {
        Toolbar toolbar = ToolbarManager.GetToolbar(toolbarId);
        if (toolbar == null || toolbar.DefaultPage == null)
            return;

        CurrentPage?.Destroy();

        Page page = (Page)Activator.CreateInstance(toolbar.DefaultPage);
        CurrentPage = page;

        if (m_MenuSelector.IsVisible)
            m_MenuSelector.IsVisible = false;

        m_CurrentPageLabel[!TextBlock.TextProperty] = new DynamicResourceExtension(toolbar.Name);

        SetToolbar(toolbar);
    }

    public void Navigate(Page page)
    {
        if (m_Page.Children.Count > 0 && m_Page.Children[0].GetType() == page.GetType())
            return;

        if (CurrentPage != null)
            CurrentPage.Destroy();

        CurrentPage = page;

        Toolbar toolbar = ToolbarManager.GetToolbar(page.GetToolbarId());
        if (toolbar == null || m_ToolbarId == toolbar.Id)
            return;

        if (m_MenuSelector.IsVisible)
            m_MenuSelector.IsVisible = false;

        m_CurrentPageLabel[!TextBlock.TextProperty] = new DynamicResourceExtension(toolbar.Name);

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
                Background = Brushes.Transparent,
                Margin = new Thickness(lastButtonWidth, 0),
                Height = 30,
                FontSize = 13,
                CornerRadius = new CornerRadius(0)
            };

            barButton[!ContentProperty] = new DynamicResourceExtension(buttonArgs.Key);

            barButton.GetObservable(ContentProperty).Subscribe(value =>
            {
                formatter.Typeface = new Typeface(barButton.FontFamily, barButton.FontStyle, barButton.FontWeight);
                formatter.FontSize = barButton.FontSize;
                formatter.Text = (string)value;

                lastButtonWidth = formatter.Bounds.Width + 4;
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