namespace Server.Common.Utilities
{
    public class NumberUtil
    {
        /// <summary>
        /// Returns whether the <see cref="string"/> is a valid <see cref="long"/> value.
        /// </summary>
        /// <param name="data">The <see cref="string"/> to be proceeded.</param>
        /// <returns><see cref="bool"/></returns>
        public static bool IsSafeLong(string data)
        {
            if (!StringUtil.ContainsDigitsOnly(data))
                return false;

            return long.TryParse(data, out _);
        }
    }
}