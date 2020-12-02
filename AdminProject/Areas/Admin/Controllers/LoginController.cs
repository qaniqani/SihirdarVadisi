﻿using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sihirdar.DataAccessLayer.Infrastructure;

namespace AdminProject.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        private readonly Func<AdminDbContext> _dbFactory;
        public LoginController(Func<AdminDbContext> dbFactory)
        {
            _dbFactory = dbFactory;
        }

        [HttpGet]
        public ActionResult Index()
        {
            if (Request.Cookies["RubyUser"] == null) 
                return View();

            var userId = Convert.ToInt32(Request.Cookies["RubyUser"].Value);

            var db = _dbFactory();
            var admin = db.Admins.FirstOrDefault(a => a.Id == userId && a.Status == Sihirdar.DataAccessLayer.StatusTypes.Active);
            if (admin == null)
                return View();

            Session["Admin"] = admin;

            admin.LastLoginDate = DateTime.Now;
            db.SaveChanges();

            return RedirectToAction("Index", "Default");
        }

        [HttpPost]
        public ActionResult Index(string Username, string Password, string rememberMe)
        {
            if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
            {
                TempData["Error"] = "Username and password is required.";
                return RedirectToAction("Index");
            }

            var db = _dbFactory();
            var admin = db.Admins.FirstOrDefault(a => a.Username == Username && a.Password == Password && a.Status == Sihirdar.DataAccessLayer.StatusTypes.Active);
            if(admin == null)
            {
                TempData["Error"] = "Username or password is incorrect.";
                return RedirectToAction("Index");
            }

            Session["Admin"] = admin;

            admin.LastLoginDate = DateTime.Now;
            db.SaveChanges();

            if(string.IsNullOrEmpty(rememberMe))
                return RedirectToAction("Index", "Default");

            var cookie = new HttpCookie("RubyUser", admin.Id.ToString())
            {
                Expires = DateTime.Now.AddYears(1)
            };

            Response.Cookies.Add(cookie);

            return RedirectToAction("Index", "Default");
        }

        [HttpGet]
        public ActionResult Logout()
        {
            Session.Remove("Admin");
            Session.Clear();
            Session.Abandon();

            if (Request.Cookies["RubyUser"] == null) 
                return RedirectToAction("Index");

            var cookie = Request.Cookies["RubyUser"];
            cookie.Expires = new DateTime(1970, 1, 1);
            Response.Cookies.Add(cookie);

            return RedirectToAction("Index");
        }
    }
}