using System;
using System.Collections.Generic;
using System.Text;

namespace TerminalCasino
{
    public class Deck
    {
        /// <summary>
        /// Deck of cards containing available cards
        /// </summary>
        public List<Card> deck = new List<Card>() { };


        public Deck()
        {
            ResetDeck(); // Defaults the deck with a new instance of it.
        }

        /// <summary>
        /// Resets the deck to have all default cards in it.
        /// This doesn't remove any cards that have already been removed from the deck.
        /// </summary>
        private void ResetDeck()
        {
            // Create a new deck with the standard 52 cards
            deck = new List<Card>()
            {
                new Card(Card.Value.Ace, Card.Suit.Spades),
                new Card(Card.Value.Two, Card.Suit.Spades),
                new Card(Card.Value.Three, Card.Suit.Spades),
                new Card(Card.Value.Four, Card.Suit.Spades),
                new Card(Card.Value.Five, Card.Suit.Spades),
                new Card(Card.Value.Six, Card.Suit.Spades),
                new Card(Card.Value.Seven, Card.Suit.Spades),
                new Card(Card.Value.Eight, Card.Suit.Spades),
                new Card(Card.Value.Nine, Card.Suit.Spades),
                new Card(Card.Value.Ten, Card.Suit.Spades),
                new Card(Card.Value.Jack, Card.Suit.Spades),
                new Card(Card.Value.Queen, Card.Suit.Spades),
                new Card(Card.Value.King, Card.Suit.Spades),
                new Card(Card.Value.Ace, Card.Suit.Diamonds),
                new Card(Card.Value.Two, Card.Suit.Diamonds),
                new Card(Card.Value.Three, Card.Suit.Diamonds),
                new Card(Card.Value.Four, Card.Suit.Diamonds),
                new Card(Card.Value.Five, Card.Suit.Diamonds),
                new Card(Card.Value.Six, Card.Suit.Diamonds),
                new Card(Card.Value.Seven, Card.Suit.Diamonds),
                new Card(Card.Value.Eight, Card.Suit.Diamonds),
                new Card(Card.Value.Nine, Card.Suit.Diamonds),
                new Card(Card.Value.Ten, Card.Suit.Diamonds),
                new Card(Card.Value.Jack, Card.Suit.Diamonds),
                new Card(Card.Value.Queen, Card.Suit.Diamonds),
                new Card(Card.Value.King, Card.Suit.Diamonds),
                new Card(Card.Value.Ace, Card.Suit.Clubs),
                new Card(Card.Value.Two, Card.Suit.Clubs),
                new Card(Card.Value.Three, Card.Suit.Clubs),
                new Card(Card.Value.Four, Card.Suit.Clubs),
                new Card(Card.Value.Five, Card.Suit.Clubs),
                new Card(Card.Value.Six, Card.Suit.Clubs),
                new Card(Card.Value.Seven, Card.Suit.Clubs),
                new Card(Card.Value.Eight, Card.Suit.Clubs),
                new Card(Card.Value.Nine, Card.Suit.Clubs),
                new Card(Card.Value.Ten, Card.Suit.Clubs),
                new Card(Card.Value.Jack, Card.Suit.Clubs),
                new Card(Card.Value.Queen, Card.Suit.Clubs),
                new Card(Card.Value.King, Card.Suit.Clubs),
                new Card(Card.Value.Ace, Card.Suit.Hearts),
                new Card(Card.Value.Two, Card.Suit.Hearts),
                new Card(Card.Value.Three, Card.Suit.Hearts),
                new Card(Card.Value.Four, Card.Suit.Hearts),
                new Card(Card.Value.Five, Card.Suit.Hearts),
                new Card(Card.Value.Six, Card.Suit.Hearts),
                new Card(Card.Value.Seven, Card.Suit.Hearts),
                new Card(Card.Value.Eight, Card.Suit.Hearts),
                new Card(Card.Value.Nine, Card.Suit.Hearts),
                new Card(Card.Value.Ten, Card.Suit.Hearts),
                new Card(Card.Value.Jack, Card.Suit.Hearts),
                new Card(Card.Value.Queen, Card.Suit.Hearts),
                new Card(Card.Value.King, Card.Suit.Hearts),
            };
        }

        /// <summary>
        /// This method aquires a random card from the deck and returns it.
        /// </summary>
        /// <returns>A random card from the deck</returns>
        public Card GetCardRandom()
        {
            // Safeguard for when the deck is empty
            if (deck.Count == 0) return null;

            // Intance of random lib
            Random rand = new Random();
            // Gets a random index in the deck
            int randomIndex = rand.Next(0, deck.Count);
            // Gets the card at that index and saves it
            Card card = deck[randomIndex];
            // Remove it from the deck
            deck.Remove(card);
            // Return the card
            return card;
        }
    }
}
