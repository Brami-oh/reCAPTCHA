using Microsoft.AspNetCore.Mvc;
using Finoaker.Web.Recaptcha;
using Microsoft.Extensions.Options;

namespace SampleCode
{
    public class SampleV2Controller : Controller
    {
        private readonly RecaptchaSettings _settings;
        public SampleV2Controller(IOptions<RecaptchaSettings> settings)
        {
            _settings = settings.Value;
        }

        [HttpGet]
        public IActionResult RecaptchaV2CheckboxDemo()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult RecaptchaV2CheckboxDemo(SampleModel model)
        {
            // check model state first to confirm all properties, including reCAPTCHA, are valid.
            if (ModelState.IsValid)
            {
                // verify the reCAPCTHA response against verification service
                var verifyResult = RecaptchaService.VerifyTokenAsync(model.RecaptchaResponse, RecaptchaType.V2Checkbox, _settings).Result;

                // check success status and score exceeds you minimum.
                if (verifyResult.Success)
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
