namespace AdminProject.Models
{
    public class RuntimeSettings
    {
        public string Language { get; set; }
        public int LanguageId { get; set; }
        public int ImageMaxWidth { get; set; }
        public int ImageMaxHeight { get; set; }
        public string[] PictureExtensionTypes { get; set; }
        public string[] PictureMimeType { get; set; }
        public string[] FileExtensionTypes { get; set; }
        public string[] FileMimeTypes { get; set; }
        public string ContactAddress { get; set; }
        public string EmailAddress { get; set; }
        public string EmailPassword { get; set; }
        public int Port { get; set; }
        public string Smtp { get; set; }
        public string Domain { get; set; }
        public string[] UserExtensionTypes { get; set; }
        public string[] UserMimeTypes { get; set; }
    }
}