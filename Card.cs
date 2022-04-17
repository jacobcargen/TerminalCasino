using System;
using System.Collections.Generic;
using System.Text;

namespace TerminalCasino
{
    public class Card
    {
        public enum Value
        {
            Unknown,
            Ace,
            Two,
            Three,
            Four,
            Five,
            Six,
            Seven,
            Eight,
            Nine,
            Ten,
            Jack,
            Queen,
            King,
        }
        public enum Suit
        {
            Unknown,
            Spades,
            Diamonds,
            Clubs,
            Hearts,
        }

        public Value value = Value.Unknown;
        public Suit suit = Suit.Unknown;
        public string line1, line2, line3, line4;
        public bool isFaceUp = true;


        public Card(Value value, Suit suit)
        {
            this.value = value;
            this.suit = suit;
            Refresh();
        }
        /// <summary>
        /// Call this to update and refresh the card image
        /// </summary>
        public void Refresh()
        {
            if (isFaceUp)
            {
                string blankSpace;
                if (value == Value.Ten) blankSpace = "";
                else blankSpace = " ";
                this.line1 = $" ___  ";
                this.line2 = $"|{Convert(value)}{blankSpace} | ";
                this.line3 = $"| {Convert(suit)} | ";
                this.line4 = $"|___| ";
            }
            else
            {
                this.line1 = $" ___  ";
                this.line2 = $"|###| ";
                this.line3 = $"|###| ";
                this.line4 = $"|###| ";
            }
        }
        private string Convert(Value value)
        {
            switch (value)
            {
                case Value.Ace: return "A";
                case Value.Two: return "2";
                case Value.Three: return "3";
                case Value.Four: return "4";
                case Value.Five: return "5";
                case Value.Six: return "6";
                case Value.Seven: return "7";
                case Value.Eight: return "8";
                case Value.Nine: return "9";
                case Value.Ten: return "10";
                case Value.Jack: return "J";
                case Value.Queen: return "Q";
                case Value.King: return "K";
                default: return "?";
            }
        }
        private string Convert(Suit suit)
        {
            switch (suit)
            {
                case Suit.Spades: return "S";
                case Suit.Diamonds: return "D";
                case Suit.Clubs: return "C";
                case Suit.Hearts: return "H";
                default: return "?";
            }
        }
    }
}
