using AdminProject.Services.Models;
using PagedList;

namespace AdminProject.Models
{
    public class WriterDetailDto
    {
        public WriterViewModel Writer { get; set; }
        public StaticPagedList<CategoryContentItemViewModel> Contents { get; set; }
    }
}