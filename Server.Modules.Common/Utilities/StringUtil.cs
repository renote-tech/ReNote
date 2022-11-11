namespace Server.Common.Utilities
{
    public class StringUtil
    {
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