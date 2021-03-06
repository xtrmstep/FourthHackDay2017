﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace presenter.predictions
{
    public static class StringExtensions
    {
        public static DateTime ToDateTime(this string text)
        {
            return DateTime.ParseExact(text, "M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);
        }
    }
}
