namespace Client.Managers;

using Client.ReNote.Data;
using System.Collections.Generic;

internal class PluginManager
{
    public static Dictionary<string, bool> Features = new Dictionary<string, bool>();
    public static List<string> FeatureTypes = new List<string>();

    public static void Initialize(Dictionary<string, bool> features)
    {
        Features = features;

        FeatureTypes.AddRange(new string[] {
            PluginTypes.GLOBAL,
            PluginTypes.USER,
            PluginTypes.STUDENT,
            PluginTypes.TEACHER,
            PluginTypes.EXPERIMENTAL
        });
    }

    public static bool Enabled(string featureType, string featureName)
    {
        featureType = featureType.ToLower();
        if (!FeatureTypes.Contains(featureType))
            return false;

        string fullFeatureName = $"{featureType}.{featureName}";
        if (!Features.ContainsKey(fullFeatureName))
            return false;

        Features.TryGetValue(fullFeatureName, out bool enabled);
        return enabled;
    }
}

internal struct PluginTypes
{
    public const string GLOBAL       = "global";
    public const string USER         = "user";
    public const string STUDENT      = "student";
    public const string TEACHER      = "teacher";
    public const string EXPERIMENTAL = "experimental";

    public static string AUTO
    {
        get
        {
            if (User.Current == null)
                return GLOBAL;

            switch (User.Current.AccountType)
            {
                case 4:
                    return STUDENT;
                case 8:
                    return TEACHER;
                default:
                    return USER;
            }
        }
    }
}

internal struct Plugins
{
    public const string KN_SHOW_ID          = "show_id";
    public const string KN_KEDIT_CONTACT    = "edit_contact";
    public const string KN_CHANGE_PASSWORD  = "change_password";
    public const string EX_NEW_LOGIN_DIALOG = "login_new_dialog";
}