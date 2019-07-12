using Microsoft.AspNetCore.Mvc;
using Finoaker.Web.Recaptcha;
using Microsoft.Extensions.Options;

namespace SampleCode
{
    public class SampleV3Controller : Controller
    {
        private readonly RecaptchaSettings _settings;
        public SampleV3Controller(IOptions<RecaptchaSettings> settings)
        {
            _settings = settings.Value;
        }

        [HttpGet]
        public IActionResult RecaptchaV3Demo()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult RecaptchaV3Demo(SampleModel model)
        {
            var myMinimumScore = 0.7m;

            // check model state first to confirm all properties, including reCAPTCHA, are valid.
            if (ModelState.IsValid)
            {
                // verify the reCAPCTHA response against verification service
                var verifyResult = RecaptchaService.VerifyTokenAsync(model.RecaptchaResponse, RecaptchaType.V3, _settings).Result;

                // check success status and score exceeds you minimum.
                if (verifyResult.Success && verifyResult.Score > myMinimumScore)
                {
                    // Success
                    //...
                } else
                {
                    // Fail
                    //...
                }
            }
            return View();
        }
    }
}
