using System;

namespace Sihirdar.DataAccessLayer.Infrastructure.Models
{
    public class Advert
    {
        public int Id { get; set; }
        public int CategoryId { get; set; } = 0;
        public string Language { get; set; }
        public int LanguageId { get; set; }
        public string Name { get; set; }
        public string AdGuid { get; set; }
        public AdvertLocationTypes AdLocation { get; set; }
        public AdvertTypes AdType { get; set; }
        public string AdCode { get; set; }
        public string AdLink { get; set; }
        public string AdFileType { get; set; }
        public string AdFilePath { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int ViewHit { get; set; } = 0;
        public int ClickHit { get; set; } = 0;
        public int SequenceNr { get; set; } = 9999;
        public StatusTypes Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int CreateEditorId { get; set; }
        public int UpdateEditorId { get; set; } = 0;
    }
}