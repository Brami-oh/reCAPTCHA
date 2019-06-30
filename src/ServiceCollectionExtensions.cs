using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Finoaker.Web.Recaptcha
{
    public static class ServiceCollectionExtensions
    {
        public static void AddRecaptcha(this IServiceCollection services, IConfigurationSection section)
        {
            services.Configure<RecaptchaSettings>(section);
            services.AddTransient<IRecaptchaService, RecaptchaService>();
        }
    }
}
