using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Anket.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Anket.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;

namespace Anket.Controllers
{
    public class HomeController : Controller
    {
        private readonly TestContext _context;
        public HomeController(TestContext context) => _context=context;
        public void SetSession(string Username, string Password)
        {
            HttpContext.Session.SetString("UserName", Username);
            HttpContext.Session.SetString("Password", Password);
        }

        public IActionResult Index()
        {
            var userName = HttpContext.Session.GetString("UserName");

            if (userName==null)
            {
                return RedirectToAction(nameof(Login));
            }

            //var list = (from a in _context.Records select a).ToList();

            ViewBag.errorList = TempData["errorList"];

            if (ViewBag.errorList != null)
            {
                List<string> errorList = new List<string>();
                foreach (var item in ViewBag.errorList)
                {
                    errorList.Add(item);
                }
                ViewBag.errorList = errorList;
            }
            else
            {
                ViewBag.errorList = null;
                ViewBag.Successful = Convert.ToBoolean(TempData["Successful"]);
            }
            return View(/*list*/);
        }

        [HttpPost]
        public IActionResult Create(
            [Bind(
            "FirstName", "LastName", "PhoneNumber", "CustomerCompany"
            )]
            Records records)
        {
            List<string> errorList = new List<string>();
            if (ModelState.IsValid)
            {
                    records.RecordedUser = HttpContext.Session.GetString("UserName");
                    _context.Add(records);
                    //_context.Database.ExecuteSqlRaw("exec dbo.sil");
                    _context.SaveChanges();
                    TempData["Successful"] = Convert.ToBoolean(true);
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                errorList.AddRange(ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList());
                TempData["Successful"] = Convert.ToBoolean(false);
            }
            TempData["errorList"] = errorList.ToList();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Login()
        {
            var userName = HttpContext.Session.GetString("UserName");

            if (userName != null)
            {
                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        [HttpPost]
        public IActionResult Login([Bind(
            "Username", "Password"
            )] string userName, string Password)
        {
            UserHelper userHelper = new UserHelper();

            string WindowsUsername = HttpContext.User.Identity.Name;

            bool user = userHelper.GetUser(userName,Password);

            if (user==false)
            {
                return BadRequest("Giriş Başarısız!");
            }

            SetSession(userName, Password);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();
            return RedirectToAction(nameof(Login));
        }
    }
}
