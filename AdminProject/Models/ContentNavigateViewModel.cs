namespace AdminProject.Models
{
    public class ContentNavigateViewModel
    {
        public CategoryVideoItemViewModel FirstVideo { get; set; }
        public NavigateVideoLink NextVideo { get; set; }
        public NavigateVideoLink PrevVideo { get; set; }
    }

    public class NavigateVideoLink
    {
        public string Name { get; set; }
        public string Picture { get; set; }
        public string Url { get; set; }
    }
}