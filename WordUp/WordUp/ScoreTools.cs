using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WordUp
{
    public static class ScoreTools
    {
        /// <summary>
        /// Calculate word score
        /// </summary>
        public static int GetWordScore(string s)
        {
            int totalScore = 0;
            
            // 1.
            // Convert to char array.
            char[] a = s.ToCharArray();

           // 2. Calculate score based on Scrabble (R) rules
           // Source: www.hasbro.com/scrabble/en_US/discover/faq.cfm
           foreach(char c in s)
           {
               if (c == 'q' || c == 'z') { totalScore += 10; }
               else if (c == 'j' || c == 'x') { totalScore += 8;  }
               else if (c == 'k') { totalScore += 5; }
               else if (c == 'f' || c == 'h' || c == 'v' || c == 'w' || c == 'y') { totalScore += 4; }
               else if (c == 'b' || c == 'c' || c == 'm' || c == 'p') { totalScore += 3; }
               else if (c == 'd' || c == 'g') { totalScore += 2; }
               else { totalScore += 1; }
           }
           
            
            // 3.
            // Multiply score by number of letters
            totalScore *= s.Length;

            // 3.
            // Return modified string.
            return totalScore;
        }
    }
}
