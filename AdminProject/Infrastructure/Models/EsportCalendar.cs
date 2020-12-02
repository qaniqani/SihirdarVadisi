using System;
using AdminProject.Models;

namespace AdminProject.Infrastructure.Models
{
    public class EsportCalendar
    {
        public int Id { get; set; }
        public int LanguageId { get; set; }
        public string Language { get; set; }
        public string Name { get; set; }
        public string Picture { get; set; }
        public DateTime StartDateTime { get; set; }
        public string Color { get; set; }
        public string Description { get; set; }
        public StatusTypes Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedEditorId { get; set; }
        public DateTime UpdatedDate { get; set; } = new DateTime(1970, 1, 1);
        public int UpdatedEditorId { get; set; } = 0;
    }
}