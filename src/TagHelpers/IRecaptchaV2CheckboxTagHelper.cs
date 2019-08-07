using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Finoaker.Web.Recaptcha.TagHelpers
{
    /// <summary>
    /// Contract for <see cref="TagHelper"/> implementation for reCAPTCHA v2 Checkbox.
    /// </summary>
    internal interface IRecaptchaV2CheckboxTagHelper
    {
        ModelExpression For { get; set; }
        string Callback { get; set; }
        string SiteKey { get; set; }
        ThemeType? Theme { get; set; }
        SizeType? Size { get; set; }
        int? TabIndex { get; set; }
        string ExpiredCallback { get; set; }
        string ErrorCallback { get; set; }
    }
}
