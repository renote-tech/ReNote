using System.Collections.Generic;

namespace Server.ReNote
{
    public class PluginManager
    {
        public static Dictionary<string, bool> Features = new Dictionary<string, bool>();
        public static Dictionary<int, string> FeatureTypes = new Dictionary<int, string>();

        public static void Initialize(Dictionary<string, bool> features)
        {
            Features = features;

            FeatureTypes.Add(Constants.PUBLIC_AUTH_ID,  PluginTypes.GLOBAL);
            FeatureTypes.Add(Constants.SHARED_AUTH_ID,  PluginTypes.USER);
            FeatureTypes.Add(Constants.STUDENT_AUTH_ID, PluginTypes.STUDENT);
            FeatureTypes.Add(Constants.TEACHER_AUTH_ID, PluginTypes.TEACHER);
        }

        public static bool Enabled(int featureType, string featureName)
        {
            if (!FeatureTypes.ContainsKey(featureType))
                return false;

            string fullFeatureName = $"{FeatureTypes[featureType]}.{featureName}";
            if (Features.ContainsKey(fullFeatureName))
            {
                Features.TryGetValue(fullFeatureName, out bool enabled);
                return enabled;
            }

            if (featureType > Constants.PUBLIC_AUTH_ID && featureType < Constants.SHARED_AUTH_ID)
                return Enabled(Constants.SHARED_AUTH_ID, featureName);

            return false;
        }
    }

    public struct PluginTypes
    {
        public const string GLOBAL  = "global";
        public const string USER    = "user";
        public const string STUDENT = "student";
        public const string TEACHER = "teacher";
    }

    public struct Plugins
    {
        public const string KN_SHOW_ID          = "show_id";
        public const string KN_KEDIT_CONTACT    = "edit_contact";
        public const string KN_CHANGE_PASSWORD  = "change_password";
        public const string EX_NEW_LOGIN_DIALOG = "login_new_dialog";
    }
}