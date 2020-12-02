using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using AdminProject.Models;
using Sihirdar.DataAccessLayer;

namespace AdminProject.Helpers
{
    public class DropdownTypes
    {
        public static SelectList GetStatus(StatusTypes selectedValue)
        {
            var list = new List<ListItem>
            {
                new ListItem {Text = "Active", Value = StatusTypes.Active.ToInt32().ToString()},
                new ListItem {Text = "Deactive", Value = StatusTypes.Deactive.ToInt32().ToString()}
            };

            return new SelectList(list, "Value", "Text", selectedValue.ToInt32().ToString());
        }

        public static SelectList GetCategoryType(CategoryTypes selectedValue)
        {
            var list = new List<ListItem>
            {
                new ListItem {Text = "Story", Value = CategoryTypes.Story.ToInt32().ToString()},
                new ListItem {Text = "Download Guide", Value = CategoryTypes.DownloadGuide.ToInt32().ToString()},
                new ListItem {Text = "Video Gallery", Value = CategoryTypes.Video.ToInt32().ToString()},
                new ListItem {Text = "Photo Gallery", Value = CategoryTypes.Gallery.ToInt32().ToString()},
                new ListItem {Text = "LOL Team Page", Value = CategoryTypes.TeamPage.ToInt32().ToString()},
                new ListItem {Text = "Hour Play LOL?", Value = CategoryTypes.ToolLOLHour.ToInt32().ToString()},
                new ListItem {Text = "Your Champ. LOL", Value = CategoryTypes.ToolLOLChamp.ToInt32().ToString()},
                new ListItem {Text = "E-Sport Calender", Value = CategoryTypes.ESportCalendar.ToInt32().ToString()},
                new ListItem {Text = "Static Category", Value = CategoryTypes.Static.ToInt32().ToString()}
            };

            return new SelectList(list, "Value", "Text", selectedValue.ToInt32().ToString());
        }

        public static SelectList GetContentType(ContentTypes selectedValue)
        {
            var list = new List<ListItem>
            {
                new ListItem {Text = "Content", Value = ContentTypes.Content.ToInt32().ToString()},
                new ListItem {Text = "Video Detail", Value = ContentTypes.Video.ToInt32().ToString()},
                new ListItem {Text = "Gallery Detail", Value = ContentTypes.Gallery.ToInt32().ToString()}
            };

            return new SelectList(list, "Value", "Text", selectedValue.ToInt32().ToString());
        }

        public static SelectList GetVideoType(VideoTypes selectedValue)
        {
            var list = new List<ListItem>
            {
                new ListItem {Text = "Not Video", Value = VideoTypes.IsNotVideo.ToInt32().ToString()},
                new ListItem {Text = "Video", Value = VideoTypes.IsVideo.ToInt32().ToString()},
                new ListItem {Text = "Video Embed Code", Value = VideoTypes.IsEmbedCode.ToInt32().ToString()}
            };

            return new SelectList(list, "Value", "Text", selectedValue.ToInt32().ToString());
        }

        public static SelectList GetPictureType(PictureTypes selectedValue)
        {
            var list = new List<ListItem>
            {
                new ListItem {Text = "Slider", Value = PictureTypes.Slider.ToInt32().ToString()},
                //new ListItem {Text = "Showcase", Value = PictureTypes.Showcase.ToInt32().ToString()},
                //new ListItem {Text = "Content Slider", Value = PictureTypes.ContentSlider.ToInt32().ToString()},
                new ListItem {Text = "LightBox", Value = PictureTypes.LightBox.ToInt32().ToString()}
            };

            return new SelectList(list, "Value", "Text", selectedValue.ToInt32().ToString());
        }

        public static SelectList GetContentPicture(ContentPictureTypes selectedValue)
        {
            var list = new List<ListItem>
            {
                new ListItem {Text = "Content", Value = ContentPictureTypes.Content.ToInt32().ToString()},
                new ListItem {Text = "Video", Value = ContentPictureTypes.Video.ToInt32().ToString()}
            };

            return new SelectList(list, "Value", "Text", selectedValue.ToInt32().ToString());
        }

        public static SelectList TakeCount(int selectedValue)
        {
            var list = new List<ListItem>
            {
                new ListItem { Text = "10", Value= "10" },
                new ListItem { Text = "20", Value= "20" },
                new ListItem { Text = "30", Value= "30" },
                new ListItem { Text = "50", Value= "50" },
                new ListItem { Text = "100", Value= "100" }
            };

            return new SelectList(list, "Value", "Text", selectedValue);
        }

        public static SelectList GetUserType(UserStatusTypes selectedValue)
        {
            var list = new List<ListItem>
            {
                new ListItem {Text = "Aktif", Value = UserStatusTypes.Active.ToInt32().ToString()},
                new ListItem {Text = "Pasif", Value = UserStatusTypes.Deactive.ToInt32().ToString()},
                new ListItem {Text = "Silinen", Value = UserStatusTypes.Deleted.ToInt32().ToString()},
                new ListItem {Text = "Yasaklanan", Value = UserStatusTypes.Banned.ToInt32().ToString()}
            };

            return new SelectList(list, "Value", "Text", selectedValue.ToInt32().ToString());
        }

        public static SelectList GetGenderType(GenderTypes selectedValue)
        {
            var list = new List<ListItem>
            {
                new ListItem {Text = "Erkek", Value = GenderTypes.Man.ToInt32().ToString()},
                new ListItem {Text = "Kadın", Value = GenderTypes.Woman.ToInt32().ToString()}
            };

            return new SelectList(list, "Value", "Text", selectedValue.ToInt32().ToString());
        }

        public static SelectList GetQuestionType(QuestionTypes selectedValue)
        {
            var list = new List<ListItem>
            {
                new ListItem {Text = "Tekli", Value = QuestionTypes.Single.ToInt32().ToString()}
                //new ListItem {Text = "Çoklu", Value = QuestionTypes.MultiOption.ToInt32().ToString()}
            };

            return new SelectList(list, "Value", "Text", selectedValue.ToInt32().ToString());
        }

        public static SelectList GetAdvertLocationType(AdvertLocationTypes selectedValue)
        {
            var list = new List<ListItem>
            {
                new ListItem {Text = "Header (Must Head)", Value = AdvertLocationTypes.Header.ToInt32().ToString()},
                new ListItem {Text = "Right Block", Value = AdvertLocationTypes.RightBlock.ToInt32().ToString()},
                new ListItem {Text = "Home Page (300x450)", Value = AdvertLocationTypes.HomePage300X450.ToInt32().ToString()},
                new ListItem {Text = "Home Page (728x90)", Value = AdvertLocationTypes.HomePage728X90.ToInt32().ToString()},
                new ListItem {Text = "Live (300x450)", Value = AdvertLocationTypes.Live728X90.ToInt32().ToString()},
                new ListItem {Text = "Live Embedded", Value = AdvertLocationTypes.LiveEmbedded.ToInt32().ToString()},
                new ListItem {Text = "Video Embedded", Value = AdvertLocationTypes.Embedded.ToInt32().ToString()},
                new ListItem {Text = "Site Center", Value = AdvertLocationTypes.SiteMiddle.ToInt32().ToString()},
                new ListItem {Text = "Content Detail", Value = AdvertLocationTypes.Content.ToInt32().ToString()}
            };

            return new SelectList(list, "Value", "Text", selectedValue.ToInt32().ToString());
        }

        public static SelectList GetAdvertType(AdvertTypes selectedValue)
        {
            var list = new List<ListItem>
            {
                new ListItem {Text = "Embedded", Value = AdvertTypes.Embedded.ToInt32().ToString()},
                new ListItem {Text = "Adsense", Value = AdvertTypes.Adsense.ToInt32().ToString()},
                new ListItem {Text = "Resim", Value = AdvertTypes.Image.ToInt32().ToString()},
                new ListItem {Text = "AdMatic", Value = AdvertTypes.AdMatic.ToInt32().ToString()}
            };

            return new SelectList(list, "Value", "Text", selectedValue.ToInt32().ToString());
        }

        public static SelectList GetGameType(string selectedValue)
        {
            var list = new List<ListItem>
            {
                new ListItem {Text = "LOL", Value = "lol-color"},
                new ListItem {Text = "CS Go", Value = "csgo-color"},
                new ListItem {Text = "Dota2", Value = "dota2-color"},
                new ListItem {Text = "PubG", Value = "pubg-color"},
                new ListItem {Text = "Hearthstone", Value = "heartstone-color"},
                new ListItem {Text = "Arena of Valor", Value = "strikeofkings-color"},
                new ListItem {Text = "Overwatch", Value = "overwatch-color"}
            };

            return new SelectList(list, "Value", "Text", selectedValue);
        }

        public static SelectList GetGameType(GameTypes selectedValue)
        {
            var list = new List<ListItem>
            {
                new ListItem {Text = "All", Value = GameTypes.All.ToInt32().ToString()},
                new ListItem {Text = "League of Legends", Value = GameTypes.LOL.ToInt32().ToString()},
                new ListItem {Text = "CS:Go", Value = GameTypes.CsGo.ToInt32().ToString()},
                new ListItem {Text = "DOTA 2", Value = GameTypes.Dota2.ToInt32().ToString()},
                new ListItem {Text = "Hearthstone", Value = GameTypes.Heartstone.ToInt32().ToString()},
                new ListItem {Text = "Overwatch", Value = GameTypes.Overwatch.ToInt32().ToString()},
                new ListItem {Text = "PubG", Value = GameTypes.PubG.ToInt32().ToString()},
                new ListItem {Text = "Arena of Valor", Value = GameTypes.StrikeOfKings.ToInt32().ToString()},
                new ListItem {Text = "Other News", Value = GameTypes.OtherNews.ToInt32().ToString()}
            };

            return new SelectList(list, "Value", "Text", selectedValue.ToInt32().ToString());
        }

        public static SelectList GetContentFilterType(ContentFilterTypes selectedValue)
        {
            var list = new List<ListItem>
            {
                new ListItem {Text = "All", Value = ContentFilterTypes.All.ToInt32().ToString()},
                new ListItem {Text = "Top Rated", Value = ContentFilterTypes.TopRated.ToInt32().ToString()},
                new ListItem {Text = "We Chose", Value = ContentFilterTypes.WeChose.ToInt32().ToString()},
                new ListItem {Text = "Homepage Static (Video)", Value = ContentFilterTypes.HomepageStatic.ToInt32().ToString()}
            };

            return new SelectList(list, "Value", "Text", selectedValue.ToInt32().ToString());
        }

        public static SelectList GetAdminType(AdminTypes selectedValue)
        {
            var list = new List<ListItem>
            {
                new ListItem {Text = "Admin", Value = AdminTypes.Admin.ToInt32().ToString()},
                new ListItem {Text = "Editor", Value = AdminTypes.Editor.ToInt32().ToString()},
                new ListItem {Text = "Writer", Value = AdminTypes.Writer.ToInt32().ToString()}
            };

            return new SelectList(list, "Value", "Text", selectedValue.ToInt32().ToString());
        }

        public static SelectList GetUserGameType(GameTypes selectedValue)
        {
            var list = new List<ListItem>
            {
                new ListItem {Text = Resources.Lang.typeAll, Value = GameTypes.All.ToInt32().ToString()},
                new ListItem {Text = "League of Legend", Value = GameTypes.LOL.ToInt32().ToString()},
                new ListItem {Text = "CS:GO", Value = GameTypes.CsGo.ToInt32().ToString()},
                new ListItem {Text = "DOTA 2", Value = GameTypes.Dota2.ToInt32().ToString()},
                new ListItem {Text = "Hearthstone", Value = GameTypes.Heartstone.ToInt32().ToString()},
                new ListItem {Text = "Overwatch", Value = GameTypes.Overwatch.ToInt32().ToString()},
                new ListItem {Text = "PubG", Value = GameTypes.PubG.ToInt32().ToString()},
                new ListItem {Text = "Arena of Valor", Value = GameTypes.StrikeOfKings.ToInt32().ToString()},
                new ListItem {Text = "Diğer", Value = GameTypes.OtherNews.ToInt32().ToString()}
            };

            return new SelectList(list, "Value", "Text", selectedValue.ToInt32().ToString());
        }

        public static SelectList GetUserContentFilterType(ContentFilterTypes selectedValue)
        {
            var list = new List<ListItem>
            {
                new ListItem {Text = Resources.Lang.typeAll, Value = ContentFilterTypes.All.ToInt32().ToString()},
                new ListItem {Text = Resources.Lang.contentFilterTypeTopRated, Value = ContentFilterTypes.TopRated.ToInt32().ToString()},
                new ListItem {Text = Resources.Lang.contentFilterTypeWeChose, Value = ContentFilterTypes.WeChose.ToInt32().ToString()}
            };

            return new SelectList(list, "Value", "Text", selectedValue.ToInt32().ToString());
        }

        public static SelectList GetCategoryTagType(CategoryTagTypes selectedValue)
        {
            var list = new List<ListItem>
            {
                new ListItem {Text = "Normal", Value = CategoryTagTypes.Normal.ToInt32().ToString()},
                new ListItem {Text = "New", Value = CategoryTagTypes.New.ToInt32().ToString()},
                new ListItem {Text = "Coming Soon", Value = CategoryTagTypes.Coming.ToInt32().ToString()}
            };

            return new SelectList(list, "Value", "Text", selectedValue.ToInt32().ToString());
        }

        public static SelectList GetSubjectType(SubjectTypes selectedValue)
        {
            var list = new List<ListItem>
            {
                new ListItem {Text = "Konu Seçiniz", Value = SubjectTypes.NotSelected.ToInt32().ToString()},
                new ListItem {Text = "Görüş ve Öneriler", Value = SubjectTypes.Opinions.ToInt32().ToString()},
                new ListItem {Text = "Reklam", Value = SubjectTypes.Adverts.ToInt32().ToString()},
                new ListItem {Text = "Hata", Value = SubjectTypes.Errors.ToInt32().ToString()},
                new ListItem {Text = "Diğer", Value = SubjectTypes.Other.ToInt32().ToString()}
            };

            return new SelectList(list, "Value", "Text", selectedValue.ToInt32().ToString());
        }
    }
}