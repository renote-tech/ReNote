using System;
using System.Collections.Generic;
using System.IO;
using Avalonia;
using Avalonia.Platform;

namespace Client
{
    internal class Language
    {
        public static List<Language> LanguageList;

        public string LangCode { get; set; }
        public string LangSymbol { get; set; }
        public string DisplayName { get; set; }

        private static string s_CurrentLanguage;
        private static Dictionary<string, string[]> s_Languages = new Dictionary<string, string[]>();

        public Language()
        { }

        public Language(string langCode, string langSymbol, string displayName)
        {
            LangCode = langCode;
            LangSymbol = langSymbol;
            DisplayName = displayName;
        }

        public static void Initialize()
        {
            LanguageList = new List<Language>() { new Language("en_GB", "\ud83c\uddec\ud83c\udde7", "English (UK)"),
                                                  new Language("en_US", "\ud83c\uddfa\ud83c\uddf8", "English (US)"),
                                                  new Language("fr_FR", "\ud83c\uddeb\ud83c\uddf7", "Français"),
                                                  new Language("zh_CN", "\ud83c\udde8\ud83c\uddf3", "\u4e2d\u6587"),
                                                  new Language("es_ES", "\ud83c\uddea\ud83c\uddf8", "Español"),
                                                  new Language("de_DE", "\ud83c\udde9\ud83c\uddea", "Deutsch") };

            SetLanguage(Configuration.Language);

            IAssetLoader loader = AvaloniaLocator.Current.GetService<IAssetLoader>();

            using Stream enGbStream = loader.Open(new Uri("avares://Client/Assets/en_GB.lang"));
            using Stream frFRStream = loader.Open(new Uri("avares://Client/Assets/fr_FR.lang"));
            using Stream deDEStream = loader.Open(new Uri("avares://Client/Assets/de_DE.lang"));
            using StreamReader enGbReader = new StreamReader(enGbStream);
            using StreamReader frFRReader = new StreamReader(frFRStream);
            using StreamReader deDEReader = new StreamReader(deDEStream);

            s_Languages["en_GB"] = enGbReader.ReadToEnd().Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            s_Languages["fr_FR"] = frFRReader.ReadToEnd().Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            s_Languages["de_DE"] = deDEReader.ReadToEnd().Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        }

        public static string GetString(string langKey)
        {
            if (s_CurrentLanguage == null)
                return langKey;

            if (!s_Languages.ContainsKey(s_CurrentLanguage))
                return langKey;

            string[] languageData = s_Languages[s_CurrentLanguage];
            if (languageData == null || languageData.Length == 0)
                return langKey;

            for (int i = 0; i < languageData.Length; i++)
            {
                string[] keyValue = languageData[i].Split('=');
                if (keyValue.Length != 2)
                    return langKey;

                if (keyValue[0] == langKey)
                    return keyValue[1].Replace(@"\n", "\n");
            }

            return langKey;
        }

        public static void SetLanguage(string langCode = "en_GB")
        {
            if (string.IsNullOrWhiteSpace(langCode))
            {
                s_CurrentLanguage = "en_GB";
                return;
            }

            s_CurrentLanguage = langCode;
        }

        public static string GetCurrentLanguage()
        {
            return s_CurrentLanguage;
        }
    }
}