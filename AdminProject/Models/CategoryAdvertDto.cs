using Sihirdar.DataAccessLayer;

namespace AdminProject.Models
{
    public class CategoryAdvertDto
    {
        public string Code { get; set; }
        public string FilePath { get; set; }
        public string FileType { get; set; }
        public string Guid { get; set; }
        public string Link { get; set; }
        public AdvertLocationTypes Location { get; set; }
        public AdvertTypes AdType { get; set; }
        public int SequenceNr { get; set; }
    }
}