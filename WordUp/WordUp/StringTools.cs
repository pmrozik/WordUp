using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WordUp
{
    public static class StringTools
    {
        /// <summary>
        /// Alphabetize the characters in the string.
        /// </summary>
        public static string Alphabetize(string s)
        {
            // 1.
            // Convert to char array.
            char[] a = s.ToCharArray();

            // 2.
            // Sort letters.
            Array.Sort(a);

            // 3.
            // Return modified string.
            return new string(a);
        }
    }
}
