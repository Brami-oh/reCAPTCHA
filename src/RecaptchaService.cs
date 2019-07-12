﻿using System;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;

namespace Finoaker.Web.Recaptcha
{
    /// <summary>
    /// Initializes a new instance of <see cref="RecaptchaService"/> that's used to verify a reCAPTCHA response.
    /// </summary>
    public class RecaptchaService : IRecaptchaService
    {
        private readonly RecaptchaSettings _settings;
        private const string VerifyUrl = "https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}&remoteip={2}";

        /// <summary>
        /// Constructor used when adding service to apps <see cref="IServiceCollection"/>
        /// </summary>
        /// <param name="settings"><see cref="RecaptchaSettings"/> object that has reCAPTCHA keys from configuration.</param>
        public RecaptchaService(IOptions<RecaptchaSettings> settings)
        {
            _settings = settings.Value;
        }

        /// <summary>
        /// Verifies a user's response to a reCAPTCHA challenge in an asynchronous operation.
        /// </summary>
        /// <param name="responseToken">The user response token provided by the reCAPTCHA client-side integration on your site.</param>
        /// <param name="type">The type of reCAPTCHA being verified.</param>
        /// <param name="secretKey">The shared key between your site and reCAPTCHA.</param>
        /// <param name="remoteIp">The user's IP address.</param>
        /// <returns><see cref="Task{VerifyResponse}"/> object containing the response from reCAPTCHA verification service.</returns>
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

        /// <summary>
        /// Verifies a user's response to a reCAPTCHA challenge in an asynchronous operation.
        /// </summary>
        /// <param name="responseToken">The user response token provided by the reCAPTCHA client-side integration on your site.</param>
        /// <param name="type">The type of reCAPTCHA being verified.</param>
        /// <param name="settings"><see cref="RecaptchaSettings"/> object that has a secret key of the <see cref="RecaptchaType"/> specificed in the type parameter.</param>
        /// <param name="remoteIp">The user's IP address.</param>
        /// <returns><see cref="Task{VerifyResponse}"/> object containing the response from reCAPTCHA verification service.</returns>
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

        /// <summary>
        /// Verifies a user's response to a reCAPTCHA challenge in an asynchronous operation.
        /// </summary>
        /// <param name="responseToken">The user response token provided by the reCAPTCHA client-side integration on your site.</param>
        /// <param name="type">The type of reCAPTCHA being verified.</param>
        /// <param name="secretKey">The shared key between your site and reCAPTCHA.</param>
        /// <param name="remoteIp">The user's IP address.</param>
        /// <returns><see cref="Task{VerifyResponse}"/> object containing the response from reCAPTCHA verification service.</returns>
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