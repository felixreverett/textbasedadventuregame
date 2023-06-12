using System.Text.Json;
using static TextBasedAdventureGame.Objects;

namespace TextBasedAdventureGame
{
    internal class Program
    {
        static void Main(string[] args) //Todo: add a difficulty level for new games
        {
            Console.Write(
                "╔═══════════════════════════════════════════════╗\n" +
                "║ Welcome to Felix's Text-Based Adventure Game! ║\n" +
                "╚═══════════════════════════════════════════════╝\n" +
                "> Please select between LOAD GAME and NEW GAME.\n> ");
            
            bool isSelecting = true; string input = "none";

            while (isSelecting)
            {
                input = Console.ReadLine()!.ToLower().Split(" ")[0];
                if (input is "new" or "load")
                {
                    isSelecting = false;
                }
                else
                {
                    Console.Write("> Please write a valid input.\n> ");
                }
            }

            if (input == "new")
            {
                Console.WriteLine(
                    "╔═══════════════════════════════════════════════╗\n" +
                    "║            Starting a NEW GAME!               ║\n" +
                    "╚═══════════════════════════════════════════════╝");
                
                string username = string.Empty;

                while (username == string.Empty)
                {
                    Console.Write("> Please choose a username: ");
                    username = Console.ReadLine()!;
                }

                Game game = new Game(username);
                game.GameStartup();
            }

            else
            {
                Console.WriteLine("Sorry, this isn't added yet");
            }
            
        }
    }
}