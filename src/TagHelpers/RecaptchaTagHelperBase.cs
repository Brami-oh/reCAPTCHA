using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Finoaker.Web.Recaptcha.TagHelpers
{
    /// <summary>
    /// An abstract base class for reCAPTCHA components.
    /// </summary>
    public abstract class RecaptchaTagHelperBase : TagHelper
    {
        // Attribute names used in Recaptcha TagHelpers
        
        internal const string TypeAttributeName = "type";
        internal const string IdAttributeName = "id";
        internal const string ExpressionAttributeName = "for";
        internal const string SiteKeyAttributeName = "sitekey";
        internal const string ActionAttributeName = "action";
        internal const string SizeAttributeName = "size";
        internal const string ThemeAttributeName = "theme";
        internal const string TabIndexAttributeName = "tabindex";
        internal const string CallbackAttributeName = "callback";
        internal const string ExpiredCallbackAttributeName = "expired-callback";
        internal const string ErrorCallbackAttributeName = "error-callback";
        internal const string BadgeAttributeName = "badge";
        internal const string ObsoleteExpressionAttributeName = "asp-for";

        /// <summary>
        /// Gets the ViewContext of the executing view.
        /// </summary>
        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }
    }
}
