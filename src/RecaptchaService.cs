﻿using System;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using Microsoft.Extensions.Options;

namespace Finoaker.Web.Recaptcha
{
    public class RecaptchaService : IRecaptchaService
    {
        private readonly RecaptchaSettings _settings;
        private const string VerifyUrl = "https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}&remoteip={2}";

        public RecaptchaService(IOptions<RecaptchaSettings> settings)
        {
            _settings = settings.Value;
        }

        public async Task<VerifyResponse> VerifyAsync(string responseToken, RecaptchaType type, string secretKey = null, IPAddress remoteIp = null)
        {
            secretKey = secretKey ?? _settings?.First(type)?.SecretKey;

            if (string.IsNullOrEmpty(secretKey))
            {
                throw new ArgumentNullException(nameof(secretKey));
            }

            return await RecaptchaService.VerifyTokenAsync(
                type,
                responseToken,
                secretKey,
                remoteIp: remoteIp);
        }

        private static async Task<VerifyResponse> VerifyTokenAsync(RecaptchaType type, string responseToken, string secretKey, IPAddress remoteIp)
        {
            if (string.IsNullOrEmpty(responseToken))
            {
                throw new ArgumentNullException(nameof(responseToken));
            }

            var url = string.Format(VerifyUrl, secretKey, responseToken, remoteIp);

            var result = await new HttpClient().GetStreamAsync(url);

            var serializer = new DataContractJsonSerializer(typeof(VerifyResponse));

            return (VerifyResponse)serializer.ReadObject(result);
        }

        public static async Task<VerifyResponse> VerifyTokenAsync(string responseToken, RecaptchaType type, RecaptchaSettings settings, IPAddress remoteIp = null)
        {
            if (string.IsNullOrEmpty(settings?.First(type)?.SecretKey))
            {
                throw new ArgumentNullException("SecretKey", "The secret key of the required type, is not found or null in RecaptchaSettings argument.");
            }

            return await RecaptchaService.VerifyTokenAsync(
                type,
                responseToken,
                settings.First(type).SecretKey,
                remoteIp: remoteIp);
        }

        public static async Task<VerifyResponse> VerifyTokenAsync(string responseToken, RecaptchaType type, string secretKey, IPAddress remoteIp = null)
        {
            if (string.IsNullOrEmpty(secretKey))
            {
                throw new ArgumentNullException(nameof(secretKey));
            }

            return await RecaptchaService.VerifyTokenAsync(
                type,
                responseToken,
                secretKey,
                remoteIp: remoteIp);
        }
    }
}