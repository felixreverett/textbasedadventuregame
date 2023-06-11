using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextBasedAdventureGame
{
    public static class Draw
    {
        //draws the player's inventory dynamically (limited to 999999 items)
        public static void DrawInventory(Player player)
        {
            string heading = $" {player.Name}'s Inventory ";
            int totalWidth = heading.Length;
            int columnTwoWidth = 8;

            foreach (KeyValuePair<string, int> item in player.Inventory)
            {
                int thisEntryWidth = item.Key.Length + 2 + columnTwoWidth + 1;
                if (thisEntryWidth > totalWidth)
                {
                    totalWidth = thisEntryWidth;
                }
            }
            
            int columnOneWidth = totalWidth - columnTwoWidth - 1;

            //HEADING
            //line 1
            Console.Write("╔");
            for (int i = 0; i < totalWidth; i++)
            {
                Console.Write("═");
            }
            Console.WriteLine("╗");

            //line 2
            Console.Write($"║{heading}");
            for (int i = 0; i < totalWidth - heading.Length; i++)
            {
                Console.Write(" ");
            }
            Console.WriteLine("║");

            //line 3
            Console.Write("╠");
            for (int i = 0; i < columnOneWidth ; i++)
            {
                Console.Write("═");
            }
            Console.Write("╤");
            for (int i = 0; i < columnTwoWidth ; i++)
            {
                Console.Write("═");
            }
            Console.WriteLine("╣");

            //ITEMS
            foreach (KeyValuePair<string, int> item in player.Inventory)
            {
                Console.Write($"║ {item.Key}");
                for (int i = 0; i < columnOneWidth - item.Key.Length -1; i++)
                {
                    Console.Write(" ");
                }
                Console.Write("│");

                for (int i = 0; i < columnTwoWidth - item.Value.ToString().Length - 1; i++)
                {
                    Console.Write(" ");
                }
                Console.WriteLine($"{item.Value} ║");
            }

            //END
            Console.Write("╚");
            for (int i = 0; i < columnOneWidth; i++)
            {
                Console.Write("═");
            }
            Console.Write("╧");
            for (int i = 0; i < columnTwoWidth; i++)
            {
                Console.Write("═");
            }
            Console.WriteLine("╝");
        }
    }
}
