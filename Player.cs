using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq;
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

        //todo: probably remove this as a member of Player
        public void AddItemsToInventory(Dictionary<string, int> items)
        {
            foreach (KeyValuePair<string, int> pair in items)
            {
                AddItemToInventory(pair.Key, pair.Value);
            }
        }

        //adds "amount" of items to existing key in inventory, or creates new entry
        public void AddItemToInventory(string itemName, int amount)
        {
            if (!Inventory.ContainsKey(itemName))
            {
                Inventory.Add(itemName, amount);
            }
            else
            {
                Inventory[itemName] += amount;
            }
        }

        //removes "amount" items from inventory, or removes item if "amount" == item amount
        public void RemoveItemFromInventory(string itemName, int amount)
        {
            if (amount == Inventory[itemName])
            {
                Inventory.Remove(itemName);
            }
            else
            {
                Inventory[itemName] -= amount;
            }
        }

        //craft "amount" or "craftingLimit" number of items from recipe
        public CraftItemResult TryCraftItem(Objects.CraftingRecipe craftingRecipe, string itemName, int amount = 1)
        {
            CraftItemResult result = CraftItemResult.FullSuccess;
            int craftingLimit = amount;
            int currentCraftingLimit = 0;
            foreach (KeyValuePair<string, int> item in craftingRecipe.ItemRecipe)
            {
                if (Inventory.ContainsKey(item.Key) && Inventory[item.Key] >= item.Value)
                {
                    currentCraftingLimit = Inventory[item.Key] / item.Value;
                    if (currentCraftingLimit < craftingLimit)
                    {
                        craftingLimit = currentCraftingLimit;
                    }
                }
                else
                {
                    return CraftItemResult.NotEnoughItems;
                }
            }

            if (amount > craftingLimit)
            {
                amount = craftingLimit;
                result = CraftItemResult.PartialSuccess;
            }

            Console.WriteLine($"Crafting {amount} of {itemName}");

            foreach (KeyValuePair<string, int> item in craftingRecipe.ItemRecipe)
            {
                RemoveItemFromInventory(item.Key, item.Value * amount);
            }

            AddItemToInventory(craftingRecipe.ItemName, amount);

            return result;
        }
    }
}
