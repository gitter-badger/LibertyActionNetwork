using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Li.Lan.Common.Extensions
{
    public static class StringExtensions
    {
        public const string ValidFileNameChacatersRegex = "[A-Za-z0-9]";
        public const string InvalidFileNameChacatersRegex = "[^A-Za-z0-9]";

        public static string ToTitleCase(this string str)
        {
            if (String.IsNullOrWhiteSpace(str))
                return str;

            return Regex.Replace(str, @"\w+", (m) =>
            {
                string tmp = m.Value;
                return char.ToUpper(tmp[0]) + tmp.Substring(1, tmp.Length - 1);
            });
        }

        /// <summary>
        /// Splits the string on Environment.NewLine and uses StringSplitOptions.RemoveEmptyEntries.
        /// Also removes entries that are only whitespace.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string[] Lines(this string str)
        {
            var lines = str.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            return lines.Where(x => !String.IsNullOrWhiteSpace(x)).ToArray();
        }

        public static string ToSafeFileName(this string str)
        {
            var regex = new Regex(InvalidFileNameChacatersRegex);

            return regex.Replace(str, "");
        }
    }
}