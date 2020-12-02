﻿using System.Collections.Generic;

namespace Tools.Models
{
    public class AovChampDetailDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Key { get; set; }
        public string Image { get; set; }
        public string Role { get; set; }
        public string Partype { get; set; }
        public string Lore { get; set; }
        public string Quote { get; set; }

        public int Gold { get; set; } = 0;
        public int Ticket { get; set; } = 0;
        public int InfoDifficulty { get; set; } = 0;
        public int InfoAttack { get; set; } = 0;
        public int InfoConst { get; set; } = 0;
        public int InfoMagic { get; set; } = 0;
        public StatsDto Stats { get; set; }
        public List<SkinDto> Skins { get; set; }
        public List<SpellDto> Spells { get; set; }
    }

    public class SpellDto
    {
        public int Id { get; set; }
        public int Num { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
    }

    public class SkinDto
    {
        public int Id { get; set; }
        public int Num { get; set; } = 0;
        public string Name { get; set; }
        public string Picture { get; set; }
    }

    public class StatsDto
    {
        public int Ad { get; set; } = 0;
        public int Adperlevel { get; set; } = 0;
        public int Ap { get; set; } = 0;
        public int Apperlevel { get; set; } = 0;
        public int Hp { get; set; } = 0;
        public int Hpperlevel { get; set; } = 0;
        public int Hpregen { get; set; } = 0;
        public int Hpregenperlevel { get; set; } = 0;
        public int Manaregen { get; set; } = 0;
        public int Manaregenperlevel { get; set; } = 0;
        public int Armor { get; set; } = 0;
        public int ArmorPerLevel { get; set; } = 0;
        public int Mr { get; set; } = 0;
        public int MrPerLevel { get; set; } = 0;
        public int As { get; set; } = 0;
        public int AsPerLevel { get; set; } = 0;
        public int Cd { get; set; } = 0;
        public int CdPerLevel { get; set; } = 0;
        public int Critic { get; set; } = 0;
        public int CriticPerLevel { get; set; } = 0;
        public int Movement { get; set; } = 0;
        public int MovementPerLevel { get; set; } = 0;
    }
}