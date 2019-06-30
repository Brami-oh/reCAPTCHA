using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Runtime.Serialization.Json;
using Microsoft.Extensions.Options;

namespace Finoaker.Web.Recaptcha
{
    public class RecaptchaService : IRecaptchaService
    {
        private readonly RecaptchaSettings _settings; 

        public RecaptchaService(IOptions<RecaptchaSettings> settings)
        {
            _settings = settings.Value;
        }

        public async Task<VerifyResponse> VerifyAsync(string responseToken, RecaptchaType type, string secretKey = null, IPAddress remoteIp = null)
        {
            if (string.IsNullOrEmpty(responseToken))
            {
                throw new ArgumentNullException(nameof(responseToken));
            }

            if (string.IsNullOrEmpty(secretKey) && string.IsNullOrEmpty(_settings?.First(type)?.SecretKey))
            {
                throw new ArgumentNullException(nameof(secretKey));
            }

            var url = new StringBuilder(Helpers.RecaptchaVerifyUrl)
                .Append($"?secret={secretKey ?? _settings.First(type).SecretKey}")
                .Append($"&response={responseToken}");

            if (remoteIp != null)
            {
                url.Append($"&remoteip={remoteIp.ToString()}");
            }

            var result = await new HttpClient().GetStreamAsync(url.ToString());

            var serializer = new DataContractJsonSerializer(typeof(VerifyResponse));

            return (VerifyResponse)serializer.ReadObject(result);
        }
    }
}
