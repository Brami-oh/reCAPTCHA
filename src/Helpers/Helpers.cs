using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Finoaker.Web.Recaptcha
{
    public static partial class Helpers
    {
        internal static RecaptchaSettings GetSettings(ViewContext viewContext)
        {
            // AspNetCore specific method to retrieve RecaptchaSettings object from Config service.
            var services = viewContext.HttpContext.RequestServices;
            return ((IOptions<RecaptchaSettings>)services.GetService(typeof(IOptions<RecaptchaSettings>))).Value;
        }
    }
}
