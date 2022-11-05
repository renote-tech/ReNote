using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Utilities
{
    public class StringUtil
    {
        public static int GetLineNumber(string data, string search)
        {
            string[] lines = data.Split('\n');
            for(int i = 0; i < lines.Length; i++)
            {
                if (lines[i] == search)
                    return i;
                else if (lines[i].Contains(search))
                    return i;
            }
            return -1;
        }
    }
}
