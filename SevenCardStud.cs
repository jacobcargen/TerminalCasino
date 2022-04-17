using System;
using System.Collections.Generic;
using System.Text;

namespace TerminalCasino
{
    public class SevenCardStud : Poker
    {
        public void Play(Player player)
        {
            // User, Ante = 0, Max Raise = 500, Bots = 2
            NewGame(player, 0, 500, 2);

            Console.WriteLine("\nShuffling..."); 
            Audio.SuffleSound();

            // Wait for 2.5 seconds before dealing cards
            do { System.Threading.Thread.Sleep(2500); } while (false);

            // Start
            RoundOne();
        }
        private void RoundOne()
        {
            NewRound();
            NextDealer();
            DealOneToAll(true);
            DealOneToAll(true);
            DealOneToAll();

            // Start turns
            while (!IsRoundOver())
            {
                NextPlayer(); // Next player
                ShowScene(); // Show scene
                GetPlayerAction(); // Get a player choice
                ShowScene();
            }
            currentPlayerTurn = null;
            ShowScene();
            Rounds(3);
        }
        private void Rounds(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                NewRound();
                DealOneToAll();

                // Start turns
                while (!IsRoundOver())
                {
                    NextPlayer(); // Next player
                    ShowScene(); // Show scene
                    GetPlayerAction(); // Get a player choice
                    ShowScene();
                }
                currentPlayerTurn = null;
                ShowScene();
            }
            LastRound();
        }
        private void LastRound()
        {
            NewRound();
            DealOneToAll(true);

            // Start turns
            while (!IsRoundOver())
            {
                NextPlayer(); // Next player
                ShowScene(); // Show scene
                GetPlayerAction(); // Get a player choice
                ShowScene();
            }
            currentPlayerTurn = null;
            ShowScene();
            CheckWhoWon();
        }
    }
}
