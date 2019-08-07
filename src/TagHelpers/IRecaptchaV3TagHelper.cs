using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Finoaker.Web.Recaptcha.TagHelpers
{
    /// <summary>
    /// Contract for <see cref="TagHelper"/> implementation for reCAPTCHA v3.
    /// </summary>
    internal interface IRecaptchaV3TagHelper
    {
        ModelExpression For { get; set; }
        string Callback { get; set; }
        string SiteKey { get; set; }
        string Action { get; set; }
        BadgeType? Badge { get; set; }
    }
}
