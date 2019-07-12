using System;
﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Finoaker.Web.Recaptcha
{
    /// <summary>
    /// Contains static extension methods for service configuration.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds settings and a new instance of RecaptchaService class to the current <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">Instance of the apps <see cref="IServiceCollection"/></param>
        /// <param name="section">The <see cref="IConfigurationSection"/> that contains settings for reCAPTCHA service.</param>
        [Obsolete("Add RecaptchaSettings in ConfigureServices method of Startup class instead. This extennsion method will be removed in a future version.")]
        public static void AddRecaptcha(this IServiceCollection services, IConfigurationSection section)
        {
            services.Configure<RecaptchaSettings>(section);
            services.AddTransient<IRecaptchaService, RecaptchaService>();
        }
    }
}