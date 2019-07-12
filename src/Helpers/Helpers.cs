using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.DependencyInjection;

namespace Finoaker.Web.Recaptcha
{
    /// <summary>
    /// A collection of static helper functions.
    /// </summary>
    public static class Helpers
    {
        /// <summary>
        /// Gets the reCAPTCHA configuration settings from the apps <see cref="IServiceCollection"/>
        /// </summary>
        /// <param name="viewContext">Current <see cref="ViewContext"/></param>
        /// <returns>reCAPTCHA configuration settings in an <see cref="RecaptchaSettings"/> object.</returns>
        internal static RecaptchaSettings GetSettings(ViewContext viewContext)
        {
            // AspNetCore specific method to retrieve RecaptchaSettings object from Config service.
            var services = viewContext.HttpContext.RequestServices;
            return ((IOptions<RecaptchaSettings>)services.GetService(typeof(IOptions<RecaptchaSettings>))).Value;
        }
    }
}
