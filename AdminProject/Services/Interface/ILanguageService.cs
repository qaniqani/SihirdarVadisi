using System.Collections.Generic;
using System.Web.Mvc;
using Sihirdar.DataAccessLayer.Infrastructure.Models;

namespace AdminProject.Services.Interface
{
    public interface ILanguageService : IBaseInterface<Language>
    {
        Language GetItem(string url);
        Language GetItem(int id, string url);
        IList<Language> ActiveList();
        SelectList ActiveList(int selectedLanguage);
    }
}