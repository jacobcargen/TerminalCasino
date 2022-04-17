using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace TerminalCasino
{
    static class Tools
    {


        /// <summary>
        /// Call this to say something to the user.
        /// </summary>
        /// <param name="txt"></param>
        public static void TellUser(string txt)
        {
            Console.WriteLine(txt); // Print text to the user
        }

        /// <summary>
        /// Call this to tell something to the user and get input back from the user.
        /// </summary>
        /// <param name="txt"></param>
        /// <returns>User input in the form of a string</returns>
        public static string GetInputString(string txt)
        {
            Console.Write(txt); // Print text to the user
            var input = Console.ReadLine(); // Allow the user to give input
            Console.WriteLine(); // Blank space
            return input; // Return the input
        }

        /// <summary>
        /// Call this to tell something to the user and get input back from the user.
        /// </summary>
        /// <param name="txt"></param>
        /// <returns>User input in the form of a console key</returns>
        public static ConsoleKey GetInputKey(string txt)
        {
            Console.Write(txt); // Print text to the user
            var input = Console.ReadKey(); // Allow the user to give a key
            Console.WriteLine(); // Blank space
            return input.Key; // Return the input in a ConsoleKey type
        }

        /// <summary>
        /// Creates a directory if one is needed.
        /// </summary>
        public static void CreateDirIfNone(string dir)
        {
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
        }


        public static void WantToPlayAgain(Action cmd)
        {
            while(true)
            {
                // If no money then gameover
                if (Program.user.money < 1) break;

                ConsoleKey input = GetInputKey("Press Space to play again, press Enter to exit.");
                switch (input)
                {
                    case ConsoleKey.Spacebar:
                        cmd();
                        break;
                    case ConsoleKey.Enter: return;
                }
            }
        }
    }
}
