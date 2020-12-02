namespace Sihirdar.DataAccessLayer.Infrastructure.Models
{
    public class Picture
    {
        public int Id { get; set; }
        public ContentPictureTypes PictureType { get; set; }
        public int ContentId { get; set; }
        public int SizeId { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string PicturePath { get; set; }
    }
}