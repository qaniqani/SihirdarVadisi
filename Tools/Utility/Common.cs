using Sihirdar.DataAccessLayer;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using Tools.Models;

namespace Tools
{
    public static class Common
    {
        public static CultureInfo Ci = new CultureInfo("tr-TR");

        //public static List<PlayedTimeMultipliers> GetPlayedTimeList = new List<PlayedTimeMultipliers>
        //{
        //    new PlayedTimeMultipliers { Name = "Suikast", Odd = 0, PlayerStatsSummaryType = PlayerStatsSummaryType.Assassinate, Time = 0 },
        //    new PlayedTimeMultipliers { Name = "Eşli Dereceli 3v3", Odd = 0, PlayerStatsSummaryType = PlayerStatsSummaryType.RankedPremade3x3, Time = 0 },
        //    new PlayedTimeMultipliers { Name = "Eşli Dereceli 5v5", Odd = 0, PlayerStatsSummaryType = PlayerStatsSummaryType.RankedPremade5x5, Time = 0 },
        //    new PlayedTimeMultipliers { Name = "ARAM", Odd = 2, PlayerStatsSummaryType = PlayerStatsSummaryType.AramUnranked5x5, Time = 25 },
        //    new PlayedTimeMultipliers { Name = "Yükseliş Modu", Odd = 2, PlayerStatsSummaryType = PlayerStatsSummaryType.Ascension, Time = 35 },
        //    new PlayedTimeMultipliers { Name = "Black Market Brawlers games", Odd = 2, PlayerStatsSummaryType = PlayerStatsSummaryType.Bilgewater, Time = 35 },
        //    new PlayedTimeMultipliers { Name = "Takım Kurucu", Odd = 2, PlayerStatsSummaryType = PlayerStatsSummaryType.CAP5x5, Time = 35 },
        //    new PlayedTimeMultipliers { Name = "Sihirdar Vadisi Bot", Odd = 1.2m, PlayerStatsSummaryType = PlayerStatsSummaryType.CoopVsAI, Time = 25 },
        //    new PlayedTimeMultipliers { Name = "Uğursuz Koruluk Bot", Odd = 1.2m, PlayerStatsSummaryType = PlayerStatsSummaryType.CoopVsAI3x3, Time = 25 },
        //    new PlayedTimeMultipliers { Name = "Dost Kazığı", Odd = 2, PlayerStatsSummaryType = PlayerStatsSummaryType.CounterPick, Time = 30 },
        //    new PlayedTimeMultipliers { Name = "Kartopu Savaşı 1x1", Odd = 2, PlayerStatsSummaryType = PlayerStatsSummaryType.FirstBlood1x1, Time = 25 },
        //    new PlayedTimeMultipliers { Name = "Kartopu Savaşı 2x2", Odd = 2, PlayerStatsSummaryType = PlayerStatsSummaryType.FirstBlood2x2, Time = 25 },
        //    new PlayedTimeMultipliers { Name = "Altıda Altı Uğursuz Koruluk", Odd = 2, PlayerStatsSummaryType = PlayerStatsSummaryType.Hexakill, Time = 35 },
        //    new PlayedTimeMultipliers { Name = "Poro Kralı", Odd = 2, PlayerStatsSummaryType = PlayerStatsSummaryType.KingPoro, Time = 35 },
        //    new PlayedTimeMultipliers { Name = "Kıyamet Botları", Odd = 1.2m, PlayerStatsSummaryType = PlayerStatsSummaryType.NightmareBot, Time = 35 },
        //    new PlayedTimeMultipliers { Name = "Kristal Kayalık", Odd = 2, PlayerStatsSummaryType = PlayerStatsSummaryType.OdinUnranked, Time = 20 },
        //    new PlayedTimeMultipliers { Name = "Birimiz Hepimiz İçin", Odd = 2, PlayerStatsSummaryType = PlayerStatsSummaryType.OneForAll5x5, Time = 25 },
        //    new PlayedTimeMultipliers { Name = "Dereceli Sihirdar Vadisi Solo", Odd = 2, PlayerStatsSummaryType = PlayerStatsSummaryType.RankedSolo5x5, Time = 35 },
        //    new PlayedTimeMultipliers { Name = "Dereceli Uğursuz Koruluk", Odd = 2, PlayerStatsSummaryType = PlayerStatsSummaryType.RankedTeam3x3, Time = 35 },
        //    new PlayedTimeMultipliers { Name = "Dereceli Sihirdar Vadisi", Odd = 2, PlayerStatsSummaryType = PlayerStatsSummaryType.RankedTeam5x5, Time = 35 },
        //    new PlayedTimeMultipliers { Name = "Esnek Dereceli SR", Odd = 2, PlayerStatsSummaryType = PlayerStatsSummaryType.RankedFlexSR, Time = 35 },
        //    new PlayedTimeMultipliers { Name = "Esnek Dereceli TT", Odd = 2, PlayerStatsSummaryType = PlayerStatsSummaryType.RankedFlexTT, Time = 35 },
        //    new PlayedTimeMultipliers { Name = "Merkez Kuşatması", Odd = 2, PlayerStatsSummaryType = PlayerStatsSummaryType.Siege, Time = 20 },
        //    new PlayedTimeMultipliers { Name = "Altıda Altı Sihirdar Vadisi", Odd = 2, PlayerStatsSummaryType = PlayerStatsSummaryType.SummonersRift6x6, Time = 35 },
        //    new PlayedTimeMultipliers { Name = "Sihirdar Vadisi", Odd = 2, PlayerStatsSummaryType = PlayerStatsSummaryType.Unranked, Time = 35 },
        //    new PlayedTimeMultipliers { Name = "Uğursuz Koruluk", Odd = 2, PlayerStatsSummaryType = PlayerStatsSummaryType.Unranked3x3, Time = 30 },
        //    new PlayedTimeMultipliers { Name = "URF", Odd = 2, PlayerStatsSummaryType = PlayerStatsSummaryType.URF, Time = 30 },
        //    new PlayedTimeMultipliers { Name = "Botlara Karşı URF", Odd = 1.2m, PlayerStatsSummaryType = PlayerStatsSummaryType.URFBots, Time = 30 },
        //    //new PlayedTimeMultipliers { Name = "Tüm Rastgele Haberci Savaşı", Odd = 1.2m, PlayerStatsSummaryType = PlayerStatsSummaryType.Arsr, Time = 30 }
        //};

        public static string StripHtml(string txt)
        {
            return Regex.Replace(txt, "<(.|\\n)*?>", string.Empty);
        }

        public static string UsernameConvert(string username)
        {
            username = username.Trim();
            //username = username.Replace("I", "İ");
            //username = username.Replace("ı", "i");
            username = username.Replace(" ", "");

            username = username.ToLower(Ci);

            return username;
        }

        public static readonly Dictionary<GameTypes, string> GetGameTypeText = new Dictionary<GameTypes, string>
        {
            {GameTypes.All, "Tümü"},
            {GameTypes.LOL, "League of Legends"},
            {GameTypes.CsGo, "CS:Go"},
            {GameTypes.Dota2, "DOTA 2"},
            {GameTypes.PubG, "PubG"},
            {GameTypes.Overwatch, "Overwatch"},
            {GameTypes.Heartstone, "Heartstone"},
            {GameTypes.StrikeOfKings, "Arena of Valor"},
            {GameTypes.OtherNews, "Diğer Haberler"}
        };
    }
}