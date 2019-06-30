using System.Threading.Tasks;
using System.Net;

namespace Finoaker.Web.Recaptcha
{
    public interface IRecaptchaService
    {
        Task<VerifyResponse> VerifyAsync(string responseToken, RecaptchaType type, string secretKey = null, IPAddress remoteIp = null);
    }
}
