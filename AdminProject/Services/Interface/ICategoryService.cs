using System.Collections.Generic;
using System.Web.Mvc;
using AdminProject.Models;
using AdminProject.Services.Models;
using Sihirdar.DataAccessLayer;
using Sihirdar.DataAccessLayer.Infrastructure.Models;

namespace AdminProject.Services.Interface
{
    public interface ICategoryService : IBaseInterface<Category>
    {
        string GetChainCategory(int id);
        SelectList GetStatusType(StatusTypes status);
        SelectList GetCategoryType(CategoryTypes status);
        List<MenuItem> GetCategories(CategoryTypes categoryType, StatusTypes status);
        List<CategoryListDto> ListCategory();
        void OrderCategory(List<SortedMenu> order);
        Category GetCategory(string url);
        List<Category> ActiveCategoryList(string language);
        List<Category> GetCategoryParentList(int parentId);
        CategoryLinkDto GetChainCategoryLink(int id, string split);
        SelectList GetCategorySelectList(int parentId, int selectedValue);
        List<CategoryLinkDto> GetChainCategoryLink(int[] id, string split);
        int[] GetCategoryParentIds(int categoryId);
        List<Category> GetCategory(int[] ids);
    }
}