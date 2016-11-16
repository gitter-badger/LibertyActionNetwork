using System;
using System.Text.RegularExpressions;

namespace Li.Lan.Common.Extensions
{
    public class RegexUtilities
    {
        public static bool IsValidRegexPattern(string pattern)
        {
            if (String.IsNullOrWhiteSpace(pattern)) return false;

            try { var r = new Regex(pattern); }
            catch { return false; }

            return true;
        }
    }
}