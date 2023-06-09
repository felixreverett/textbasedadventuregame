﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextBasedAdventureGame
{
        public class RollRange
        {
            public int Min { get; set; }
            public int Max { get; set; }
        }

        public class Entry
        {
            public string Name { get; set; } = string.Empty;
            public int Weight { get; set; }
        }

        public class Pool
        {
            public RollRange Rolls { get; set; } = new();
            public List<Entry> Entries { get; set; } = new();
        }

        public class LootTable
        {
            public List<Pool> Pools { get; set; } = new();
        }
}
