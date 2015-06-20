using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace WordUp
{
    /// <summary>
    /// Processes input keys 
    /// </summary>
    public static class KeyboardProcessor
    {
        
        /// <summary>
        /// 
        /// </summary>
        private static Dictionary<Keys, bool> keyPressedDictionary = new Dictionary<Keys, bool>();


        /// <summary>
        /// Initializes the keyPressedDictionary with false values
        /// </summary>
        static KeyboardProcessor()
        {
            for (Alphabet letter = Alphabet.A; letter <= Alphabet.Z; letter++)
            {
                keyPressedDictionary.Add((Keys)letter, false);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyboard"></param>
        /// <returns></returns>
        public static char GetLetter(KeyboardState keyboard)
        {
            for (Alphabet letter = Alphabet.A; letter <= Alphabet.Z; letter++)
            {
                Keys keyToCheck = (Keys)letter;
                
                // Check whether key had been pressed earlier
                if(keyPressedDictionary[keyToCheck])
                {
                    // Check if released
                    if(keyboard.IsKeyUp(keyToCheck))
                    {
                        // Key released, set pressed flag back to false
                        keyPressedDictionary[keyToCheck] = false;
                        Debug.WriteLine("GetLetter: " + keyToCheck);
                        return keyToCheck.ToString().ToLower()[0];
                    }
                }
                else if(keyboard.IsKeyDown(keyToCheck))
                {
                    keyPressedDictionary[keyToCheck] = true;
                }
            }

            return ' ';

        }
        /// <summary>
        /// Checks whether the key pressed is a letter
        /// </summary>
        /// <param name="letter"></param>
        /// <returns></returns>
        public static bool IsLetter(Keys key)
        {
            char letter = key.ToString().Trim().ToLower()[0];

            Debug.WriteLine("(IsLetter(): Checking letter: " + letter);
            

            for(char c = 'a'; c <= 'z'; c++)
            {
                if(c == letter)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
