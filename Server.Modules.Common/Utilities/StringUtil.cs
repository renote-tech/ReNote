namespace Server.Common.Utilities
{
    public class StringUtil
    {
        /// <summary>
        /// Returns whether the <see cref="string"/> only contains digits.
        /// </summary>
        /// <param name="data">The <see cref="string"/> to be proceeded.</param>
        /// <returns><see cref="bool"/></returns>
        public static bool ContainsDigitsOnly(string data)
        {
            bool result = true;
            for(int i = 0; i < data.Length; i++)
            {
                if (!char.IsDigit(data[i]))
                    result = false;
            }

            return result;
        }
    }
}