using System;
using System.Collections.Generic;
using System.Text;
using static TerminalCasino.Poker;

namespace TerminalCasino
{
    public class Player
    {
        public int money = 500; // Amount of money this player has
        public string name = string.Empty; // Players name
        public bool isUser = false; // If this player is a user rather than an AI

        #region Poker
        public List<Card> hand = new List<Card>(); // This players hand of cards
        public int currentRaise = 0; // This players current raise in a game of poker
        public bool hasCheckedCalledRaised = false;
        public bool hasFolded = false;
        public bool isTappedIn = false;
        public Choice lastChoice = Choice.None;
        #endregion


        public Player() { }

        /// <summary>
        /// Call this with some saved data to load saved player stats
        /// </summary>
        /// <param name="existingPlayer"></param>
        public Player(SaveData saveData) 
        {
            this.money = saveData.money;
            this.isUser = saveData.isUser;
            this.name = saveData.name;
        }
    }
}
