using System;
﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Finoaker.Web.Recaptcha
{
    public static class ServiceCollectionExtensions
    {
        [Obsolete("Add RecaptchaSettings in ConfigureServices method of Startup class instead. This extennsion method will be removed in a future version.")]
        public static void AddRecaptcha(this IServiceCollection services, IConfigurationSection section)
        {
            services.Configure<RecaptchaSettings>(section);
            services.AddTransient<IRecaptchaService, RecaptchaService>();
        }
    }
}