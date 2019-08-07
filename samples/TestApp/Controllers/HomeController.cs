using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Finoaker.Web.Recaptcha.TestApp.Models;

namespace Finoaker.Web.Recaptcha.TestApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View("V3FormTagHelper");
        }

        public IActionResult V3TagHelper()
        {
            return View();
        }

        public IActionResult V3HtmlHelper()
        {
            return View();
        }

        public IActionResult V2CheckboxTagHelper()
        {
            return View();
        }

        public IActionResult V2CheckboxHtmlHelper()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
