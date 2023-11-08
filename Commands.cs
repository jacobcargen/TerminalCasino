using System;
using System.Collections.Generic;
using System.Text;

namespace TerminalCasino
{
    public static class Commands
    {
        public static List<Command> commands = new List<Command>()
        {
            new Command("help", Help, "A simple help list."),
            new Command("quit", Quit, "Exit the program."),
            new Command("cmds", Cmds, "Show a list of available commands."),
            new Command("clear", Clear, "Clears the terminal."),
            new Command("stats", Stats, "Shows current progress and stats."),
            new Command("RESET", ResetProgress, "Will reset all progress to the defaults."),
            new Command("name", ChangeName, "Call this to change and set a username."),
            new Command("textcolor", ChangeTextColor, "Call this to change the color of the text."),
            new Command("backcolor", ChangeBackroundColor, "Call this to change the color of the backrouond."),
            // Games
            //new Command("2card", TwoCard, "Start playing two card stud poker."),
            new Command("7card", SevenCard, "Start playing seven card stud poker."),
            new Command("21", BlackJack, "Start playing blackjack."),
            new Command("roulette", Roulette, "Start playing roulette.")
        };


        /// <summary>
        /// Checks a users input and compares it to a list of known commands
        /// </summary>
        /// <returns>True if a command is found, otherwise it will return false</returns>
        public static bool CheckCommand(string cmd)
        {
            bool isFound = false; // Cmd not found at the start
            foreach (Command command in commands) // Check to see if the input matches a known cmd
            {
                if (command.command == cmd) // If it matches with a cmd
                {
                    // Play accept sound
                    Audio.PlayCmdAcceptedSound();
                    isFound = true; // Has found a cmd
                    command.action(); // Call the method synced with the cmd
                }
            }
            return isFound; // Return if the cmd was found or not
        }

        /// <summary>
        /// This is called when a command is not found.
        /// </summary>
        public static void CmdNotFound()
        {
            Tools.TellUser("Invalid command or not found. Type \"help\" for more info.");
            Audio.ErrorSound();
        }

        #region Commands
        
        /// <summary>
        /// Change the color of the terminal text
        /// </summary>
        public static void ChangeTextColor()
        {
            Clear();
            ConsoleKey key;

            // Get a choice from the user
            key = Tools.GetInputKey(@"Choose a text color
 - Press a key - 
1 : White
2 : Gray
3 : Yellow
4 : Blue
5 : Green
6 : Red
7 : Cyan
8 : Magenta");

            switch (key)
            {
                case ConsoleKey.D1:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case ConsoleKey.D2:
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
                case ConsoleKey.D3:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case ConsoleKey.D4:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    break;
                case ConsoleKey.D5:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case ConsoleKey.D6:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case ConsoleKey.D7:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    break;
                case ConsoleKey.D8: 
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    break;
                default:
                    ChangeTextColor(); // Call until valid
                    break;
            }
            Clear();
        }

        /// <summary>
        /// Change the color of the terminal text
        /// </summary>
        public static void ChangeBackroundColor()
        {
            Clear();
            ConsoleKey key;

            // Get a choice from the user
            key = Tools.GetInputKey(@"Choose a backround color
 - Press a key - 
1 : Black
2 : Gray
3 : Yellow
4 : Blue
5 : Green
6 : Red
7 : Cyan
8 : Magenta");

            switch (key)
            {
                case ConsoleKey.D1:
                    Console.BackgroundColor = ConsoleColor.Black;
                    break;
                case ConsoleKey.D2:
                    Console.BackgroundColor = ConsoleColor.DarkGray;
                    break;
                case ConsoleKey.D3:
                    Console.BackgroundColor = ConsoleColor.DarkYellow;
                    break;
                case ConsoleKey.D4:
                    Console.BackgroundColor = ConsoleColor.DarkBlue;
                    break;
                case ConsoleKey.D5:
                    Console.BackgroundColor = ConsoleColor.DarkGreen;
                    break;
                case ConsoleKey.D6:
                    Console.BackgroundColor = ConsoleColor.DarkRed;
                    break;
                case ConsoleKey.D7:
                    Console.BackgroundColor = ConsoleColor.DarkCyan;
                    break;
                case ConsoleKey.D8:
                    Console.BackgroundColor = ConsoleColor.DarkMagenta;
                    break;
                default:
                    ChangeTextColor(); // Call until valid
                    break;
            }
            Clear();
        }

        /// <summary>
        /// Displays a simple help message for the user.
        /// </summary>
        public static void Help()
        {
            Tools.TellUser(@"[-[-[ HELP ]-]-]
Type ""help"" for this current list
Type ""cmds"" for a list of commands
Type ""quit"" to exit the program");
        }

        /// <summary>
        /// Displays all available commands
        /// </summary>
        private static void Cmds()
        {
            string cmds = "";
            foreach (Command command in commands)
            {
                cmds += $"\nType \"{command.command}\" : {command.info}";
            }
            Tools.TellUser($"[-[-[ COMMANDS ]-]-]{cmds}");
        }

        /// <summary>
        /// Asks the user to give their name
        /// </summary>
        public static void ChangeName()
        {
            Program.user.name = Tools.GetInputString("Enter a username: ");
        }

        /// <summary>
        /// Call this to quit the program.
        /// </summary>
        public static void Quit()
        {
            Tools.TellUser("Exiting...");
            // Wait time... so the user can read that it is exiting
            System.Threading.Thread.Sleep(500);
            Program.isQuit = true;
        }

        /// <summary>
        /// Clears the terminal
        /// </summary>
        public static void Clear()
        {
            Console.Clear();
        }

        // Games
        /// <summary>
        /// Call this to play two card stud poker
        /// </summary>
        private static void TwoCard()
        {
            Clear();
            Program.StopMenuMusic();

        }
        /// <summary>
        /// Call this to play seven card stud poker
        /// </summary>
        public static void SevenCard()
        {
            Clear();
            Program.StopMenuMusic();
            new SevenCardStud().Play(Program.user);
        }
        /// <summary>
        /// Call this to start roulette
        /// </summary>
        public static void Roulette()
        {
            Clear();
            Program.StopMenuMusic();
            new Roulette().Play(Program.user);
        }
        /// <summary>
        /// Call this to play blackjack
        /// </summary>
        public static void BlackJack()
        {
            Clear();
            Program.StopMenuMusic();
            new BlackJack().Play(Program.user);
        }

        /// <summary>
        /// Defaults the users progress
        /// </summary>
        public static void ResetProgress()
        {
            // new save
            Saving.SaveProgress(new SaveData(new Player()), true);
            Program.LoadUser();
            ChangeName();
        }
        /// <summary>
        /// Displays the users stats
        /// </summary>
        public static void Stats()
        {
            Tools.TellUser($"[-[-[ STATS ]-]-]" +
                $"\nName : {Program.user.name}" +
                $"\nMoney : ${Program.user.money}");
        }

        #endregion

    }
    public class Command
    {
        public string command; // string the user must type to use this command.
        public Action action; // Method this command is linked to.
        public string info; // Summary of this command for the user.
        public Command(string command, Action action, string info)
        {
            this.command = command;
            this.action = action;
            this.info = info;
        }
    }
}
