using System;
using System.Linq;
using AdminProject.Helpers;
using AdminProject.Models;
using AdminProject.Services.Interface;
using AdminProject.Services.Models;
using PagedList;
using Sihirdar.DataAccessLayer.Infrastructure;
using Sihirdar.DataAccessLayer;

namespace AdminProject.Services
{
    public class AdminService : IAdminService
    {
        private readonly Func<AdminDbContext> _dbFactory;

        public AdminService(Func<AdminDbContext> dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public WriterViewModel GetWriterDetail(string userName)
        {
            var db = _dbFactory();
            var writer = db.Admins.FirstOrDefault(a => a.Username == userName);
            if (writer == null)
                return null;

            var result = new WriterViewModel
            {
                AdminType = writer.AdminType,
                Facebook = writer.Facebook,
                Id = writer.Id,
                Motto = writer.Motto,
                Name = writer.Name,
                Picture = writer.Picture,
                Twitter = writer.Twitter,
                Username = writer.Username
            };

            return result;
        }

        public StaticPagedList<CategoryContentItemViewModel> GetEditorContent(string writerUsername, CategoryTypes categoryType, int skip, int take)
        {
            var db = _dbFactory();

            var contents = (from content in db.Contents
                            join cat in db.Categories
                            on content.CategoryId equals cat.Id
                            join editor in db.Admins
                            on content.CreateUser equals editor.Id
                            orderby content.Id descending
                            where content.Status == StatusTypes.Active
                                  && cat.Status == StatusTypes.Active
                                  && cat.CategoryType == categoryType
                                  && editor.Username == writerUsername
                            select new
                            {
                                content.Id,
                                content.CreateDate,
                                Subject = content.Name,
                                content.GameType,
                                EditorName = editor.Name,
                                Description = content.Detail,
                                content.Url,
                                content.Picture,
                                CategoryUrl = cat.Url
                            });

            var records = contents.ToPagedList(skip, take);
            var contentItems = records.Select(a =>
            {
                var detail = Utility.StripHtml(a.Description);
                detail = detail.Length > 160 ? detail.Substring(0, 160) + "..." : detail;
                var item = new CategoryContentItemViewModel
                {
                    Id = a.Id,
                    Url = $"{a.CategoryUrl}/{a.Url}",
                    Subject = a.Subject,
                    Description = detail,
                    PictureName = a.Url,
                    CreateDate = a.CreateDate,
                    GameType = Tool.GetGameTypeText[a.GameType],
                    EditorName = a.EditorName
                };
                return item;
            }).ToList();

            var result = new StaticPagedList<CategoryContentItemViewModel>(contentItems, records);

            return result;
        }
    }
}