using System;
using System.Collections.Generic;
using System.Drawing;
using System.Web;
using AdminProject.Models;
using AdminProject.Resources;
using System.IO;
using Sihirdar.DataAccessLayer;

namespace AdminProject.Helpers
{
    public class Tool
    {
        public static UserResultDto UserCheck()
        {
            try
            {
                if (HttpContext.Current.Session["User"] != null)
                    return HttpContext.Current.Session["User"] as UserResultDto;

                //var admin = new User
                //{
                //    Id = 1,
                //    LastLoginDate = DateTime.Now,
                //    Name = "Kamil",
                //    Password = "1234",
                //    Status = UserTypes.Active
                //};

                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static void SaveImage(string saveName, string base64)
        {
            using (var ms = new MemoryStream(Convert.FromBase64String(base64)))
            {
                using (var bm2 = new Bitmap(ms))
                {
                    bm2.Save(HttpContext.Current.Server.MapPath($"~/Content/{saveName}"));
                }
            }
        }

        public static void SaveUserImage(string saveName, string base64)
        {
            //saveName = Utility.UrlSeo(saveName) + ".jpg";
            using (var ms = new MemoryStream(Convert.FromBase64String(base64)))
            {
                using (var bm2 = new Bitmap(ms))
                {
                    bm2.Save(HttpContext.Current.Server.MapPath($"~/Content/User/{saveName}"));
                }
            }
        }

        public static readonly Dictionary<GameTypes, string> GetGameTypeText = new Dictionary<GameTypes, string>
        {
            {GameTypes.All, Lang.typeAll},
            {GameTypes.LOL, "League of Legends"},
            {GameTypes.CsGo, "CS:Go"},
            {GameTypes.Dota2, "DOTA 2"},
            {GameTypes.Overwatch, "Overwatch"},
            {GameTypes.PubG, "PubG"},
            {GameTypes.Heartstone, "Hearthstone"},
            {GameTypes.StrikeOfKings, "Arena of Valor"},
            {GameTypes.OtherNews, "Diğer Haberler"}
        };

        public static readonly Dictionary<AdminTypes, string> GetEditorTypeText = new Dictionary<AdminTypes, string>
        {
            {AdminTypes.Admin, Lang.adminTypeAdmin},
            {AdminTypes.Editor, Lang.adminTypeEditor},
            {AdminTypes.Writer, Lang.adminTypeWriter}
        };

        public static readonly Dictionary<GameTypes, string> GetGameTypeColorCode = new Dictionary<GameTypes, string>
        {
            {GameTypes.All, "bordered-white"},
            {GameTypes.LOL, "bordered-pink"},
            {GameTypes.CsGo, "bordered-yellow"},
            {GameTypes.Dota2, "bordered-orange"},
            {GameTypes.Overwatch, "bordered-light-yellow"},
            {GameTypes.StrikeOfKings, "bordered-light-yellow2"},
            {GameTypes.Heartstone, "bordered-blue"},
            {GameTypes.PubG, "bordered-blue"},
            {GameTypes.OtherNews, "bordered-green"}
        };
    }
}