#if DEBUG

using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Client.Managers;

using System;

namespace Client.Windows;

public partial class ThemeColorPicker : Window
{
    private static byte s_RedColor;
    private static byte s_GreenColor;
    private static byte s_BlueColor;
    private static bool s_IsDarkMode;

    public ThemeColorPicker()
    {
        InitializeComponent();

        if (Design.IsDesignMode)
            return;

        InitializeEvents();
    }

    private void InitializeEvents()
    {
        m_SliderRed.GetObservable(Slider.ValueProperty).Subscribe((value) =>
        {
            if (!IsVisible)
                return;

            byte byteValue = (byte)(m_SliderRed.Value * 255 / 100);
            s_RedColor = byteValue;
            m_RedText.Text = $"Red ({byteValue})";

            SetTheme(new Color(255, s_RedColor, s_GreenColor, s_BlueColor), s_IsDarkMode);
        });


        m_SliderGreen.GetObservable(Slider.ValueProperty).Subscribe((value) =>
        {
            if (!IsVisible)
                return;

            byte byteValue = (byte)(m_SliderGreen.Value * 255 / 100);
            s_GreenColor = byteValue;
            m_GreenText.Text = $"Green ({byteValue})";

            SetTheme(new Color(255, s_RedColor, s_GreenColor, s_BlueColor), s_IsDarkMode);
        });

        m_SliderBlue.GetObservable(Slider.ValueProperty).Subscribe((value) =>
        {
            if (!IsVisible)
                return;

            byte byteValue = (byte)(m_SliderBlue.Value * 255 / 100);
            s_BlueColor = byteValue;
            m_BlueText.Text = $"Blue ({byteValue})";

            SetTheme(new Color(255, s_RedColor, s_GreenColor, s_BlueColor), s_IsDarkMode);
        });

        PointerPressed += (sender, e) =>
        {
            s_IsDarkMode = !s_IsDarkMode;
            SetTheme(new Color(255, s_RedColor, s_GreenColor, s_BlueColor), s_IsDarkMode);
        };

        Opened += (sender, e) =>
        {
            m_SliderRed.Value   = s_RedColor * 100 / 255.0;
            m_SliderGreen.Value = s_GreenColor * 100 / 255.0;
            m_SliderBlue.Value  = s_BlueColor * 100 / 255.0;
        };
    }

    public void SetTheme(Color color, bool isDarkTheme)
    {
        if (ThemeManager.s_DefaultTheme.Count == 0)
            return;

        Background = new SolidColorBrush(color);

        Application.Current.Resources["BaseBackground"] = GetThemedColor("BaseBackground", color, isDarkTheme);

        Application.Current.Resources["BaseButton"] = GetThemedColor("BaseButton", color, isDarkTheme);
        Application.Current.Resources["BaseButtonHover"] = GetThemedColor("BaseButtonHover", color, isDarkTheme);
        Application.Current.Resources["BaseButtonDisabled"] = GetThemedColor("BaseButtonDisabled", color, isDarkTheme);

        Application.Current.Resources["BaseSidebar"] = GetThemedColor("BaseSidebar", color, isDarkTheme);
        Application.Current.Resources["BaseTopBar"] = GetThemedColor("BaseTopBar", color, isDarkTheme);
        Application.Current.Resources["BaseToolBar"] = GetThemedColor("BaseToolBar", color, isDarkTheme);

        Application.Current.Resources["BaseMenu"] = GetThemedColor("BaseMenu", color, isDarkTheme);
        Application.Current.Resources["BaseMenuButton"] = GetThemedColor("BaseMenuButton", color, isDarkTheme);
        Application.Current.Resources["BaseMenuButtonHover"] = GetThemedColor("BaseMenuButtonHover", color, isDarkTheme);

        Application.Current.Resources["BaseBackgroundDialog"] = GetThemedColor("BaseBackgroundDialog", color, isDarkTheme);

        Application.Current.Resources["BaseSelectionHover"] = GetThemedColor("BaseSelectionHover", color, false);

        Application.Current.Resources["BaseForegroundGray"] = GetThemedColor("BaseForegroundGray", color, isDarkTheme);

        Application.Current.Resources["BaseAccent"] = color;
        Application.Current.Resources["BaseAccentHover"] = GetThemedColor("BaseAccentHover", color, false);
        Application.Current.Resources["BaseAccentPress"] = GetThemedColor("BaseAccentPress", color, false);

        if (isDarkTheme)
        {
            Application.Current.Resources["BaseForeground"] = ThemeManager.s_DefaultTheme["BaseForeground"];
            Application.Current.Resources["BaseForegroundTitle"] = ThemeManager.s_DefaultTheme["BaseForegroundTitle"];
        }
        else
        {
            Application.Current.Resources["BaseForeground"] = ThemeManager.s_LightTheme["BaseForegroundLight"];
            Application.Current.Resources["BaseForegroundTitle"] = ThemeManager.s_LightTheme["BaseForegroundTitleLight"];
        }
    }

    private static SolidColorBrush GetThemedColor(string type, Color targetColor, bool isDarkMode)
    {
        ThemeManager.s_DefaultTheme.TryGetValue(type, out SolidColorBrush brush);

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

#endif