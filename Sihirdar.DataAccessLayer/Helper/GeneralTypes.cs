namespace Sihirdar.DataAccessLayer
{
    public enum AdminTypes
    {
        Admin = 0,
        Editor = 1,
        Writer = 2
    }

    public enum StatusTypes
    {
        Freeze = 2,
        Active = 1,
        Deactive = 0
    }

    public enum CategoryTypes
    {
        ESportCalendar = 8,
        Gallery = 7,
        Video = 6,
        Story = 5,
        Static = 4,
        ToolLOLHour = 3,
        ToolLOLChamp = 2,
        TeamPage = 1,
        DownloadGuide = 0,
        All = -1
    }

    public enum CategoryTagTypes
    {
        Normal = 0,
        New = 1,
        Coming = 2
    }

    public enum ContentTypes
    {
        Content = 0,
        Gallery = 1,
        Video = 2
    }

    public enum GameTypes
    {
        All = 0,
        LOL = 1,
        CsGo = 2,
        Dota2 = 3,
        Heartstone = 4,
        Overwatch = 5,
        OtherNews = 6,
        StrikeOfKings = 7,
        PubG = 8
    }

    public enum YoutubePictureTypes
    {
         Default,
         Medium,
         High
    }

    public enum ContentFilterTypes
    {
        All = 0,
        TopRated = 1,
        WeChose = 2,
        HomepageStatic = 3
    }

    public enum PictureTypes
    {
        Slider = 0,
        LightBox = 1,
        Showcase = 2,
        ContentSlider = 3
    }

    public enum VideoTypes
    {
        IsNotVideo = 0,
        IsVideo = 1,
        IsEmbedCode = 2
    }

    public enum UserStatusTypes
    {
        Deactive = 0,
        Active = 1,
        Unapproved = 2,
        Banned = 3,
        Deleted = 4,
        OldUser = 5
    }

    public enum DrawApiStatusTypes
    {
        Deactive = 0,
        Active = 1,
        Unapproved = 2,
        Banned = 3,
        Deleted = 4,
        OldApi = 5,
        Pending = 6
    }

    public enum GenderTypes
    {
        Woman = 0,
        Man = 1
    }

    public enum SignInTypes
    {
        Site = 0,
        Facebook = 1,
        Twitter = 2,
        GPlus = 3
    }

    public enum AdvertTypes
    {
        Adsense = 0,
        AdMatic = 1,
        Image = 2,
        Embedded = 3
    }

    public enum AdvertLocationTypes
    {
        Header = 1,
        HomePage300X450 = 2,
        HomePage728X90 = 3,
        RightBlock = 4,
        SiteMiddle = 5,
        Live728X90 = 6,
        LiveEmbedded = 7,
        Embedded = 8,
        Content = 9
    }

    public enum ContentPictureTypes
    {
        Content = 0,
        Video = 1
    }

    public enum UserActivationTypes
    {
        ActivationCodeNotFound,
        ActivationSuccess
    }

    public enum QuestionTypes
    {
        Single = 1
        //MultiOption = 2
    }
}