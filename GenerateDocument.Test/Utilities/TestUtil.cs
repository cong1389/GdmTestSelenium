using System;
using System.Text;
using System.Text.RegularExpressions;

namespace GenerateDocument.Test.Utilities
{
    public static class TestUtil
    {
        

        public static string RandomName(string[] names)
        {
            var random = new Random();
            var index = random.Next(0, names.Length);

            return names[index];
        }

        public static string RemoveSpecialChars(this string input)
        {
            return Regex.Replace(input, @"[^0-9a-zA-Z\._]", string.Empty);
        }

       
    }
}
