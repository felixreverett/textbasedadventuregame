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
        //todo: refine this algorithm
        public CraftItemResult TryCraftItem(Item item, string itemName, int amount = 1)
        {
            CraftItemResult result = CraftItemResult.FullSuccess;
            int craftingLimit = amount;
            int currentCraftingLimit = 0;
            foreach (KeyValuePair<string, int> ingredient in item.CraftingRecipe!) //it won't be null
            {
                if (Inventory.ContainsKey(ingredient.Key) && Inventory[ingredient.Key] >= ingredient.Value)
                {
                    currentCraftingLimit = Inventory[ingredient.Key] / ingredient.Value;
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

            foreach (KeyValuePair<string, int> ingredient in item.CraftingRecipe)
            {
                RemoveItemFromInventory(ingredient.Key, ingredient.Value * amount);
            }

            AddItemToInventory(item.ItemName, amount);

            return result;
        }
    }
}
