using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GenerateDocument.Common.Helpers
{
    public static class NameHelper
    {
        public static string RandomName(int length)
        {
            const string Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var randomString = new StringBuilder();
            var random = new Random();
            for (int i = 0; i < length; i++)
            {
                randomString.Append(Chars[random.Next(Chars.Length)]);
            }
            return randomString.ToString();
        }

        public static string CorrectFileNameOnWindows(this string input)
        {
            var regEx = new Regex("[\\\\/|<|>*/:?\"]");
            return regEx.Replace(input, "_");
        }
    }
}
