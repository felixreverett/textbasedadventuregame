﻿using System.Runtime.CompilerServices;
using System.Text.Json;
using static TextBasedAdventureGame.Objects;
using static TextBasedAdventureGame.Utils;

namespace TextBasedAdventureGame
{
    public class Game
    {
        //PROPERTIES
        bool GameIsRunning { get; set; }
        bool DayIsRunning { get; set; }
        int DayOfMonth { get; set; }
        List<Season> SeasonsList { get; set; }
        int CurrentSeasonId { get; set; }
        Season CurrentSeason { get; set; }
        Weather CurrentWeather { get; set; }
        Dictionary<string, Dictionary<string, string>> Commands { get; set; }
        List<Biome> BiomesList { get; set; }
        List<Item> ItemsList { get; set; }
        List<Weather> WeathersList { get; set; }
        Biome CurrentBiome { get; set; }
        Player Player { get; set; }

        //CONSTRUCTORS
        public Game(string playerName)
        {
            DayOfMonth = 0;
            SeasonsList = LoadAllObjectsIntoList<Season>(folderPathSeasons);
            BiomesList = LoadBiomes(folderPathBiomes);
            ItemsList = LoadAllObjectsIntoList<Item>(folderPathItems);
            WeathersList = LoadAllObjectsIntoList<Weather>(folderPathWeathers);
            CurrentSeasonId = 0;
            CurrentSeason = SeasonsList[0];
            CurrentBiome = SetBiome();
            CurrentWeather = new Weather();
            Commands = LoadObject<Dictionary<string, Dictionary<string, string>>>(filePathCommands); //it does do things :)
            Player = new Player(playerName);
        }

        //METHODS
        public void GameStartup()
        {
            Console.WriteLine("Game Loading...");
            Console.WriteLine($"Welcome, {Player.Name}.");
            GameLoop();
        }

        public void GameLoop()
        {
            GameIsRunning = true;

            while (GameIsRunning)
            {
                NextDay();                   
                DayIsRunning = true;

                Console.WriteLine($"Today is day {DayOfMonth} of {CurrentSeason.SeasonName}");
                Console.WriteLine(GetWeatherSplash()); //write the current weather
                Console.WriteLine($"You wake up in a {CurrentBiome.BiomeName} biome."); //where are you
                
                while (DayIsRunning)
                {
                    HandleCommand(Console.ReadLine()!);
                }
            }
        }

        //return true if name matches query, and is craftable
        public bool IsRecipeInItemList(string itemName)
        {
            if (ItemsList.Any(item => item.ItemName == itemName && item.CraftingRecipe != null))
            {
                return true;
            }
            return false;
        }

        //Only remaining custom Load method. Todo: change that
        public List<Biome> LoadBiomes(string folderPath)
        {
            List<Biome> BiomesList = new List<Biome>();

            foreach (string s in Directory.GetFiles(folderPath))
            {
                if (s.EndsWith(".json"))
                {
                    Biome biome = LoadObject<Biome>(s);
                    biome.LootTable = LoadObject<LootTable>($"{folderPathLootTables}{biome.LootTableFilePath}")!;
                    BiomesList.Add(biome);
                }
            }

            return BiomesList;
        }

        //Generate Loot
        public Dictionary<string, int> GenerateLoot(LootTable lootTable)
        {
            Dictionary<string, int> Loot = new();

            //for every pool in the LootTable
            foreach (Pool pool in lootTable.Pools)
            {
                Random random = new Random();
                int numberOfRolls = random.Next(pool.Rolls.Min, pool.Rolls.Max);

                //get sum of weights
                int sumOfWeights = 0;
                foreach (Entry entry in  pool.Entries)
                {
                    sumOfWeights += entry.Weight;
                }

                //for every roll
                for (int i = 0; i < numberOfRolls; i++)
                {
                    //grab a random item accounting for its weight
                    int r = random.Next(sumOfWeights);
                    foreach (Entry entry in pool.Entries)
                    {
                        r -= entry.Weight;
                        if (r <= 0)
                        {
                            if (!Loot.ContainsKey(entry.Name))
                            {
                                Loot.Add(entry.Name, 1);
                            }
                            else
                            {
                                Loot[entry.Name]++;
                            }
                            break;
                        }
                    }
                }
            }
            return Loot;
        }
       
        //GET METHODS
        public string GetWeatherSplash()
        {
            Random random = new Random();
            int r = random.Next(CurrentWeather.Splashes.Length);
            return CurrentWeather.Splashes[r];
        }

        //SET METHODS
        public Weather SetWeather(Season season)
        {
            int sumOfWeights = season.WeatherWeights.Values.Sum();
            Random random = new Random();
            int r = random.Next(sumOfWeights);
            foreach (string weather in season.WeatherWeights.Keys)
            {
                r -= season.WeatherWeights[weather];
                if (r <= 0)
                {
                    return WeathersList.First(w => w.WeatherName == weather);
                }
            }
            return new(); //I sure hope this isn't a bad coding practice
        }

        public Biome SetBiome()
        {
            Random random = new Random();
            int r = random.Next(BiomesList.Count);
            return BiomesList[r];
        }

        //NEXT METHODS
        public void NextDay()
        {
            DayOfMonth++;
            if (DayOfMonth > 28)
            {
                DayOfMonth = 1;
                NextSeason();
            }
            CurrentWeather = SetWeather(CurrentSeason);
            //humidity = SetHumidity(Season, Weather);
            //temperature = SetTemperature(Season, Weather);
        }

        public void NextSeason()
        {
            CurrentSeasonId++;
            if (CurrentSeasonId >= SeasonsList.Count)
            {
                CurrentSeasonId = 0;
            }
            CurrentSeason = SeasonsList[CurrentSeasonId];
        }


        public void HandleCommand(string input)
        {
            string[] splitInput = input.Split(" ");
            switch(splitInput[0])
            {
                //if an item is specified, try craft 1 or specified amount of that item
                case "/craft":
                    {
                        if (splitInput.Length > 1 && IsRecipeInItemList(splitInput[1]))
                        {
                            Item item = ItemsList.First(recipe => recipe.ItemName == splitInput[1]);
                            CraftItemResult result = CraftItemResult.Null;
                            if (splitInput.Length > 2 && Int32.TryParse(splitInput[2], out int amount))
                            {
                                result = Player.TryCraftItem(item, splitInput[1], Math.Abs(amount));
                            }

                            else
                            {
                                result = Player.TryCraftItem(item, splitInput[1]);
                            }

                            if (result == CraftItemResult.FullSuccess)
                            {
                                Console.WriteLine("Item successfully crafted.");
                            }

                            if (result == CraftItemResult.NotEnoughItems)
                            {
                                Console.WriteLine("You don't have enough items to craft that!");
                            }

                        }

                        else
                        {
                            Console.WriteLine("No such recipe found!");
                        }

                        break;
                    }
                case "/eat":
                    {
                        if (splitInput.Length > 1 && Player.Inventory.ContainsKey(splitInput[1]))
                        {
                            Item item = ItemsList.First(i => i.ItemName == splitInput[1]);
                            int playerEnergy = Player.CurrentEnergy;
                            Player.ConsumeItem(item);
                            Console.WriteLine($"> Consumed 1 {splitInput[1]}, restoring {Player.CurrentEnergy - playerEnergy} energy.");
                        }
                        else
                        {
                            Console.WriteLine("! You don't have enough items to do that");
                        }
                        break;
                    }
                case "/energy":
                    {
                        Console.WriteLine($">{Player.Name}'s current energy: {Player.CurrentEnergy}/{Player.MaxEnergy}");
                        break;
                    }
                case "/exit":
                    {
                        Console.WriteLine("this isn't implemented yet.");
                        break;
                    }
                case "/forage":
                    {
                        Console.WriteLine("> You have a look around to forage.");
                        Dictionary<string, int> loot = GenerateLoot(CurrentBiome.LootTable);
                        Player.AddItemsToInventory(loot);
                        break;
                    }
                case "/help":
                    {
                        Console.WriteLine("Here is a list of valid commands:");
                        foreach (string s in Commands.Keys)
                        {
                            Console.WriteLine($"{s}: {Commands[s]["description"]}");
                        }
                        break;
                    }
                case "/inventory" or "/i":
                    {
                        Draw.DrawInventory(Player);
                        break;
                    }
                case "/save":
                    {
                        Console.WriteLine("as if I'd have already added this!");
                        break;
                    }
                case "/sleep":
                    {
                        DayIsRunning = false;
                        break;
                    }
                case "/travel":
                    {
                        CurrentBiome = SetBiome();
                        Console.WriteLine($"You enter a {CurrentBiome.BiomeName} biome.");
                        break;
                    }
                default:
                    Console.WriteLine("Unrecognised Command. Please type /help to show a list of valid commands.");
                    break;
            }
        }

        //Filepaths
        string folderPathSeasons = @"..\..\..\Resources\Seasons";
        string filePathCommands = @"..\..\..\Resources\commands.json";
        string folderPathBiomes = @"..\..\..\Resources\Biomes";
        string folderPathLootTables = @"..\..\..\Resources\LootTables";
        string folderPathItems = @"..\..\..\Resources\Items";
        string folderPathWeathers = @"..\..\..\Resources\Weathers";

    }
}
