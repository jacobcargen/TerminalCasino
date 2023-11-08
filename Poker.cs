using System;
using System.Collections.Generic;
using System.Text;

namespace TerminalCasino
{
    public class Poker
    {
        // Players in the game, this includes the players that have folded
        public List<Player> players = new List<Player>();
        // Whos turn is it?
        protected Player currentPlayerTurn = null;
        // the current dealer
        private Player dealer;
        // total value in the pot
        private int potValue = 0;
        // Max amount a player is allowed to raise (not implemented yet)
        private int raiseLimit = 500;
        // Cost to play (not implemented yet)
        private int ante = 0;
        // Current raise each player must pay to play
        private int raiseMatchAmt = 0;
        // The deck of cards to be used
        private Deck deck = new Deck();
        // Possible choices during a players turn.
        public enum Choice
        {
            None,
            Bet,
            Check,
            Call,
            TapIn,
            Raise,
            Fold,
        }
        // Keybinds for the choices
        private ConsoleKey betKey = ConsoleKey.Enter;
        private ConsoleKey checkKey = ConsoleKey.Spacebar;
        private ConsoleKey callKey = ConsoleKey.Spacebar;
        private ConsoleKey tapInKey = ConsoleKey.Spacebar;
        private ConsoleKey raiseKey = ConsoleKey.Enter;
        private ConsoleKey foldKey = ConsoleKey.Backspace;


        /// <summary>
        /// Shows the game view to the player.
        /// This will clear the terminal and show the scene.
        /// </summary>
        /// <exception cref="Exception"></exception>
        protected void ShowScene()
        {
            Commands.Clear();
            Tools.TellUser($"POT:${potValue} RAISE:${raiseMatchAmt}");
            foreach (Player player in players)
            {
                string turnNotify = string.Empty;
                string decision, dealerString = "    ";
                if (player == dealer) dealerString = "[D] ";
                // if the player has folded
                if (player.hasFolded)
                {
                    decision = "Folded";
                }
                if (player == currentPlayerTurn)
                {
                    turnNotify = "<<<---<<<---<<<";
                    decision = "...";
                }
                else
                {
                    if (player.lastChoice != Choice.None)
                    {
                        switch (player.lastChoice)
                        {
                            case Choice.Check: decision = "Checked"; break;
                            case Choice.Bet: decision = $"Bet"; break;
                            case Choice.Call: decision = $"Called"; break;
                            case Choice.TapIn: decision = $"Tapped In"; break;
                            case Choice.Raise: decision = $"Raised"; break;
                            case Choice.Fold: decision = $"Folded"; break;
                            default: throw new Exception("NOT VALID!");
                        }
                    }
                    else // If choice NONE
                    {
                        decision = "...";
                    }
                }

                foreach (Card card in player.hand)
                {
                    // If this is the players hand
                    if (player.isUser) card.isFaceUp = true;

                    card.Refresh();
                }


                Tools.TellUser($"\n{dealerString}{player.name} : ${player.money} [{decision}] {turnNotify}");
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
        }

        #region Turns & Players & Rounds

        /// <summary>
        /// Ask the player to check/call/tapin/raise/or fold.
        /// If it is a bot, the bot will decide.
        /// </summary>
        protected void GetPlayerAction()
        {
            Program.SaveUser();
            Player player = currentPlayerTurn;


            // If it is a bot
            if (!player.isUser)
            {
                // Get a bot choice instead
                BotChoice(player);
                return;
            }

            // text for the user
            string choicesText = " - Press a key - ";
            string bet = "\nEnter : Bet";
            string check = "\nSpace : Check";
            string call = $"\nSpace : Call";
            string tapIn = "\nSpace : Tap In";
            string raise = "\nEnter : Raise";
            string fold = "\nBackspace : Fold";
            // Choices the user has
            List<Choice> choices = GetAvailableChoices(player);

            // Gets a list of available user choices and add those to a viewable list for the user
            foreach (Choice choice in choices)
            {
                // Add the available choices to the printed text
                switch (choice)
                {
                    case Choice.Bet:
                        choicesText += String.Concat(bet);
                        break;
                    case Choice.Check:
                        choicesText += String.Concat(check);
                        break;
                    case Choice.Call:
                        choicesText += String.Concat(call + $" ${raiseMatchAmt - player.currentRaise}");
                        break;
                    case Choice.TapIn:
                        choicesText += String.Concat(tapIn);
                        break;
                    case Choice.Raise:
                        choicesText += String.Concat(raise);
                        break;
                    case Choice.Fold:
                        choicesText += String.Concat(fold);
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
                Choice choice = CheckChoice(key, choices, player);
                // Loops until a valid response is given
                if (choice != Choice.None) break;
                else ShowScene();
            }
            while (true);

            IsGameOver();
        }

        /// <summary>
        /// Call this at the start of every round. 
        /// This will reset and prepare each round.
        /// </summary>
        protected void NewRound()
        {
            IsRoundOver();
            // Players
            foreach (Player player in players)
            {
                player.currentRaise = 0;
                player.hasCheckedCalledRaised = false;
                player.lastChoice = Choice.None;
            }
            // Game
            raiseMatchAmt = 0;
            currentPlayerTurn = null;
        }

        /// <summary>
        /// Get the next dealer, or a starting one
        /// </summary>
        protected void NextDealer()
        {
            // If no dealer yet
            if (dealer == null)
            {
                Random rand = new Random();
                // Get a random index
                int index = rand.Next(0, players.Count);
                // Decide the player at that index to be the dealer
                Player player = players[index];
                // Set the dealer
                dealer = player;
            }
            // Next dealer
            else
            {
                // Get the next player after the current dealer to be the dealer
                dealer = GetNextPlayer(dealer);
            }
        }

        /// <summary>
        /// Sets the current player turn to the next player after the current
        /// </summary>
        protected void NextPlayer()
        {
            // If no current player yet
            if (currentPlayerTurn == null)
            {
                // Get the starting player after the dealer
                currentPlayerTurn = GetNextPlayer(dealer);
            }
            // Go to the next player
            else
            {
                currentPlayerTurn = GetNextPlayer(currentPlayerTurn);
            }
            if (currentPlayerTurn.hasFolded) NextPlayer();
        }

        protected void NewGame(Player user, int ante, int maxRaise, int amtOfBots)
        {
            this.ante = ante;
            raiseLimit = maxRaise;
            AddPlayers(user, amtOfBots);
            ResetGameAndPlayer();
        }

        #region Local

        /// <summary>
        /// Add players to the game, this is recommneded at the start before any rounds.
        /// Make sure there is not too many, running out of cards could cause a crash.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="amtOfBots"></param>
        private void AddPlayers(Player user, int amtOfBots)
        {
            // Add the user
            players.Add(user);
            // Add a bot for every amountOfBots
            for (int i = 0; i < amtOfBots; i++)
            {
                // Get a bot
                Player bot = Bots.GetRandomAvailableBot();
                // Add the bot to the players in game
                players.Add(bot);
            }
            // Interate over players in game
            foreach (Player player in players)
            {
                // Make sure they have no cards in hand
                player.hand = new List<Card>();
            }
        }

        /// <summary>
        /// Resets the game and the player.
        /// </summary>
        private void ResetGameAndPlayer()
        {
            // Reset game
            dealer = null;
            currentPlayerTurn = null;
            potValue = 0;
            deck = new Deck();

            // Reset the players
            foreach (Player player in players)
            {
                player.lastChoice = Choice.None;
                player.hasCheckedCalledRaised = false;
                player.currentRaise = 0;
                player.hasFolded = false;
                player.isTappedIn = false;
                player.hand = new List<Card>();
            }
        }

        /// <summary>
        /// Returns the next player in turn.
        /// Make sure there is at least one player or the program will crash.
        /// </summary>
        /// <param name="lastPlayer"></param>
        /// <returns>Next player</returns>
        private Player GetNextPlayer(Player lastPlayer)
        {
            Player nextPlayer = null;
            foreach (Player player in players)
            {
                if (player == lastPlayer)
                {
                    int lastIndex = players.IndexOf(player);

                    if (lastIndex + 1 < players.Count)
                    {
                        nextPlayer = players[lastIndex + 1];
                    }
                    else
                    {
                        // Loop back to beginning
                        nextPlayer = players[0];
                    }
                }
            }
            if (nextPlayer.hasFolded)
            {
                GetNextPlayer(nextPlayer);
            }
            return nextPlayer;
        }

        #endregion

        #endregion

        #region Cards

        /// <summary>
        /// Gives an amount of cards to a player
        /// </summary>
        /// <param name="player"></param>
        /// <param name="amount"></param>
        /// <exception cref="Exception"></exception>
        protected void DealToPlayer(Player player, bool isHidden = false)
        {
            if (deck.deck.Count > 0) // Checks to make sure there is cards to choose from
            {
                Random rand = new Random();
                int waitTime = rand.Next(600, 900);
                // Wait for a .75 seconds
                System.Threading.Thread.Sleep(waitTime);

                Card card = deck.GetCardRandom();
                player.hand.Add(card);
                if (isHidden) card.isFaceUp = false;

                // play sound
                Audio.DealCardSound();
                // Update scene
                ShowScene();
            }
            else throw new Exception("No cards left!");
        }

        /// <summary>
        /// Deals an amount of cards to all the players 
        /// </summary>
        /// <param name="amount"></param>
        protected void DealOneToAll(bool isHidden = false)
        {
            foreach (Player player in players)
            {
                // Only deal to players that are not folded
                if (!player.hasFolded) DealToPlayer(player, isHidden);
            }
        }

        private void ShowAllHands()
        {
            foreach (Player player in players)
            {
                if (!player.hasFolded)
                {
                    foreach (Card card in player.hand)
                    {
                        card.isFaceUp = true;
                    }
                }
            }
            ShowScene();
        }

        #endregion

        #region Choices

        #region Local

        /// <summary>
        /// Call this to place a bet
        /// </summary>
        /// <param name="player"></param>
        private void Bet(Player player)
        {
            player.lastChoice = Choice.Bet;
            player.hasCheckedCalledRaised = true;
            if (player.isUser) RaiseForUser(player);
            else RaiseForBot(player);
        }

        /// <summary>
        /// Call this to ask the player for a raise amount, then raise if valid
        /// </summary>
        /// <param name="player"></param>
        private void Raise(Player player)
        {
            player.lastChoice = Choice.Raise;
            player.hasCheckedCalledRaised = true;
            if (player.isUser) RaiseForUser(player);
            else RaiseForBot(player);
        }

        /// <summary>
        /// Checks
        /// </summary>
        /// <param name="player"></param>
        private void Check(Player player)
        {
            player.lastChoice = Choice.Check;
            player.hasCheckedCalledRaised = true;
        }
        
        /// <summary>
        /// Calls
        /// </summary>
        /// <param name="player"></param>
        private void Call(Player player)
        {
            player.lastChoice = Choice.Call;
            player.hasCheckedCalledRaised = true;
            int matchAmt = raiseMatchAmt - player.currentRaise; // Calcs the amt the player needs to add
            if (player.money >= matchAmt) // Can match the amount
            {
                AddToPot(player, matchAmt); // Matches the current raise
            }
            else throw new Exception("Do not tap in from call. Why wasn't TapIn called instead?");
        }

        /// <summary>
        /// This is called from the call method, if it cost more to play then the player can afford
        /// </summary>
        /// <param name="player"></param>
        private void TapIn(Player player)
        {
            player.lastChoice = Choice.TapIn;
            player.hasCheckedCalledRaised = true;
            AddToPot(player, player.money); // Tries to match the current raise
            player.isTappedIn = true; // Taps the player in
        }

        /// <summary>
        /// Folds the player
        /// </summary>
        /// <param name="player"></param>
        private void Fold(Player player)
        {
            player.lastChoice = Choice.Fold;
            player.hasFolded = true;
        }

        #region More

        /// <summary>
        /// Gets the input from the user for a raise amount.
        /// </summary>
        /// <param name="player"></param>
        private void RaiseForUser(Player player)
        {
            string input = Tools.GetInputString("Amount $"); // Get an amount of money from the user
            if (int.TryParse(input, out int amtToRaise)) // Try to parse the value to a int
            {
                if (IsValidRaiseAmt(player, amtToRaise)) // If the value is within a valid range
                {
                    AddToPot(player, amtToRaise, true); // Completes the raise
                    return;
                }
                else Tools.TellUser("Invalid amount.(1)"); // Value not in range
            }
            else Tools.TellUser("Invalid input.(2)"); // Input invalid
            Bet(player);
        }

        /// <summary>
        /// Gets a bot decision for a raise amount.
        /// </summary>
        /// <param name="bot"></param>
        /// <exception cref="Exception"></exception>
        private void RaiseForBot(Player bot)
        {
            // Find the max allowed raise
            int maxPossibleRaise;
            int availableRaiseAmt = bot.money - (raiseMatchAmt - bot.currentRaise);
            if (availableRaiseAmt > raiseLimit) maxPossibleRaise = raiseLimit;
            else maxPossibleRaise = availableRaiseAmt;


            // Get a random raise
            Random random = new Random();
            int amtToRaise = random.Next(1, maxPossibleRaise);

            if (IsValidRaiseAmt(bot, amtToRaise))
            {
                AddToPot(bot, amtToRaise, true);
            }
            else throw new Exception("Bot choose an invalid range");
        }

        /// <summary>
        /// Adds the raise to the pot and removes that amount from the players money.
        /// Call TryRaise for safe raises
        /// </summary>
        private void AddToPot(Player player, int amtOfMoney, bool addCallAmount = false)
        {
            // Adds the amount the player needs to call
            if (addCallAmount) amtOfMoney += raiseMatchAmt - player.currentRaise;

            // Add the amount to the pot
            potValue += amtOfMoney;
            // Removes the amount from the player
            player.money -= amtOfMoney;

            // Save how much the player raised
            player.currentRaise += amtOfMoney;
            // Increase the pots current raise if it was increased
            if (raiseMatchAmt < player.currentRaise)
                raiseMatchAmt = player.currentRaise;
        }

        /// <summary>
        /// Called for an AI to decide on a bots turn.
        /// Called when the player is given their choices.
        /// </summary>
        /// <param name="bot"></param>
        /// <exception cref="Exception"></exception>
        private void BotChoice(Player bot)
        {
            // Get the available choices for the bot
            List<Choice> choices = GetAvailableChoices(bot);
            // Get a new ai with this game, and player/bot
            AI ai = new AI(bot);
            // Get an AI choice for a decision
            Choice botChoice = ai.GetDecision(choices);

            Random rand = new Random();
            int rangeWaitMS = rand.Next(1500, 4500);
            // Wait for 3 seconds As if the bot is thinking
            do { System.Threading.Thread.Sleep(rangeWaitMS); } while (false);

            // Check that choice and use it
            switch (botChoice)
            {
                case Choice.Check: Check(bot); break;
                case Choice.Bet: Bet(bot); break;
                case Choice.Call: Call(bot); break;
                case Choice.TapIn: TapIn(bot); break;
                case Choice.Raise: Raise(bot); break;
                case Choice.Fold: Fold(bot); break;
                default: throw new Exception("Bot did not chose a choice!");
            }
        }

        #endregion

        #endregion

        #endregion

        #region Check/Verify

        /// <summary>
        /// Returns if the round is over
        /// </summary>
        /// <returns>True if the round is finished</returns>
        protected bool IsRoundOver()
        {
            if (IsGameOver()) return true;
            int amountPlaying = 0, amountFinished = 0;
            foreach (Player player in players)
            {
                if (!player.hasFolded)
                {
                    amountPlaying++;
                    // Has made a decision, and either has enough more OR is tapped in
                    if (player.hasCheckedCalledRaised && (player.currentRaise == raiseMatchAmt || player.isTappedIn))
                    {
                        amountFinished++;
                    }
                }
            }
            if (amountPlaying == amountFinished)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Check if the game is over.
        /// </summary>
        /// <returns>Returns true if it is over.</returns>
        protected bool IsGameOver()
        {
            List<Player> playersPlaying = new List<Player>();
            foreach (Player player in players)
            {
                if (!player.hasFolded) playersPlaying.Add(player);
            }
            if (playersPlaying.Count == 1)
            {
                Player playerWon = playersPlaying[0];
                Commands.Clear();
                Tools.TellUser($"\n{playerWon.name} won ${potValue}");
                playerWon.money += potValue;
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Checks for the game winnner.
        /// </summary>
        protected void CheckWhoWon()
        {
            List<Player> playersPlaying = new List<Player>();
            foreach (Player player in players)
            {
                if (!player.hasFolded)
                {
                    playersPlaying.Add(player);
                }
            }

            List<Player> winningPlayers = PokerHands.GetWinningPlayers(playersPlaying);
            ShowAllHands();
            int winningAmount = potValue % winningPlayers.Count; 
            foreach (Player player in winningPlayers)
            {
                Tools.TellUser($"{player.name} won ${winningAmount}");
                player.money += winningAmount;
            }
        }

        #region Local 

        /// <summary>
        /// Checks if a raise amount for a specific player is valid.
        /// </summary>
        /// <param name="amtToRaise"></param>
        /// <returns>True if the raise was valid</returns>
        private bool IsValidRaiseAmt(Player player, int amtToRaise)
        {
            // Starts as valid
            bool isValid = true;
            // Amount the player has to match before raising
            int callAmount = raiseMatchAmt - player.currentRaise;
            // Raise must be at least 1
            if (amtToRaise < 1) isValid = false;
            // Raise must less than the games limit
            if (amtToRaise > raiseLimit) isValid = false;
            // The player must have enough money for the raise
            if (amtToRaise + callAmount > player.money) isValid = false;
            // Return true if the amount was valid
            return isValid;
        }

        /// <summary>
        /// Checks for which choices are available to the player and returns it.
        /// </summary>
        /// <param name="player"></param>
        /// <returns>A list of available choices</returns>
        private List<Choice> GetAvailableChoices(Player player)
        {
            List<Choice> choices = new List<Choice>();

            // Has money
            if (player.money > 0)
            {
                // If the player needs to match
                if (player.currentRaise < raiseMatchAmt)
                {
                    choices.Add(Choice.Fold);
                    int payAmt = raiseMatchAmt - player.currentRaise;

                    // Player has only enough to call
                    if (player.money == payAmt)
                    {
                        //all in, just a call
                        choices.Add(Choice.Call);
                        return choices;
                    }
                    // Player has more than amtTopay and hasnt raised/called/or checked yet
                    else if (player.money > payAmt)
                    {
                        //raise,call
                        choices.Add(Choice.Call);
                        if (!player.hasCheckedCalledRaised) 
                            choices.Add(Choice.Raise);

                        return choices;
                    }
                    // Player has less than the required amount
                    else // player.money < payAmt
                    {
                        //tapin
                        choices.Add(Choice.TapIn);
                        return choices;
                    }
                }
                else if (player.currentRaise == raiseMatchAmt)
                {
                    // Can check or bet
                    choices.Add(Choice.Bet);
                    choices.Add(Choice.Check);
                    return choices;
                }
                else throw new Exception("Player raise amount is higher than the games current round raise!");
            }
            // No money
            else
            {
                // Can only tapin
                if (player.currentRaise < raiseMatchAmt)
                {
                    // tapin
                    choices.Add(Choice.TapIn);
                    return choices;
                }
                // Can check but not place a bet
                else if (raiseMatchAmt == player.currentRaise)
                {
                    //check
                    choices.Add(Choice.Check);
                    return choices;
                }
                else throw new Exception("Player raise amount is higher than the games current round raise!");
            }
        }

        /// <summary>
        /// Checks the users choice and returns it, if it is valid
        /// </summary>
        /// <param name="key"></param>
        /// <param name="availableChoices"></param>
        /// <returns>Users choice</returns>
        private Choice CheckChoice(ConsoleKey key, List<Choice> availableChoices, Player player)
        {
            foreach (Choice choice in availableChoices)
            {
                // Returns the choice if it is available and the right key was pressed
                if (choice == Choice.Bet && key == betKey)
                {
                    Bet(player);
                    return choice;
                }
                if (choice == Choice.Check && key == checkKey)
                {
                    Check(player);
                    return choice;
                }
                if (choice == Choice.Call && key == callKey)
                {
                    Call(player);
                    return choice;
                }
                if (choice == Choice.TapIn && key == tapInKey)
                {
                    TapIn(player);
                    return choice;
                }
                if (choice == Choice.Raise && key == raiseKey)
                {
                    Raise(player);
                    return choice;
                }
                if (choice == Choice.Fold && key == foldKey)
                {
                    Fold(player);
                    return choice;
                }
            }
            Tools.TellUser("Invalid!");
            // Returns invalid if a choice was not found
            return Choice.None;
        }

        #endregion

        #endregion
    }
}
