using System;
using System.Collections.Generic;
using System.IO;
using Avalonia;
using Avalonia.Platform;

namespace Client.Managers
{
    internal class LanguageManager
    {
        public static List<Language> LanguageList;

        private static string s_CurrentLanguage;
        private static Dictionary<string, string[]> s_Languages = new Dictionary<string, string[]>();

        public static void Initialize()
        {
            LanguageList = new List<Language>() { new Language("en-GB", "\ud83c\uddec\ud83c\udde7", "English (UK)"),
                                                  new Language("en-US", "\ud83c\uddfa\ud83c\uddf8", "English (US)"),
                                                  new Language("fr-FR", "\ud83c\uddeb\ud83c\uddf7", "Français"),
                                                  new Language("zh-CN", "\ud83c\udde8\ud83c\uddf3", "\u4e2d\u6587"),
                                                  new Language("es-ES", "\ud83c\uddea\ud83c\uddf8", "Español"),
                                                  new Language("de-DE", "\ud83c\udde9\ud83c\uddea", "Deutsch") };

            IAssetLoader loader = AvaloniaLocator.Current.GetService<IAssetLoader>();

            using Stream enGbStream = loader.Open(new Uri("avares://Client/Assets/en-GB.lang"));
            using Stream enUsStream = loader.Open(new Uri("avares://Client/Assets/en-US.lang"));
            using Stream frFrStream = loader.Open(new Uri("avares://Client/Assets/fr-FR.lang"));
            using Stream deDeStream = loader.Open(new Uri("avares://Client/Assets/de-DE.lang"));
            using StreamReader enGbReader = new StreamReader(enGbStream);
            using StreamReader enUsReader = new StreamReader(enUsStream);
            using StreamReader frFRReader = new StreamReader(frFrStream);
            using StreamReader deDEReader = new StreamReader(deDeStream);

            s_Languages["en-GB"] = enGbReader.ReadToEnd().Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            s_Languages["en-US"] = enUsReader.ReadToEnd().Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            s_Languages["fr-FR"] = frFRReader.ReadToEnd().Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            s_Languages["de-DE"] = deDEReader.ReadToEnd().Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            SetLanguage(Configuration.Language);
        }


        public static void SetLanguage(string langCode = "en-GB")
        {
            if (string.IsNullOrWhiteSpace(langCode))
                s_CurrentLanguage = "en_GB";
            else
                s_CurrentLanguage = langCode;

            if (!s_Languages.ContainsKey(s_CurrentLanguage))
                return;

            string[] languageData = s_Languages[s_CurrentLanguage];
            for (int i = 0; i < languageData.Length; i++)
            {
                string[] keyValuePair = languageData[i].Split('=');
                if (keyValuePair.Length != 2)
                    continue;

                Application.Current.Resources[keyValuePair[0]] = keyValuePair[1].Replace(@"\n", "\n");
            }
        }

        public static string GetCurrentLanguage()
        {
            return s_CurrentLanguage;
        }
    }

    internal class Language
    {
        public string LangCode { get; set; }
        public string LangSymbol { get; set; }
        public string DisplayName { get; set; }

        public Language(string langCode, string langSymbol, string displayName)
        {
            LangCode = langCode;
            LangSymbol = langSymbol;
            DisplayName = displayName;
        }
    }
}