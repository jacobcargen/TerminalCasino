using System;
using System.Collections.Generic;
using System.Text;

namespace TerminalCasino
{
    internal class Bots 
    {
        private static string[] names = new string[]
        {
            "Kenneth", "Elfrieda", "Lynnette",
            "Murray", "Brice", "Emersyn",
            "Kathryn", "Dawn", "Rosalind",
            "Eileen", "Sam", "Roxane",
            "Di", "Emmy", "Arline",
            "Danita", "Letty", "Mary Beth",
            "Sharla", "Bertina", "Deanna",
            "Melina", "Carson", "Kendrick",
            "Rudy", "Nanette", "Nelson",
            "Addie", "Danni", "Dexter",
        };

        /// <summary>
        /// Call this to recieve a bot player.
        /// </summary>
        /// <returns>A random player with a name and an amount of money</returns>
        public static Player GetRandomAvailableBot()
        {
            Player player = new Player();
            player.name = RandomName();
            player.money = 500;//RandomMoney(100, 2500);
            player.isUser = false;
            return player;
        }
        private static string RandomName()
        {
            Random rand = new Random();
            int index = rand.Next(0, names.Length);
            string name = names[index];
            return name;
        }
        private static int RandomMoney(int min, int max)
        {
            Random rand = new Random();
            int money = rand.Next(min, max);
            return money;
        }
    }
}
