namespace Client.Managers;

using Avalonia;
using Avalonia.Platform;

using System;
using System.Collections.Generic;
using System.IO;

internal class LanguageManager
{
    public static Language[] Languages;

    private static Language s_CurrentLanguage;
    private static Dictionary<string, string[]> s_Languages = new Dictionary<string, string[]>();

    public static void Initialize()
    {
        Languages = new Language[] { new Language("en-GB", "\ud83c\uddec\ud83c\udde7", "English (UK)"),
                                     new Language("en-US", "\ud83c\uddfa\ud83c\uddf8", "English (US)"),
                                     new Language("fr-FR", "\ud83c\uddeb\ud83c\uddf7", "Français"),
                                     new Language("zh-CN", "\ud83c\udde8\ud83c\uddf3", "中文"),
                                     new Language("de-DE", "\ud83c\udde9\ud83c\uddea", "Deutsch"),
                                     new Language("es-ES", "\ud83c\uddea\ud83c\uddf8", "Español") };

        for (int i = 0; i < Languages.Length; i++)
            Languages[i].Id = i;

        IAssetLoader loader = AvaloniaLocator.Current.GetService<IAssetLoader>();

        for (int i = 0; i < Languages.Length; i++)
        {
            string langCode = Languages[i].LangCode;

            using Stream fileStream = loader.Open(new Uri($"avares://Client/Assets/{langCode}.lang"));
            using StreamReader fileReader = new StreamReader(fileStream);

            s_Languages[langCode] = fileReader.ReadToEnd().Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        }

        SetLanguage(Configuration.Language);
    }

    public static int RestoreDefault()
    {
        return SetLanguage(Configuration.Language);
    }

    public static int SetLanguage(string langCode)
    {
        string actualLangCode = string.IsNullOrWhiteSpace(langCode) ? Configuration.Language : langCode;
        for (int i = 0; i < Languages.Length; i++)
        {
            if (Languages[i].LangCode == actualLangCode)
                s_CurrentLanguage = Languages[i];
        }

        if (s_CurrentLanguage == null)
            return -1;

        if (!s_Languages.ContainsKey(actualLangCode))
            return -1;

        string[] languageData = s_Languages[actualLangCode];
        for (int i = 0; i < languageData.Length; i++)
        {
            string[] keyValuePair = languageData[i].Split('=');
            if (keyValuePair.Length != 2)
                continue;

            Application.Current.Resources[keyValuePair[0]] = keyValuePair[1].Replace(@"\n", "\n");
        }

        return s_CurrentLanguage.Id;
    }

    public static Language GetLanguageByName(string langCode)
    {
        if (string.IsNullOrWhiteSpace(langCode))
            return null;

        for (int i = 0; i < Languages.Length; i++)
        {
            if (Languages[i].LangCode == langCode)
                return Languages[i];
        }

        return null;
    }

    public static Language GetCurrentLanguage()
    {
        return s_CurrentLanguage;
    }
}

internal class Language
{
    public string LangCode { get; set; }
    public string LangSymbol { get; set; }
    public string DisplayName { get; set; }
    public int Id { get; set; }

    public Language(string langCode, string langSymbol, string displayName)
    {
        LangCode = langCode;
        LangSymbol = langSymbol;
        DisplayName = displayName;
    }
}