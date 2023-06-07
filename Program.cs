using System.Text.Json;
using static TextBasedAdventureGame.Objects;

namespace TextBasedAdventureGame
{
    internal class Program
    {
        static void Main(string[] args)
        {
            
            Game game = new Game("default username");
            
            game.GameStartup();

            //string jsonString = JsonSerializer.Serialize(mySeason);
            //File.WriteAllText(@$"D:\Programming\C#\TextBasedAdventureGame\Resources\Seasons\{mySeason.seasonName}.json", jsonString);

        }
    }
}