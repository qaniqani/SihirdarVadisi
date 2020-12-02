using System;

namespace Sihirdar.DataAccessLayer.Infrastructure.Models
{
    public class PromiseDay
    {
        public int Id { get; set; }
        public string Language { get; set; }
        public int LanguageId { get; set; }
        public string Promise { get; set; }
        public string Teller { get; set; }
        public DateTime PublishDate { get; set; }
        public StatusTypes Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdateDate { get; set; } = new DateTime(1970, 1, 1);
        public int CreateEditorId { get; set; }
        public int UpdateEditorId { get; set; } = 0;
    }
}