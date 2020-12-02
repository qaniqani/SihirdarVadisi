using System;

namespace AdminProject.Models
{
    public class TournamentExcelDto
    {
        //[ExcelExport("Oyun Adı", order = 1)]
        public string GameName { get; set; }

        //[ExcelExport("Kaptanın Adı", order = 2)]
        public string Username1 { get; set; }

        //[ExcelExport("1. Oyuncu", order = 3)]
        public string Username2 { get; set; }

        //[ExcelExport("2. Oyuncu", order = 4)]
        public string Username3 { get; set; }

        //[ExcelExport("3. Oyuncu", order = 5)]
        public string Username4 { get; set; }

        //[ExcelExport("4. Oyuncu", order = 6)]
        public string Username5 { get; set; }

        //[ExcelExport("Kaptanın Nicki", order = 7)]
        public string UserNick1 { get; set; }

        //[ExcelExport("1. Oyuncunun Nicki", order = 8)]
        public string UserNick2 { get; set; }

        //[ExcelExport("2. Oyuncunun Nicki", order = 9)]
        public string UserNick3 { get; set; }

        //[ExcelExport("3. Oyuncunun Nicki", order = 10)]
        public string UserNick4 { get; set; }

        //[ExcelExport("4. Oyuncunun Nicki", order = 11)]
        public string UserNick5 { get; set; }

        //[ExcelExport("Takım Adı", order = 12)]
        public string TeamName { get; set; }

        //[ExcelExport("1. Yedek Adı", order = 13)]
        public string BackupUsername1 { get; set; }

        //[ExcelExport("2. Yedek Adı", order = 14)]
        public string BackupUsername2 { get; set; }

        //[ExcelExport("1. Yedek Nick", order = 15)]
        public string BackupUserNick1 { get; set; }

        //[ExcelExport("2. Yedek Nick", order = 16)]
        public string BackupUserNick2 { get; set; }

        //[ExcelExport("Telefon", order = 17)]
        public string Phone { get; set; }

        //[ExcelExport("Oluş. Tar.", order = 18)]
        public DateTime CreteDate { get; set; }
    }
}