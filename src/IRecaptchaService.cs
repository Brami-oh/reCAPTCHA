using System;
using System.Threading.Tasks;
using System.Net;
using Microsoft.Extensions.DependencyInjection;

namespace Finoaker.Web.Recaptcha
{
    /// <summary>
    /// Interface definition for services to be added to the apps <see cref="IServiceCollection"/> at startup.
    /// </summary>
    //[Obsolete("This interface will be removed in a future version.")]
    public interface IRecaptchaService
    {
        /// <summary>
        /// Verifies a user's response to a reCAPTCHA challenge in an asynchronous operation.
        /// </summary>
        /// <param name="responseToken">The user response token provided by the reCAPTCHA client-side integration on your site.</param>
        /// <param name="type">The type of reCAPTCHA being verified.</param>
        /// <param name="secretKey">The shared key between your site and reCAPTCHA.</param>
        /// <param name="remoteIp">The user's IP address.</param>
        /// <returns><see cref="Task{VerifyResponse}"/> object containing the response from reCAPTCHA verification service.</returns>
        [Obsolete("Use the static method RecaptchaService.VerifyTokenAsync() instead.")]
        Task<VerifyResponse> VerifyAsync(string responseToken, RecaptchaType type, string secretKey = null, IPAddress remoteIp = null);
    }
}