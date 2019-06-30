using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;

namespace Finoaker.Web.Recaptcha
{
    public static partial class Helpers
    {
        internal static RecaptchaSettings GetOptions(ViewContext viewContext)
        {
            // Get the reCAPTCHA settings from IOptions service if not included in parameters.
            var services = viewContext.HttpContext.RequestServices;
            return ((IOptions<RecaptchaSettings>)services.GetService(typeof(IOptions<RecaptchaSettings>))).Value;
        }

        public const string RecaptchaVerifyUrl = "https://www.google.com/recaptcha/api/siteverify";
    }
}
