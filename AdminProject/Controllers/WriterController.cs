using System.Web.Mvc;
using AdminProject.Models;
using AdminProject.Services.Interface;
using Sihirdar.DataAccessLayer;

namespace AdminProject.Controllers
{
    public class WriterController : Controller
    {
        private readonly IAdminService _adminService;

        public WriterController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [Route("yazar/{username}")]
        public ActionResult WriteDetail(string username, int page = 1)
        {
            if (string.IsNullOrEmpty(username))
                return Redirect("/");

            var writer = _adminService.GetWriterDetail(username);
            if (writer == null)
                return Redirect("/");

            TempData["CategoryUrl"] = "";
            var contents = _adminService.GetEditorContent(username, CategoryTypes.Story, page, 6);

            var result = new WriterDetailDto
            {
                Writer = writer,
                Contents = contents
            };

            ViewBag.Title = "Yazar " + writer.Name;

            return View(result);
        }
    }
}