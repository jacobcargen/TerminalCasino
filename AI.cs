using System;
using System.Collections.Generic;
using System.Text;
using static TerminalCasino.Poker;

namespace TerminalCasino
{
    internal class AI
    {
        private Player bot;


        public AI(Player bot)
        {
            this.bot = bot;
        }
        public Choice GetDecision(List<Choice> availableChoices)
        {
            Choice choice = Choice.None;

            // Random choice for now
            Random random = new Random();

            int index = random.Next(availableChoices.Count);
            choice = availableChoices[index];
            return choice;
        }
    }
}
