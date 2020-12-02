namespace AdminProject.Areas.Admin.Models
{
    public class PictureSizeItemDto
    {
        public int Id { get; set; }
        public int SizeId { get; set; }
        public string Name { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public bool Selected { get; set; }
    }
}