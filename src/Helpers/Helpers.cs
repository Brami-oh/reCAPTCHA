using System;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization.Json;
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

        public const string VerifyUrl = "https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}&remoteip={2}";

        public static async Task<VerifyResponse> VerifyAsync(string responseToken, RecaptchaType type, RecaptchaSettings settings, IPAddress remoteIp = null)
        {
            if (string.IsNullOrEmpty(responseToken))
            {
                throw new ArgumentNullException(nameof(responseToken));
            }

            if (string.IsNullOrEmpty(settings?.First(type)?.SecretKey))
            {
                throw new ArgumentNullException("SecretKey");
            }

            var url = string.Format(VerifyUrl, settings.First(type).SecretKey, responseToken, remoteIp);

            var result = await new HttpClient().GetStreamAsync(url);

            var serializer = new DataContractJsonSerializer(typeof(VerifyResponse));

            return (VerifyResponse)serializer.ReadObject(result);
        }
    }
}
