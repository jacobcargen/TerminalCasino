using System;
using System.Collections.Generic;
using System.IO;

namespace TerminalCasino
{
    class Program
    {
        // The program will close if this is set to true
        public static bool isQuit = false;
        // The users player
        public static Player user = null;
        // If saving progress is enabled
        private static bool isSaving = true;
        // 
        private static bool isMusicPlaying = false;

        
        
        /// <summary>
        /// Loads the saved progress or creates a new one if none exist
        /// </summary>
        public static void LoadUser()
        {
            // Loads the saved progress
            user = new Player(Saving.LoadProgress());
            user.isUser = true;
        }

        /// <summary>
        /// Saves the current user progress
        /// </summary>
        public static void SaveUser()
        {
            if (isSaving)
            {
                user.isUser = true;
                // Saves the current progress
                Saving.SaveProgress(new SaveData(user));
            }
        }

        /// <summary>
        /// The player is out of money, RESET game
        /// </summary>
        private static void GameOver()
        {
            Saving.DeleteSave();
            Commands.Clear();
            Tools.TellUser("GAME OVER :[");
            Console.ReadKey();
            return;
        }
        /// <summary>
        /// Called at the beginning of the program.
        /// </summary>
        private static void Welcome()
        {
            // Make sure there is an audio folder
            Audio.CheckDir();
            // Loads the saved progress (if none, one is created)
            LoadUser();

            // Clears the terminal
            Commands.Clear();
            // Sets the title
            Console.Title = "Terminal Casino";
            // Welcome audio
            Audio.WelcomeSound();
            // Music
            PlayMenuMusic();

            // First time playing (empty name)
            if (user.name == string.Empty)
            {
                // Ask user to enter a name
                Commands.ChangeName();
                // Clear console
                Commands.Clear();
                // Breif welcome msg for the user
                Tools.TellUser(@" _____                   _             _    ____          _             
|_   _|__ _ __ _ __ ___ (_)_ __   __ _| |  / ___|__ _ ___(_)_ __   ___  
  | |/ _ \ '__| '_ ` _ \| | '_ \ / _` | | | |   / _` / __| | '_ \ / _ \ 
  | |  __/ |  | | | | | | | | | | (_| | | | |__| (_| \__ \ | | | | (_) |
  |_|\___|_|  |_| |_| |_|_|_| |_|\__,_|_|  \____\__,_|___/_|_| |_|\___/");
                Tools.TellUser($"\n! ! ! WELCOME TO TERMINAL CASINO {user.name.ToUpper()} ! ! !\n");
                // Gives a starting help msg
                Commands.Help();
            }
            // Returning player
            else
            {
                // Breif welcome msg for the user
                Tools.TellUser(@" _____                   _             _    ____          _             
|_   _|__ _ __ _ __ ___ (_)_ __   __ _| |  / ___|__ _ ___(_)_ __   ___  
  | |/ _ \ '__| '_ ` _ \| | '_ \ / _` | | | |   / _` / __| | '_ \ / _ \ 
  | |  __/ |  | | | | | | | | | | (_| | | | |__| (_| \__ \ | | | | (_) |
  |_|\___|_|  |_| |_| |_|_|_| |_|\__,_|_|  \____\__,_|___/_|_| |_|\___/");
                Tools.TellUser($"\n! ! ! WELCOME BACK {user.name.ToUpper()} ! ! !");
                //Commands.Stats();
            }
        }

        public static void StopMenuMusic()
        {
            Audio.PlayMenuMusic(false);
        }
        private static void PlayMenuMusic()
        {
            Audio.PlayMenuMusic(true);
        }

        /// <summary>
        /// Called at the start of the program
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Welcome();

            // Loop this while the user has not choose to quit the program
            do
            {
                // Check if music is playing
                if (!isMusicPlaying)
                {
                    // Play menu music
                    PlayMenuMusic();
                }

                // Save
                Program.SaveUser();
                if (user.money < 1) break;
                // Main Input line
                string input = Tools.GetInputString("\nTerminal Casino > ");
                // Check the input for a vlid command
                if (!Commands.CheckCommand(input))
                {
                    Commands.CmdNotFound();
                }
            }
            while (!isQuit);

            // Save
            Program.SaveUser();
            // Delete save
            if (user.money < 1) GameOver();
        }
    }
}
