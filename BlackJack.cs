using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TerminalCasino.Card;

namespace TerminalCasino
{
    public class BlackJack
    {
        // The dealer
        Player dealer = new Player();
        // The user
        Player user;
        // The deck of cards to be used
        private Deck deck = new Deck();
        // Bet limit
        private int betLimit = 50000;
        // Users bet
        private int userBet = 0;
        // Possible choices during a players turn.
        public enum Choice
        {
            None,
            Hit,
            Stand,
            DoubleDown,
            Split,
            Insurance
        }
        // Keybinds for the choices
        private ConsoleKey hitKey = ConsoleKey.Spacebar;
        private ConsoleKey standKey = ConsoleKey.Enter;


        public void Play(Player user)
        {
            this.user = user;
            dealer = new Player();
            dealer.name = "Dealer";
            ResetGameAndPlayer();
            dealer.money = 9999999;

            Console.WriteLine("\nShuffling...");
            Audio.SuffleSound();

            // Wait for 2.5 seconds before dealing cards
            do { System.Threading.Thread.Sleep(2500); } while (false);

            // Place bet
            PlaceBet();

            // Starting cards
            DealToPlayer(user);
            DealToPlayer(dealer, true);
            DealToPlayer(user);
            DealToPlayer(dealer);

            // User goes first
            PlayerTurn();
            // Dealer goes next
            DealerTurn();
            // Check who won the game
            CheckWhoWon();
            // Wait for 1.5 seconds
            System.Threading.Thread.Sleep(1500);
            Tools.WantToPlayAgain(Commands.BlackJack);
        }
        private void PlayerTurn()
        {
            while (true)
            {
                if (GetHandNumberValue(user.hand) == 21) Stand(user); 
                Choice choice = GetUserChoice();

                if (choice == Choice.Stand) break;
                if (IsBust(user)) break;
            }
        }
        private void DealerTurn()
        {
            // Show the dealers cards
            foreach (Card card in dealer.hand)
            {
                card.isFaceUp = true;
            }

            // If the user already busted then skip, dealer won
            if (IsBust(user)) { ShowScene(); return; } 
            while (true)
            {
                Choice choice = GetDealerChoice();

                if (choice == Choice.Stand) break;
                if (IsBust(dealer)) break;
            }
        }

        /// <summary>
        /// Shows the game view to the player.
        /// This will clear the terminal and show the scene.
        /// </summary>
        /// <exception cref="Exception"></exception>
        private void ShowScene()
        {
            // Clear the terminal
            Commands.Clear();
            Tools.TellUser($"Bet: ${userBet}");

            // Show dealer
            Tools.TellUser($"\nDealer");
            PrintHand(dealer);
            // Show user
            Tools.TellUser($"\n{user.name} : ${user.money}");
            PrintHand(user);
            // Some blank space at the end
            Console.WriteLine();
        }

        /// <summary>
        /// Resets the game and the player.
        /// </summary>
        private void ResetGameAndPlayer()
        {
            // Reset game
            deck = new Deck();
            user.hand = new List<Card>();
            dealer.hand = new List<Card>();
            userBet = 0;
        }

        #region Choices

        /// <summary>
        /// Checks if the dealer should hit or stand.
        /// </summary>
        /// <param name="hand"></param>
        /// <returns>Returns true for hit.</returns>
        private Choice GetDealerChoice()
        {
            ShowScene();
            // If is over 16 then stand
            if (GetHandNumberValue(dealer.hand) > 16)
            {
                Stand(dealer);
                return Choice.Stand;
            }
            // If is under or equal to 16 then hit
            else
            {
                Hit(dealer);
                return Choice.Hit;
            }
        }

        /// <summary>
        /// Gets a choice from the user.
        /// </summary>
        /// <returns>Returns the users choice.</returns>
        private Choice GetUserChoice()
        {
            Program.SaveUser();

            // text for the user
            string choicesText = " - Press a key - ";
            string hit = "\nSpace : Hit";
            string stand = "\nEnter : Stand";
            List<Choice> choices = GetAvailableChoices(user);

            // Gets a list of available user choices and add those to a viewable list for the user
            foreach (Choice choice in choices)
            {
                // Add the available choices to the printed text
                switch (choice)
                {
                    case Choice.Hit:
                        choicesText += String.Concat(hit);
                        break;
                    case Choice.Stand:
                        choicesText += String.Concat(stand);
                        break;
                }
            }

            // Ask the player for a choice until it is valid
            do
            {
                // Ask the player to choose
                ConsoleKey key = Tools.GetInputKey(choicesText);
                // Blank space
                Console.WriteLine();
                // Returns the result of the user input
                Choice choice = CheckChoiceKey(key);
                // Loops until a valid response is given
                if (choice != Choice.None) return choice;
                else ShowScene();
            }
            while (true);
        }

        /// <summary>
        /// Hit
        /// </summary>
        /// <param name="player"></param>
        private void Hit(Player player)
        {
            DealToPlayer(player);
        }

        /// <summary>
        /// Stand
        /// </summary>
        /// <param name="player"></param>
        private void Stand(Player player)
        {

        }

        /// <summary>
        /// Gets the input from the user for a bet amount.
        /// </summary>
        /// <param name="player"></param>
        private void PlaceBet()
        {
            Commands.Clear();
            string input = Tools.GetInputString($"Money : {user.money} \nBet $"); // Get an amount of money from the user
            if (int.TryParse(input, out int amt)) // Try to parse the value to a int
            {
                if (IsValidBetAmt(amt)) // If the value is within a valid range
                {
                    // Add the bet
                    user.money -= amt;
                    userBet += amt;
                    return;
                }
                else Tools.TellUser("Invalid amount."); // Value not in range
            }
            else Tools.TellUser("Invalid input."); // Input invalid
            PlaceBet();
        }

        #endregion

        #region Check & Verify

        /// <summary>
        /// Checks if the player has busted (gone over 21)
        /// </summary>
        /// <param name="player"></param>
        /// <returns>Returns true if the player busted</returns>
        private bool IsBust(Player player)
        {
            int handValue = GetHandNumberValue(player.hand);
            if (handValue > 21) return true;
            else return false;
        }

        /// <summary>
        /// Returns the players available choices.
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        private List<Choice> GetAvailableChoices(Player player)
        {
            List<Choice> choices = new List<Choice>();

            choices.Add(Choice.Hit);
            choices.Add(Choice.Stand);

            return choices;
        }

        /// <summary>
        /// Checks the users choice and returns it, if it is valid.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="availableChoices"></param>
        /// <returns>Users choice</returns>
        private Choice CheckChoiceKey(ConsoleKey key)
        {
            foreach (Choice choice in GetAvailableChoices(user))
            {
                // Returns the choice if it is available and the right key was pressed
                if (choice == Choice.Hit && key == hitKey)
                {
                    Hit(user);
                    return choice;
                }
                if (choice == Choice.Stand && key == standKey)
                {
                    Stand(user);
                    return choice;
                }
            }
            ShowScene();
            // Returns invalid if a choice was not found
            return Choice.None;
        }

        private void CheckWhoWon()
        {
            Player winningPlayer = null;
            int winningAmt = 0;
            int userPoints = GetHandNumberValue(user.hand);
            int dealerPoints = GetHandNumberValue(dealer.hand);
            if (IsBust(user))
            {
                // Dealer won
                winningPlayer = dealer;
                winningAmt = userBet;
                Tools.TellUser("BUST!");
            }
            else if (IsBust(dealer))
            {
                // Player won
                winningPlayer = user;
                // Take from dealer
                dealer.money -= userBet;
                // Add to the money
                winningAmt = userBet * 2;
                Tools.TellUser("DEALER BUST!");
            }
            // No bust, but user has more points
            else if (userPoints > dealerPoints)
            {
                // Player won
                winningPlayer = user;
                // Take from dealer
                dealer.money -= userBet;
                // Add to the money
                winningAmt = userBet * 2;
            }
            // No bust, but dealer has more points
            else if (dealerPoints > userPoints)
            {
                // Dealer won
                winningPlayer = dealer;
                winningAmt = userBet;
            }
            // No bust, but it is a tie
            else if (userPoints == dealerPoints)
            {
                // If 21 then dealer wins the tie
                if (dealerPoints == 21)
                {
                    // Dealer won
                    winningPlayer = dealer;
                    winningAmt = userBet;
                }
                // Push
                else
                {
                    // Push
                    Tools.TellUser("PUSH");
                    // Give player their money back
                    user.money += userBet;
                    // Return from this method
                    return;
                }
            }
            else throw new Exception("How could this even happen?!");

            Tools.TellUser($"{winningPlayer.name} has won ${winningAmt}");
            winningPlayer.money += winningAmt;
        }

        /// <summary>
        /// Checks if a bet is valid.
        /// </summary>
        /// <param name="bet"></param>
        /// <returns>Returns true if the bet amount was valid.</returns>
        private bool IsValidBetAmt(int bet)
        {
            // Starts as valid
            bool isValid = true;
            // Raise must be at least 1
            if (bet < 1) isValid = false;
            // Raise must less than the games limit
            if (bet > betLimit) isValid = false;
            // The player must have enough money for the raise
            if (bet > user.money) isValid = false;
            // Return true if the amount was valid
            return isValid;
        }

        #endregion

        #region Hands & Cards

        /// <summary>
        /// Prints a players hand to the terminal
        /// </summary>
        /// <param name="player"></param>
        private void PrintHand(Player player)
        {
            // Refresh the cards
            foreach (Card card in player.hand) card.Refresh();
            // Display the cards
            foreach (Card card in player.hand)
            {
                Console.Write(card.line1);
            }
            Console.WriteLine();
            foreach (Card card in player.hand)
            {
                Console.Write(card.line2);
            }
            Console.WriteLine();
            foreach (Card card in player.hand)
            {
                Console.Write(card.line3);
            }
            Console.WriteLine();
            foreach (Card card in player.hand)
            {
                Console.Write(card.line4);
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Returns the number value of a specific card.
        /// </summary>
        /// <param name="card"></param>
        /// <param name="aceEqualOne"></param>
        /// <returns>Card value of the inputed card.</returns>
        private int GetCardNumberValue(Card card)
        {
            switch (card.value)
            {
                case Value.Ace:
                    return 11;
                case Value.Two:
                    return 2;
                case Value.Three:
                    return 3;
                case Value.Four:
                    return 4;
                case Value.Five:
                    return 5;
                case Value.Six:
                    return 6;
                case Value.Seven:
                    return 7;
                case Value.Eight:
                    return 8;
                case Value.Nine:
                    return 9;
                default: // Face cards and ten
                    return 10;
            }
        }

        /// <summary>
        /// Get the value of a specific hand of cards.
        /// </summary>
        /// <param name="hand"></param>
        /// <returns>Returns the value of the hand.</returns>
        private int GetHandNumberValue(List<Card> hand)
        {
            // The players hand value
            int handNumberValue = 0;
            // Amount of aces in the hand
            int aces = 0;

            // For every card in the hand...
            foreach (Card card in hand)
            {
                // Track the amount of aces
                if (card.value is Value.Ace) aces++;
                // Get the value of the card and add it to the total
                handNumberValue += GetCardNumberValue(card);
            }

            // Ace reduction if over 21 and there is aces
            if (handNumberValue > 21 && aces > 0)
            {
                // For every ace in the hand...
                for (int i = 0; i < aces; i++)
                {
                    // Only reduce an ace if it goes over 21
                    if (handNumberValue > 21)
                    {
                        // Reduce the ace to 1
                        handNumberValue -= 10;
                    }
                }
            }

            // Return the amount
            return handNumberValue;
        }

        /// <summary>
        /// Gives an amount of cards to a player
        /// </summary>
        /// <param name="player"></param>
        /// <param name="amount"></param>
        /// <exception cref="Exception"></exception>
        private void DealToPlayer(Player player, bool isHidden = false)
        {
            if (deck.deck.Count > 0) // Checks to make sure there is cards to choose from
            {
                Random rand = new Random();
                // Wait for a breif moment
                int waitTime = rand.Next(650, 750);
                System.Threading.Thread.Sleep(waitTime);

                // Get a card
                Card card = deck.GetCardRandom();
                // Give the card to the player
                player.hand.Add(card);
                // If the player should be hidden
                if (isHidden) card.isFaceUp = false;

                // Play sound
                Audio.DealCardSound();
                // Update scene
                ShowScene();
            }
            else throw new Exception("No cards left!");
        }

        #endregion

    }
}
