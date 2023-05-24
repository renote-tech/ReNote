namespace Client.Managers;

using Avalonia;
using Avalonia.Media;

using Client.ReNote.Data;

using System;
using System.Collections.Generic;

internal class ThemeManager
{
    private static Theme[] s_Themes;
    private static Theme s_CurrentTheme;

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
        s_Themes = themes;
        for (int i = 0; i < s_Themes.Length; i++)
            s_Themes[i].Id = i;

        for (int i = 0; i < s_ThemeTypes.Length; i++)
        {
            string type = s_ThemeTypes[i];
            string lightType = $"{type}Light";

            Application.Current.Resources.TryGetResource(type, out object resource);
            Application.Current.Resources.TryGetResource(lightType, out object lightResource);

            if (resource != null && resource is SolidColorBrush)
                s_DefaultTheme[type] = (SolidColorBrush)resource;

            if (lightResource != null && lightResource is SolidColorBrush)
                s_LightTheme[lightType] = (SolidColorBrush)lightResource;
        }
    }

    public static void RestoreDefault()
    {
        s_CurrentTheme = null;

        for (int i = 0; i < s_ThemeTypes.Length; i++)
        {
            string type = s_ThemeTypes[i];
            s_DefaultTheme.TryGetValue(type, out SolidColorBrush brush);
            Application.Current.Resources[type] = brush;
        }
    }

    public static void SetThemeByName(string themeName)
    {
        if (string.IsNullOrWhiteSpace(themeName))
            return;

        for (int i = 0; i < s_Themes.Length; i++)
        {
            if (s_Themes[i].Name == themeName)
            {
                SetTheme(s_Themes[i]);
                break;
            }
        }
    }

    public static void SetTheme(Theme theme)
    {
        if (theme == null)
            return;

        if (s_CurrentTheme != null && s_CurrentTheme.Name.ToLower() == theme.Name.ToLower())
            return;

        s_CurrentTheme = theme;

        switch (s_CurrentTheme.Name.ToLower())
        {
            case "dark":
                RestoreDefault();
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

        Application.Current.Resources["BaseBackground"] = GetThemedColor("BaseBackground", s_CurrentTheme.Accent, s_CurrentTheme.IsDarkTheme);

        Application.Current.Resources["BaseButton"] = GetThemedColor("BaseButton", s_CurrentTheme.Accent, s_CurrentTheme.IsDarkTheme);
        Application.Current.Resources["BaseButtonHover"] = GetThemedColor("BaseButtonHover", s_CurrentTheme.Accent, s_CurrentTheme.IsDarkTheme);
        Application.Current.Resources["BaseButtonDisabled"] = GetThemedColor("BaseButtonDisabled", s_CurrentTheme.Accent, s_CurrentTheme.IsDarkTheme);

        Application.Current.Resources["BaseSidebar"] = GetThemedColor("BaseSidebar", s_CurrentTheme.Accent, s_CurrentTheme.IsDarkTheme);
        Application.Current.Resources["BaseTopBar"] = GetThemedColor("BaseTopBar", s_CurrentTheme.Accent, s_CurrentTheme.IsDarkTheme);
        Application.Current.Resources["BaseToolBar"] = GetThemedColor("BaseToolBar", s_CurrentTheme.Accent, s_CurrentTheme.IsDarkTheme);

        Application.Current.Resources["BaseMenu"] = GetThemedColor("BaseMenu", s_CurrentTheme.Accent, s_CurrentTheme.IsDarkTheme);
        Application.Current.Resources["BaseMenuButton"] = GetThemedColor("BaseMenuButton", s_CurrentTheme.Accent, s_CurrentTheme.IsDarkTheme);
        Application.Current.Resources["BaseMenuButtonHover"] = GetThemedColor("BaseMenuButtonHover", s_CurrentTheme.Accent, s_CurrentTheme.IsDarkTheme);
        
        Application.Current.Resources["BaseBackgroundDialog"] = GetThemedColor("BaseBackgroundDialog", s_CurrentTheme.Accent, s_CurrentTheme.IsDarkTheme);

        Application.Current.Resources["BaseSelectionHover"] = GetThemedColor("BaseSelectionHover", s_CurrentTheme.Accent, false);

        Application.Current.Resources["BaseForegroundGray"] = GetThemedColor("BaseForegroundGray", s_CurrentTheme.Accent, s_CurrentTheme.IsDarkTheme);

        Application.Current.Resources["BaseAccent"] = s_CurrentTheme.Accent;
        Application.Current.Resources["BaseAccentHover"] = GetThemedColor("BaseAccentHover", s_CurrentTheme.Accent, false);
        Application.Current.Resources["BaseAccentPress"] = GetThemedColor("BaseAccentPress", s_CurrentTheme.Accent, false);

        if (s_CurrentTheme.IsDarkTheme)
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
            return null;

        for (int i = 0; i < s_Themes.Length; i++)
        {
            if (s_Themes[i].Name == themeName)
            {
                return s_Themes[i];
            }
        }

        return null;
    }

    public static Theme GetCurrentTheme()
    {
        if (s_CurrentTheme != null)
            return s_CurrentTheme;

        for (int i = 0; i < s_Themes.Length; i++)
        {
            if (s_Themes[i].Name.ToLower() == "dark")
                return s_Themes[i];
        }

        return null;
    }

    public static Theme[] GetAllThemes()
    {
        return s_Themes;
    }

    private static SolidColorBrush GetThemedColor(string type, Color targetColor, bool isDarkMode)
    {
        s_DefaultTheme.TryGetValue(type, out SolidColorBrush brush);
        if (brush == null)
            throw new ArgumentException("'brush' was null");

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