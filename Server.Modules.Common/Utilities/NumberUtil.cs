namespace Server.Common.Utilities
{
    public class NumberUtil
    {
        public static bool IsSafeLong(string data)
        {
            if (!StringUtil.ContainsDigitsOnly(data))
                return false;

            return long.TryParse(data, out _);
        }
    }
}