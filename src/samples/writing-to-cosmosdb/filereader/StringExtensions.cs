using System;
using System.Collections.Generic;
using System.Globalization;
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
            var numberFormatInfo = (NumberFormatInfo)CultureInfo.CurrentCulture.NumberFormat.Clone();
            numberFormatInfo.CurrencyDecimalSeparator = ".";
            return float.TryParse(text, NumberStyles.Float, numberFormatInfo, out fValue);

        }
    }
}