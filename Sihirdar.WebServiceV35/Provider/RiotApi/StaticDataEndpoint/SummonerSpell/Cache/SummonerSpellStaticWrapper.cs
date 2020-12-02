﻿using Sihirdar.WebServiceV3.Provider.RiotApi.Misc;

namespace Sihirdar.WebServiceV3.Provider.RiotApi.StaticDataEndpoint.SummonerSpell.Cache
{
    public class SummonerSpellStaticWrapper
    {
        public SummonerSpellStatic SummonerSpellStatic { get; private set; }
        public Language Language { get; private set; }
        public SummonerSpellData SummonerSpellData { get; private set; }

        public SummonerSpellStaticWrapper(SummonerSpellStatic spell, Language language
            , SummonerSpellData summonerSpellData)
        {
            SummonerSpellStatic = spell;
            Language = language;
            SummonerSpellData = summonerSpellData;
        }
    }
}