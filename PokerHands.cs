using System;
using System.Collections.Generic;
using System.Text;

namespace TerminalCasino
{
    public class PokerHands
    {
        public enum HandValue
        {
            RoyalFlush,
            StraightFlush,
            FourOfAKind,
            FullHouse,
            Flush,
            Straight,
            ThreeOfAKind,
            TwoPair,
            Pair,
            HighCard,
        }


        public static List<Player> GetWinningPlayers(List<Player> playersToCompare)
        {
            // Make new lists tracking how many players have what
            List<Player> playersWithRoyalFlush = new List<Player>();
            List<Player> playersWithStraightFlush = new List<Player>();
            List<Player> playersWithFourOfAKind = new List<Player>();
            List<Player> playersWithFullHouse = new List<Player>();
            List<Player> playersWithFlush = new List<Player>();
            List<Player> playersWithStraight = new List<Player>();
            List<Player> playersWithThreeOfAKind = new List<Player>();
            List<Player> playersWithTwoPair = new List<Player>();
            List<Player> playersWithPair = new List<Player>();
            List<Player> playersWithHighCard = new List<Player>();
            // Add to the lists
            foreach (Player player in playersToCompare)
            {
                switch (GetHandValue(player.hand))
                {
                    case HandValue.RoyalFlush:
                        playersWithRoyalFlush.Add(player);
                        break;
                    case HandValue.StraightFlush:
                        playersWithStraightFlush.Add(player);
                        break;
                    case HandValue.FourOfAKind:
                        playersWithFourOfAKind.Add(player);
                        break;
                    case HandValue.FullHouse:
                        playersWithFullHouse.Add(player);
                        break;
                    case HandValue.Flush:
                        playersWithFlush.Add(player);
                        break;
                    case HandValue.Straight:
                        playersWithStraight.Add(player);
                        break;
                    case HandValue.ThreeOfAKind:
                        playersWithThreeOfAKind.Add(player);
                        break;
                    case HandValue.TwoPair:
                        playersWithTwoPair.Add(player);
                        break;
                    case HandValue.Pair:
                        playersWithPair.Add(player);
                        break;
                    case HandValue.HighCard:
                        playersWithHighCard.Add(player);
                        break;
                }
            }

            // Check if any royal flush winners
            if (playersWithRoyalFlush.Count >= 1)
            {
                // If only one player return that player as the winner..if there is only 1 player
                if (playersWithRoyalFlush.Count == 1) return new List<Player>() { playersWithRoyalFlush[0] };
                // There is more than one player, ITS A TIE
                else return playersWithRoyalFlush;
            }
            // Check if any straight flush winners
            else if (playersWithStraightFlush.Count >= 1)
            {
                // If only one player return that player as the winner..if there is only 1 player
                if (playersWithStraightFlush.Count == 1) return new List<Player>() { playersWithStraightFlush[0] };
                // There is more than one player, ITS A TIE
                else return playersWithStraightFlush;
            }
            // Check if any four of a kind winners
            else if (playersWithFourOfAKind.Count >= 1)
            {
                // If only one player return that player as the winner..if there is only 1 player
                if (playersWithFourOfAKind.Count == 1) return new List<Player>() { playersWithFourOfAKind[0] };
                // There is more than one player, Must determine who has the higher cards
                else
                {
                    return TieChecker(playersWithFourOfAKind, HandValue.FourOfAKind);
                }
            }
            // Check if any full house winners
            else if (playersWithFullHouse.Count >= 1)
            {
                // If only one player return that player as the winner..if there is only 1 player
                if (playersWithFullHouse.Count == 1) return new List<Player>() { playersWithFullHouse[0] };
                // There is more than one player, Must determine who has the higher cards
                else
                {
                    return TieChecker(playersWithFullHouse, HandValue.FullHouse);
                }
            }
            // Check if any flush winners
            else if (playersWithFlush.Count >= 1)
            {
                // If only one player return that player as the winner..if there is only 1 player
                if (playersWithFlush.Count == 1) return new List<Player>() { playersWithFlush[0] };
                // There is more than one player, Must determine who has the higher cards
                else
                {
                    return TieChecker(playersWithFlush, HandValue.Flush);
                }
            }
            // Check if any straight winners
            else if (playersWithStraight.Count >= 1)
            {
                // If only one player return that player as the winner..if there is only 1 player
                if (playersWithStraight.Count == 1) return new List<Player>() { playersWithStraight[0] };
                // There is more than one player, Must determine who has the higher cards
                else
                {
                    return TieChecker(playersWithStraight, HandValue.Straight);
                }
            }
            // Check if any three of a kind winners
            else if (playersWithThreeOfAKind.Count >= 1)
            {
                // If only one player return that player as the winner..if there is only 1 player
                if (playersWithThreeOfAKind.Count == 1) return new List<Player>() { playersWithThreeOfAKind[0] };
                // There is more than one player, Must determine who has the higher cards
                else
                {
                    return TieChecker(playersWithThreeOfAKind, HandValue.ThreeOfAKind);
                }
            }
            // Check if any two pair winners
            else if (playersWithTwoPair.Count >= 1)
            {
                // If only one player return that player as the winner..if there is only 1 player
                if (playersWithTwoPair.Count == 1) return new List<Player>() { playersWithTwoPair[0] };
                // There is more than one player, Must determine who has the higher cards
                else
                {
                    return TieChecker(playersWithTwoPair, HandValue.TwoPair);
                }
            }
            // Check if any pair winners
            else if (playersWithTwoPair.Count >= 1)
            {
                // If only one player return that player as the winner..if there is only 1 player
                if (playersWithTwoPair.Count == 1) return new List<Player>() { playersWithTwoPair[0] };
                // There is more than one player, Must determine who has the higher cards
                else
                {
                    return TieChecker(playersWithTwoPair, HandValue.Pair);
                }
            }
            // Check if any straight winners
            else if (playersWithHighCard.Count >= 1)
            {
                // If only one player return that player as the winner..if there is only 1 player
                if (playersWithHighCard.Count == 1) return new List<Player>() { playersWithHighCard[0] };
                // There is more than one player, Must determine who has the higher cards
                else
                {
                    return TieChecker(playersWithHighCard, HandValue.HighCard);
                }
            }
            else throw new Exception("No winning players?");
        }

        private static List<Player> TieChecker(List<Player> playersToCheck, HandValue handValue)
        {
            // For now just return the first in the index until i can figure out how to do this
            return new List<Player>() { playersToCheck[0] };
            /*
            switch (handValue)
            {
                case HandValue.RoyalFlush:

                    break;
            }
            */
        }

        #region Getting the Hand value

        private static HandValue GetHandValue(List<Card> hand)
        {
            // High card by default
            HandValue handValue = HandValue.HighCard;
            // Check for best option, overwriting if any below true
            if (IsPair(hand))           handValue = HandValue.Pair;
            if (IsTwoPair(hand))        handValue = HandValue.TwoPair;
            if (IsThreeOfAKind(hand))   handValue = HandValue.ThreeOfAKind;
            if (IsStraight(hand))       handValue = HandValue.Straight;
            if (IsFlush(hand))          handValue = HandValue.Flush;
            if (IsFullHouse(hand))      handValue = HandValue.FullHouse;
            if (IsFourOfAKind(hand))    handValue = HandValue.FourOfAKind;
            if (IsStraightFlush(hand))  handValue = HandValue.StraightFlush;
            if (IsRoyalFlush(hand))     handValue = HandValue.RoyalFlush;
            // Return result
            return handValue;

        }

        private static bool IsRoyalFlush(List<Card> hand)
        {
            bool valid = true;

            HandValuesAndSuits handValuesAndSuits = new HandValuesAndSuits(hand);
            if (handValuesAndSuits.aces > 0 && handValuesAndSuits.kings > 0)
                if (handValuesAndSuits.queens > 0 && handValuesAndSuits.jacks > 0)
                    if (handValuesAndSuits.tens > 0) ;


            return valid;
        }
        private static bool IsStraightFlush(List<Card> hand)
        {
            bool valid = true;


            return valid;
        }
        private static bool IsFourOfAKind(List<Card> hand)
        {
            bool valid = true;


            return valid;
        }
        private static bool IsFullHouse(List<Card> hand)
        {
            bool valid = true;


            return valid;
        }
        private static bool IsFlush(List<Card> hand)
        {
            bool valid = true;


            return valid;
        }
        private static bool IsStraight(List<Card> hand)
        {
            bool valid = true;


            return valid;
        }
        private static bool IsThreeOfAKind(List<Card> hand)
        {
            bool valid = true;


            return valid;
        }
        private static bool IsTwoPair(List<Card> hand)
        {
            bool valid = true;


            return valid;
        }
        private static bool IsPair(List<Card> hand)
        {
            HandValuesAndSuits handValuesAndSuits = new HandValuesAndSuits(hand);
            // If 2+ of any vard value
            if (handValuesAndSuits.aces >= 2) return true;
            else if (handValuesAndSuits.kings >= 2) return true;
            else if (handValuesAndSuits.queens >= 2) return true;
            else if (handValuesAndSuits.jacks >= 2) return true;
            else if (handValuesAndSuits.tens >= 2) return true;
            else if (handValuesAndSuits.nines >= 2) return true;
            else if (handValuesAndSuits.eights >= 2) return true;
            else if (handValuesAndSuits.sevens >= 2) return true;
            else if (handValuesAndSuits.sixes >= 2) return true;
            else if (handValuesAndSuits.fives >= 2) return true;
            else if (handValuesAndSuits.fours >= 2) return true;
            else if (handValuesAndSuits.threes >= 2) return true;
            else if (handValuesAndSuits.twos >= 2) return true;
            // If none ..  then there are no pairs
            else return false;
        }

        #endregion

    }
    public class HandValuesAndSuits // Honestly I dont think i need this class
    {
        // Values
        public int aces;
        public int twos;
        public int threes;
        public int fours;
        public int fives;
        public int sixes;
        public int sevens;
        public int eights;
        public int nines;
        public int tens;
        public int jacks;
        public int queens;
        public int kings;
        // Suits
        public int spades;
        public int hearts;
        public int diamon;
        public int clubs;
        
        public HandValuesAndSuits(List<Card> hand)
        {
            List<Card.Value> values = new List<Card.Value>();
            List<Card.Suit> suits = new List<Card.Suit>();
            foreach (Card card in hand)
            {
                values.Add(card.value);
                suits.Add(card.suit);
            }
            // Values
            int aces     = values.FindAll(v => v == Card.Value.Ace).Count;
            int twos     = values.FindAll(v => v == Card.Value.Two).Count;
            int threes   = values.FindAll(v => v == Card.Value.Three).Count;
            int fours    = values.FindAll(v => v == Card.Value.Four).Count;
            int fives    = values.FindAll(v => v == Card.Value.Five).Count;
            int sixes    = values.FindAll(v => v == Card.Value.Six).Count;
            int sevens   = values.FindAll(v => v == Card.Value.Seven).Count;
            int eights   = values.FindAll(v => v == Card.Value.Eight).Count;
            int nines    = values.FindAll(v => v == Card.Value.Nine).Count;
            int tens     = values.FindAll(v => v == Card.Value.Ten).Count;
            int jacks    = values.FindAll(v => v == Card.Value.Jack).Count;
            int queens   = values.FindAll(v => v == Card.Value.Queen).Count;
            int kings    = values.FindAll(v => v == Card.Value.King).Count;
            // Suits
            int spades   = suits.FindAll(s => s == Card.Suit.Spades).Count;
            int hearts   = suits.FindAll(s => s == Card.Suit.Hearts).Count;
            int diamonds = suits.FindAll(s => s == Card.Suit.Diamonds).Count;
            int clubs    = suits.FindAll(s => s == Card.Suit.Clubs).Count;
        }
    }
}
