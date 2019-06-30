using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using reCAPTCHA_AspNetCore_Sample.Models;
using Finoaker.Web.Recaptcha;

namespace reCAPTCHA_AspNetCore_Sample.Controllers
{
    public class HomeController : Controller
    {
        [Route("~/")]
        public IActionResult Index()
        {
            return RedirectToAction(nameof(RecaptchaV2CheckboxDemo));
        }

        [Route("~/V2Checkbox"), HttpGet]
        public IActionResult RecaptchaV2CheckboxDemo()
        {
            return View(Mvc.Views.RecaptchaV2CheckboxDemo);
        }

        [Route("~/V2Checkbox"), HttpPost]
        public IActionResult RecaptchaV2CheckboxDemo(ContactViewModel model, [FromServices] IRecaptchaService recaptchaService)
        {
            // check model state first to confirm all properties, including reCAPTCHA, are valid.
            if (ModelState.IsValid)
            {
                // verify the reCAPCTHA response against verification service
                var verifyResult = recaptchaService.VerifyAsync(model.RecaptchaResponse, RecaptchaType.V2Checkbox).Result;

                ViewBag.Result = "Failed!";

                // check success status
                if (verifyResult.Success)
                {
                    ViewBag.Result = "Success!";
                }
            }
            return View(Mvc.Views.RecaptchaV2CheckboxDemo);
        }

        [Route("~/V3"), HttpGet]
        public IActionResult RecaptchaV3Demo()
        {
            return View(Mvc.Views.RecaptchaV3Demo);
        }

        [Route("~/V3"), HttpPost]
        public IActionResult RecaptchaV3Demo(ContactViewModel model, [FromServices] IRecaptchaService recaptchaService)
        {
            // check model state first to confirm all properties, including reCAPTCHA, are valid.
            if (ModelState.IsValid)
            {
                var myMinimumScore = 0.8m;

                // verify the reCAPCTHA response against verification service
                var verifyResult = recaptchaService.VerifyAsync(model.RecaptchaResponse, RecaptchaType.V3).Result;

                ViewBag.Result = "Failed!";

                // check success status
                if (verifyResult.Success && verifyResult.Score > myMinimumScore)
                {
                    ViewBag.Result = "Success!";
                }
            }
            return View(Mvc.Views.RecaptchaV3Demo);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
