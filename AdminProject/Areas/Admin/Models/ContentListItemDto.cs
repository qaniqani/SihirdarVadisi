using System;
using AdminProject.Models;
using Sihirdar.DataAccessLayer;

namespace AdminProject.Areas.Admin.Models
{
    public class ContentListItemDto
    {
        public int Id { get; set; }
        public string Categories { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string CreateEditor { get; set; }
        public string UpdateEditor { get; set; }
        public int UpdateEditorId { get; set; }
        public DateTime PublishDate { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public StatusTypes Status { get; set; }
    }
}