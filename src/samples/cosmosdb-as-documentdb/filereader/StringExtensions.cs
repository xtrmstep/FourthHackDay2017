using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace feeder.filereader
{
    static class StringExtensions
    {
        public static bool IsNumeric(this string text)
        {
            float fValue;
            return float.TryParse(text, out fValue);

        }
    }
}