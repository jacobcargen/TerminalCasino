using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace TerminalCasino
{
    internal class Roulette
    {
        Player user;
        public void Play(Player user)
        {
            this.user = user;

            Console.WriteLine("Testing Roulette");
            // Place bet
            //PlaceBets();
            // Check who won the game
            //CheckWinnings();
            
            // Wait for 1.5 seconds
            System.Threading.Thread.Sleep(1500);
            Tools.WantToPlayAgain(Commands.BlackJack);
        }
    }
}
