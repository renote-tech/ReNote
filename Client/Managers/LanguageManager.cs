using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Avalonia;
using Avalonia.Platform;

namespace Client.Managers
{
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
                                         new Language("zh-CN", "\ud83c\udde8\ud83c\uddf3", "\u4e2d\u6587"),
                                         new Language("es-ES", "\ud83c\uddea\ud83c\uddf8", "Español"),
                                         new Language("de-DE", "\ud83c\udde9\ud83c\uddea", "Deutsch") };
            
            for (int i = 0; i < Languages.Length; i++)
                Languages[i].Id = i;

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
}