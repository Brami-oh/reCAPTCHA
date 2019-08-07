using System;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Finoaker.Web.Recaptcha.TagHelpers
{
    /// <summary>
    /// [deprecated] <see cref="ITagHelper"/> implementation for creating a reCAPTCHA v3 component and binding the response to a model property.
    /// </summary>
    [Obsolete("This TagHelper is obsolete and will be removed in a future version. Use <recaptcha type=\"V3\" /> instead.")]
    [HtmlTargetElement("recaptcha-v3", TagStructure = TagStructure.WithoutEndTag)]
    public class RecaptchaV3TagHelper : RecaptchaTagHelperBase, IRecaptchaV3TagHelper
    {
        private const string ObsoleteBadgeVisibleAttributeName = "badge-visible";

        /// <summary>
        /// [deprecated] An expression to be evaluated against the current model.
        /// </summary>
        [HtmlAttributeName(ObsoleteExpressionAttributeName)]
        [Obsolete("This property is obsolete and will be removed in a future version. Use 'for' attribute instead.")]
        public ModelExpression For2 { get => For; set => For = For ?? value; }

        /// <summary>
        /// An expression to be evaluated against the current model.
        /// </summary>
        [HtmlAttributeName(ExpressionAttributeName)]
        public ModelExpression For { get; set; }

        /// <summary>
        /// Your sites unique reCAPTCHA site key. Required if the site key is not provided in configuration.
        /// </summary>
        /// <remarks>Site key supplied through TagHelper attribute takes precedence.</remarks>
        [HtmlAttributeName(SiteKeyAttributeName)]
        public string SiteKey { get; set; }

        /// <summary>
        /// Javascript function to be executed after a successful reCAPTCHA challenge response is recieved. The response token is passed as an argument.
        /// </summary>
        [HtmlAttributeName(CallbackAttributeName)]
        public string Callback { get; set; }

        /// <summary>
        /// Term that reCAPTCHA service uses to provide a data break-down and adaptive risk analysis.
        /// </summary>
        [HtmlAttributeName(ActionAttributeName)]
        public string Action { get; set; }

        /// <summary>
        /// Controls position and visibility of the reCAPTCHA badge on the page.
        /// </summary>
        [HtmlAttributeName(BadgeAttributeName)]
        public BadgeType? Badge { get; set; }

        /// <summary>
        /// [deprecated] Controls visibility of the reCAPTCHA badge.
        /// </summary>
        [HtmlAttributeName(ObsoleteBadgeVisibleAttributeName)]
        [Obsolete("This property / attribute is obsolete and will be removed in a future version.")]
        public bool? IsBadgeVisible { get; set;  }
        
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            // todo: Obsolete and targeted for removal in a future version.
            if (IsBadgeVisible.HasValue && !IsBadgeVisible.Value)
            {
                Badge = BadgeType.None;
            }

            var tagHelper = new RecaptchaTagHelper()
            {
                Callback = Callback,
                For = For,
                SiteKey = SiteKey,
                Type = RecaptchaType.V3,
                ViewContext = ViewContext,
                Action = Action,
                Badge = Badge
            };

            tagHelper.Process(context, output);
        }
    }
}