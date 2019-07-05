using System;
using System.Threading.Tasks;
using System.Net;

namespace Finoaker.Web.Recaptcha
{
    [Obsolete("This interface will be removed in a future version.")]
    public interface IRecaptchaService
    {
        [Obsolete("Use the static method RecaptchaService.VerifyTokenAsync() instead of adding a service.")]
        Task<VerifyResponse> VerifyAsync(string responseToken, RecaptchaType type, string secretKey = null, IPAddress remoteIp = null);
    }
}