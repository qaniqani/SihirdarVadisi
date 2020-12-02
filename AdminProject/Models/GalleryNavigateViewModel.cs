namespace AdminProject.Models
{
    public class GalleryNavigateViewModel
    {
        public CategoryGalleryItemViewModel FirstGallery { get; set; }
        public NavigateGalleryLink NextGallery { get; set; }
        public NavigateGalleryLink PrevGallery { get; set; }
    }

    public class NavigateGalleryLink
    {
        public string Name { get; set; }
        public string Picture { get; set; }
        public string Url { get; set; }
    }
}