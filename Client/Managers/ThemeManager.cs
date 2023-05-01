using System;
using System.Net;
using Avalonia;
using Avalonia.Media;
using Client.Api.Responses;
using Client.Api;
using Client.ReNote.Data;

namespace Client.Managers
{
    internal class ThemeManager
    {
        public const string DefaultTheme = "Dark";

        private static Theme[] s_Themes;
        private static Theme s_CurrentTheme;

        private readonly static Color s_BaseBackground  = new Color(255, 20, 20, 20);
        private readonly static Color s_BaseButton      = new Color(255, 75, 75, 75);
        private readonly static Color s_BaseButtonHover = new Color(255, 55, 55, 55);
        private readonly static Color s_BaseButtonPress = new Color(255, 120, 120, 120);
        private readonly static Color s_BaseSidebar     = new Color(255, 30, 30, 30);
        private readonly static Color s_BaseTopBar      = new Color(255, 21, 21, 21);
        private readonly static Color s_BaseToolBar     = new Color(255, 70, 70, 70);

        private readonly static Color s_BaseSelectionHover = new Color(255, 85, 85, 85);
        
        private readonly static Color s_BaseAccent         = new Color(255, 147, 112, 219);
        private readonly static Color s_BaseAccentHover    = new Color(255, 137, 91, 233);
        private readonly static Color s_BaseAccentPress    = new Color(255, 122, 70, 231);
        
        private readonly static Color s_BaseForeground           = new Color(255, 255, 255, 255);
        private readonly static Color s_BaseForegroundError      = new Color(255, 219, 112, 147);
        private readonly static Color s_BaseForegroundTitle      = new Color(255, 210, 210, 210);
        private readonly static Color s_BaseForegroundLight      = new Color(255, 0, 0, 0);
        private readonly static Color s_BaseForegroundTitleLight = new Color(255, 45, 45, 45);

        private readonly static Color s_ThemeSelectionHover = new Color(255, 25, 25, 25);

        public static void Initialize(Theme[] themes)
        {
            s_Themes = themes;
            for (int i = 0; i < s_Themes.Length; i++)
                s_Themes[i].Id = i;
        }

        public static void RestoreDefault()
        {
            s_CurrentTheme = null;

            Application.Current.Resources["BaseBackground"] = new SolidColorBrush(s_BaseBackground);
            Application.Current.Resources["BaseSelectionHover"] = new SolidColorBrush(s_BaseSelectionHover);
            Application.Current.Resources["BaseButton"] = new SolidColorBrush(s_BaseButton);
            Application.Current.Resources["BaseButtonHover"] = new SolidColorBrush(s_BaseButtonHover);
            Application.Current.Resources["BaseButtonPress"] = new SolidColorBrush(s_BaseButtonPress);
            Application.Current.Resources["BaseBackground"] = new SolidColorBrush(s_BaseBackground);
            Application.Current.Resources["BaseBackground"] = new SolidColorBrush(s_BaseBackground);
            Application.Current.Resources["BaseSidebar"] = new SolidColorBrush(s_BaseSidebar);
            Application.Current.Resources["BaseTopBar"] = new SolidColorBrush(s_BaseTopBar);
            Application.Current.Resources["BaseToolBar"] = new SolidColorBrush(s_BaseToolBar);

            Application.Current.Resources["BaseAccent"] = new SolidColorBrush(s_BaseAccent);
            Application.Current.Resources["BaseAccentHover"] = new SolidColorBrush(s_BaseAccentHover);
            Application.Current.Resources["BaseAccentPress"] = new SolidColorBrush(s_BaseAccentPress);

            Application.Current.Resources["BaseForegroundError"] = new SolidColorBrush(s_BaseForegroundError);
            Application.Current.Resources["BaseForegroundTitle"] = new SolidColorBrush(s_BaseForegroundTitle);
            Application.Current.Resources["BaseForeground"] = new SolidColorBrush(s_BaseForeground);
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

            if (s_CurrentTheme.Name.ToLower() == "dark")
            {
                RestoreDefault();
                return;
            }

            Application.Current.Resources["BaseBackground"]  = GetThemedColor(s_BaseBackground, s_CurrentTheme.Accent, s_CurrentTheme.IsDarkTheme);
            Application.Current.Resources["BaseButton"]      = GetThemedColor(s_BaseButton, s_CurrentTheme.Accent, s_CurrentTheme.IsDarkTheme);
            Application.Current.Resources["BaseButtonHover"] = GetThemedColor(s_BaseButtonHover, s_CurrentTheme.Accent, s_CurrentTheme.IsDarkTheme);
            Application.Current.Resources["BaseButtonPress"] = GetThemedColor(s_BaseButtonPress, s_CurrentTheme.Accent, s_CurrentTheme.IsDarkTheme);
            Application.Current.Resources["BaseSidebar"]     = GetThemedColor(s_BaseSidebar, s_CurrentTheme.Accent, s_CurrentTheme.IsDarkTheme);
            Application.Current.Resources["BaseTopBar"]      = GetThemedColor(s_BaseTopBar, s_CurrentTheme.Accent, s_CurrentTheme.IsDarkTheme);
            Application.Current.Resources["BaseToolBar"]     = GetThemedColor(s_BaseToolBar, s_CurrentTheme.Accent, s_CurrentTheme.IsDarkTheme);

            Application.Current.Resources["BaseSelectionHover"] = GetThemedColor(s_ThemeSelectionHover, s_CurrentTheme.Accent, false);

            Application.Current.Resources["BaseAccent"]      = s_CurrentTheme.Accent;
            Application.Current.Resources["BaseAccentHover"] = GetThemedColor(s_BaseAccentHover, s_CurrentTheme.Accent, false);
            Application.Current.Resources["BaseAccentPress"] = GetThemedColor(s_BaseAccentPress, s_CurrentTheme.Accent, false);

            if (s_CurrentTheme.IsDarkTheme)
            {
                Application.Current.Resources["BaseForeground"] = new SolidColorBrush(s_BaseForeground);
                Application.Current.Resources["BaseForegroundTitle"] = new SolidColorBrush(s_BaseForegroundTitle);
            }
            else
            {
                Application.Current.Resources["BaseForeground"] = new SolidColorBrush(s_BaseForegroundLight);
                Application.Current.Resources["BaseForegroundTitle"] = new SolidColorBrush(s_BaseForegroundTitleLight);
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

        private static SolidColorBrush GetThemedColor(Color baseColor, Color targetColor, bool isDarkMode)
        {
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
}