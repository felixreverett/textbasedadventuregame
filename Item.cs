namespace TextBasedAdventureGame
{
    public class Item
    {
        public string ItemName { get; set; } = string.Empty;
        public Dictionary<string, int>? CraftingRecipe { get; set; }
        public bool IsEdible { get; set; }
    }
}
