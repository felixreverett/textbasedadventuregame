using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextBasedAdventureGame
{
    public class Objects
    {
        //every instance of Season must have these members:
        public class Season
        {
            public string SeasonName { get; set; } = string.Empty;
            public int TemperatureBase { get; set; }
            public int HumidityModifier { get; set; }
            public Dictionary<string, int>? WeatherWeights { get; set; }
        }

        public class Weather
        {
            public string WeatherName { get; set; } = string.Empty;
            public int TemperatureMod { get; set; }
            public int BaseHumidity { get; set; }
            public float Visibility { get; set; }
            public string[] Splashes { get; set; }
        }

        public class Biome
        {
            public string BiomeName { get; set; } = string.Empty;
            public string LootTableFilePath { get; set; } = string.Empty;
            public string Climate { get; set; } = string.Empty;
            public LootTable LootTable { get; set; } = new();

        }
    }
}
