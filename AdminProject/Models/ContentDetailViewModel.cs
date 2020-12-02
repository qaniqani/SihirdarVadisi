namespace AdminProject.Models
{
    public class ContentDetailViewModel
    {
        public ContentViewModel Content { get; set; }
        public NavigateLink PrevContent { get; set; }
        public NavigateLink NextContent { get; set; }
        public EditorUser Editor { get; set; }
    }

    public class NavigateLink
    {
        public string Name { get; set; }
        public string Url { get; set; }
    }

    public class EditorUser
    {
        public string Username { get; set; }
        public string NameSurname { get; set; }
        public string Title { get; set; }
        public string Picture { get; set; }
        public string Motto { get; set; }
    }
}