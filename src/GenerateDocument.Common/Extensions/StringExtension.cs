using System;
using System.Text;
using System.Text.RegularExpressions;

namespace GenerateDocument.Common.Extensions
{
    public static class StringExtension
    {
        public static bool IsContains(this string source, string toCheck, StringComparison comp = StringComparison.OrdinalIgnoreCase)
        {
            return source?.IndexOf(toCheck, comp) >= 0;
        }

        public static bool IsEquals(this string source, string toCheck, StringComparison comp = StringComparison.OrdinalIgnoreCase)
        {
            return string.Equals(source.Trim(), toCheck.Trim(), comp);
        }

        public static string EncodeSpecialCharacters(this string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new Exception("Text verify must be not nullable");
            }

            return text.Replace("<", "&lt;").Replace(">", "&gt;");
        }

        
    }
}
