using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextBasedAdventureGame
{
    public class Player
    {
        public Player(string playerName)
        {
            Name = playerName;
            Inventory = new Dictionary<string, int>();
            Health = 20;
        }

        public string Name { get; set; }
        public Dictionary<string, int> Inventory { get; set; }
        public int Health { get; set; }

        public void AddItemsToInventory(Dictionary<string, int> items)
        {
            foreach (KeyValuePair<string, int> pair in items) 
            { 
                if (!Inventory.ContainsKey(pair.Key))
                {
                    Inventory.Add(pair.Key, pair.Value);
                }
                else
                {
                    Inventory[pair.Key] += pair.Value;
                }
            }
        }
    }
}
