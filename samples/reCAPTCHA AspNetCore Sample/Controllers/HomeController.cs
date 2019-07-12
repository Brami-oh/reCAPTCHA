using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using reCAPTCHA_AspNetCore_Sample.Models;
using Finoaker.Web.Recaptcha;
using Microsoft.Extensions.Options;

namespace reCAPTCHA_AspNetCore_Sample.Controllers
{
    public class HomeController : Controller
    {
        private readonly RecaptchaSettings _settings;
        public HomeController(IOptions<RecaptchaSettings> settings)
        {
            _settings = settings.Value;
        }

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

        [Route("~/V2Checkbox"), HttpPost, ValidateAntiForgeryToken]
        public IActionResult RecaptchaV2CheckboxDemo(ContactViewModel model)
        {
            // check model state first to confirm all properties, including reCAPTCHA, are valid.
            if (ModelState.IsValid)
            {
                // verify the reCAPCTHA response against verification service
                var verifyResult = RecaptchaService.VerifyTokenAsync(model.RecaptchaResponse, RecaptchaType.V2Checkbox, _settings).Result;
                ViewData["VerifyResponse"] = verifyResult;
                ViewData["Name"] = model.Name;

                // check success status
                if (verifyResult.Success)
                {
                    // Success...
                }
                else
                {
                    // Failed...
                }
            }
            return View(Mvc.Views.RecaptchaV2CheckboxDemo);
        }

        [Route("~/V3"), HttpGet]
        public IActionResult RecaptchaV3Demo()
        {
            return View(Mvc.Views.RecaptchaV3Demo);
        }

        [Route("~/V3"), HttpPost, ValidateAntiForgeryToken]
        public IActionResult RecaptchaV3Demo(ContactViewModel model)
        {
            // check model state first to confirm all properties, including reCAPTCHA, are valid.
            if (ModelState.IsValid)
            {
                var myMinimumScore = 0.8m;

                // verify the reCAPCTHA response against verification service
                var verifyResult = RecaptchaService.VerifyTokenAsync(model.RecaptchaResponse, RecaptchaType.V3, _settings).Result;
                ViewData["VerifyResponse"] = verifyResult;
                ViewData["Name"] = model.Name;

                // check success status
                if (verifyResult.Success && verifyResult.Score > myMinimumScore)
                {
                    // Success AND scored above minimum
                } else
                {
                    // Failed OR scored less than minimum.
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
