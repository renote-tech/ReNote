namespace Client.Managers;

using Avalonia;
using Avalonia.Media;
using Client.ReNote.Data;
using System;
using System.Collections.Generic;

internal class ThemeManager
{
    public const string DEFAULT_THEME_NAME = "dark";

    public static Theme[] Themes;
    public static Theme CurrentTheme;

#if DEBUG
    public static readonly Dictionary<string, SolidColorBrush> s_DefaultTheme = new Dictionary<string, SolidColorBrush>();
    public static readonly Dictionary<string, SolidColorBrush> s_LightTheme = new Dictionary<string, SolidColorBrush>();
#else
    private static readonly Dictionary<string, SolidColorBrush> s_DefaultTheme = new Dictionary<string, SolidColorBrush>();
    private static readonly Dictionary<string, SolidColorBrush> s_LightTheme = new Dictionary<string, SolidColorBrush>();
#endif

    private static readonly string[] s_ThemeTypes = new string[]
    {
            "BaseBackground",
            "BaseButton",
            "BaseButtonHover",
            "BaseButtonDisabled",
            "BaseSidebar",
            "BaseTopBar",
            "BaseToolBar",
            "BaseMenu",
            "BaseMenuButton",
            "BaseMenuButtonHover",
            "BaseBackgroundDialog",
            "BaseSelectionHover",
            "BaseAccent",
            "BaseAccentHover",
            "BaseAccentPress",
            "BaseForeground",
            "BaseForegroundTitle",
            "BaseForegroundGray"
    };

    public static void Initialize(Theme[] themes)
    {
        Themes = themes;
        for (int i = 0; i < Themes.Length; i++)
            Themes[i].Id = i;

        for (int i = 0; i < s_ThemeTypes.Length; i++)
        {
            string type = s_ThemeTypes[i];
            string lightType = $"{type}Light";

            Application.Current.Resources.TryGetResource(type, out object resource);
            Application.Current.Resources.TryGetResource(lightType, out object lightResource);

            if (resource != null && resource is SolidColorBrush darkBrush)
                s_DefaultTheme[type] = darkBrush;

            if (lightResource != null && lightResource is SolidColorBrush lightBruh)
                s_LightTheme[lightType] = lightBruh;
        }
    }

    public static void SetThemeByName(string themeName)
    {
        if (string.IsNullOrWhiteSpace(themeName))
            return;

        for (int i = 0; i < Themes.Length; i++)
        {
            if (Themes[i].Name.ToLower() == themeName.ToLower())
            {
                SetTheme(Themes[i]);
                break;
            }
        }
    }

    public static void SetTheme(Theme theme)
    {
        if (theme == null)
            return;

        if (CurrentTheme != null && CurrentTheme.Name.ToLower() == theme.Name.ToLower())
            return;

        CurrentTheme = theme;

        switch (CurrentTheme.Name.ToLower())
        {
            case "dark":
                for (int i = 0; i < s_ThemeTypes.Length; i++)
                {
                    string type = s_ThemeTypes[i];
                    s_DefaultTheme.TryGetValue(type, out SolidColorBrush brush);
                    Application.Current.Resources[type] = brush;
                }
                return;
            case "light":
                for (int i = 0; i < s_ThemeTypes.Length; i++)
                {
                    string type = s_ThemeTypes[i];
                    s_LightTheme.TryGetValue($"{type}Light", out SolidColorBrush brush);
                    Application.Current.Resources[type] = brush;
                }
                return;
        }

        Application.Current.Resources["BaseBackground"] = GetThemedColor("BaseBackground", CurrentTheme.Accent, CurrentTheme.IsDarkTheme);

        Application.Current.Resources["BaseButton"] = GetThemedColor("BaseButton", CurrentTheme.Accent, CurrentTheme.IsDarkTheme);
        Application.Current.Resources["BaseButtonHover"] = GetThemedColor("BaseButtonHover", CurrentTheme.Accent, CurrentTheme.IsDarkTheme);
        Application.Current.Resources["BaseButtonDisabled"] = GetThemedColor("BaseButtonDisabled", CurrentTheme.Accent, CurrentTheme.IsDarkTheme);

        Application.Current.Resources["BaseSidebar"] = GetThemedColor("BaseSidebar", CurrentTheme.Accent, CurrentTheme.IsDarkTheme);
        Application.Current.Resources["BaseTopBar"] = GetThemedColor("BaseTopBar", CurrentTheme.Accent, CurrentTheme.IsDarkTheme);
        Application.Current.Resources["BaseToolBar"] = GetThemedColor("BaseToolBar", CurrentTheme.Accent, CurrentTheme.IsDarkTheme);

        Application.Current.Resources["BaseMenu"] = GetThemedColor("BaseMenu", CurrentTheme.Accent, CurrentTheme.IsDarkTheme);
        Application.Current.Resources["BaseMenuButton"] = GetThemedColor("BaseMenuButton", CurrentTheme.Accent, CurrentTheme.IsDarkTheme);
        Application.Current.Resources["BaseMenuButtonHover"] = GetThemedColor("BaseMenuButtonHover", CurrentTheme.Accent, CurrentTheme.IsDarkTheme);

        Application.Current.Resources["BaseBackgroundDialog"] = GetThemedColor("BaseBackgroundDialog", CurrentTheme.Accent, CurrentTheme.IsDarkTheme);

        Application.Current.Resources["BaseSelectionHover"] = GetThemedColor("BaseSelectionHover", CurrentTheme.Accent, false);

        Application.Current.Resources["BaseForegroundGray"] = GetThemedColor("BaseForegroundGray", CurrentTheme.Accent, CurrentTheme.IsDarkTheme);

        Application.Current.Resources["SystemAccentColor"] = CurrentTheme.Accent;

        Application.Current.Resources["BaseAccent"] = new SolidColorBrush(CurrentTheme.Accent);
        Application.Current.Resources["BaseAccentHover"] = GetThemedColor("BaseAccentHover", CurrentTheme.Accent, false);
        Application.Current.Resources["BaseAccentPress"] = GetThemedColor("BaseAccentPress", CurrentTheme.Accent, false);

        if (CurrentTheme.IsDarkTheme)
        {
            Application.Current.Resources["BaseForeground"] = s_DefaultTheme["BaseForeground"];
            Application.Current.Resources["BaseForegroundTitle"] = s_DefaultTheme["BaseForegroundTitle"];
        }
        else
        {
            Application.Current.Resources["BaseForeground"] = s_LightTheme["BaseForegroundLight"];
            Application.Current.Resources["BaseForegroundTitle"] = s_LightTheme["BaseForegroundTitleLight"];
        }
    }

    public static Theme GetThemeByName(string themeName)
    {
        if (string.IsNullOrWhiteSpace(themeName))
            themeName = DEFAULT_THEME_NAME;

        for (int i = 0; i < Themes.Length; i++)
        {
            if (Themes[i].Name.ToLower() == themeName.ToLower())
                return Themes[i];
        }

        return null;
    }

    private static SolidColorBrush GetThemedColor(string type, Color targetColor, bool isDarkMode)
    {
        s_DefaultTheme.TryGetValue(type, out SolidColorBrush brush);
        if (brush == null)
            throw new ArgumentException("'brush' is null");

        Color baseColor = brush.Color;

        double baseValue = 1 - 1 / 765 * (baseColor.R + baseColor.B + baseColor.G);
        double darkness = 1 - (0.299 * baseColor.R + 0.587 * baseColor.G + 0.114 * baseColor.B) / 255;
        double adjustment = (isDarkMode ? darkness - baseValue : 1 - darkness) * 2 / 3;

        Color themedColor = Color.FromRgb(
            (byte)Math.Max(Math.Min(targetColor.R + (int)(adjustment * 255), 255), 0),
            (byte)Math.Max(Math.Min(targetColor.G + (int)(adjustment * 255), 255), 0),
            (byte)Math.Max(Math.Min(targetColor.B + (int)(adjustment * 255), 255), 0));

        return new SolidColorBrush(themedColor);
    }
}