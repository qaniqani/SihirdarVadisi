using AdminProject.Models;
using AdminProject.Services.Models;
using PagedList;
using Sihirdar.DataAccessLayer;

namespace AdminProject.Services.Interface
{
    public interface IAdminService
    {
        WriterViewModel GetWriterDetail(string userName);
        StaticPagedList<CategoryContentItemViewModel> GetEditorContent(string writerUsername, CategoryTypes categoryType, int skip, int take);
    }
}