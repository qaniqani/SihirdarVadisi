namespace Sihirdar.DataAccessLayer.Infrastructure.Models
{
    public class PictureSizeDetail
    {
        public int Id { get; set; }
        public int SizeId { get; set; }
        public string Name { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public StatusTypes Status { get; set; } = StatusTypes.Active;
    }
}