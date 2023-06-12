namespace Client.Managers;

using Avalonia;
using Avalonia.Platform;
using System;
using System.Collections.Generic;
using System.IO;

internal class LanguageManager
{
    public static Language[] Languages;
    public static Language CurrentLanguage;

    private static readonly Dictionary<string, string[]> s_Languages = new Dictionary<string, string[]>();

    public static void Initialize()
    {
        Languages = new Language[] { new Language("en-GB", "\ud83c\uddec\ud83c\udde7", "English (UK)"),
                                     new Language("en-US", "\ud83c\uddfa\ud83c\uddf8", "English (US)"),
                                     new Language("fr-FR", "\ud83c\uddeb\ud83c\uddf7", "Français"),
                                     new Language("zh-CN", "\ud83c\udde8\ud83c\uddf3", "中文"),
                                     new Language("de-DE", "\ud83c\udde9\ud83c\uddea", "Deutsch"),
                                     new Language("es-ES", "\ud83c\uddea\ud83c\uddf8", "Español") };

        IAssetLoader loader = AvaloniaLocator.Current.GetService<IAssetLoader>();

        for (int i = 0; i < Languages.Length; i++)
        {
            string langCode = Languages[i].LangCode;

            using Stream fileStream = loader.Open(new Uri($"avares://Client/Assets/{langCode}.lang"));
            using StreamReader fileReader = new StreamReader(fileStream);

            s_Languages[langCode] = fileReader.ReadToEnd().Split('\n', StringSplitOptions.RemoveEmptyEntries);
        }

        SetLanguage(Configuration.Language);
    }

    public static int SetLanguage(string langCode)
    {
        if (string.IsNullOrWhiteSpace(langCode))
            langCode = Configuration.Language;

        for (int i = 0; i < Languages.Length; i++)
        {
            if (Languages[i].LangCode == langCode)
                CurrentLanguage = Languages[i];
        }

        if (CurrentLanguage == null || !s_Languages.ContainsKey(langCode))
            return -1;

        string[] languageData = s_Languages[langCode];
        for (int i = 0; i < languageData.Length; i++)
        {
            string[] keyValuePair = languageData[i].Split('=');
            if (keyValuePair.Length != 2)
                continue;

            Application.Current.Resources[keyValuePair[0]] = keyValuePair[1].Replace(@"\n", "\n");
        }

        return CurrentLanguage.Id;
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
}

internal class Language
{
    public int Id { get; }
    public string LangCode { get; }
    public string LangSymbol { get; }
    public string DisplayName { get; }

    private static int s_NextLangId;

    public Language(string langCode, string langSymbol, string displayName)
    {
        LangCode = langCode;
        LangSymbol = langSymbol;
        DisplayName = displayName;

        Id = s_NextLangId++; // Post-incrementation: returns the current value & increment itself
    }
}