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
            public string seasonName { get; set; }
            public int temperatureModifier { get; set; }
            public int humidityModifier { get; set; }
            public Dictionary<string, int>? weatherWeights { get; set; }
        }

        public class Weather
        {
            public int baseHumidity;
            public int baseTemperature;
            public float visibility;
        }

        public class Biome
        {
            public string BiomeName { get; set; }
            public string LootTableFilePath { get; set; }
            public string Climate { get; set; }
            public LootTable LootTable { get; set; }

        }
    }
}
