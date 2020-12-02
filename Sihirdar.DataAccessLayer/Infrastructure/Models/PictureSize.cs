namespace Sihirdar.DataAccessLayer.Infrastructure.Models
{
    public class PictureSize
    {
        public int Id { get; set; }
        public ContentTypes PictureType { get; set; }
        public string Name { get; set; }
        public StatusTypes Status { get; set; }
    }
}