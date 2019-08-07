using System;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Finoaker.Web.Recaptcha.TagHelpers
{
    /// <summary>
    /// <see cref="ITagHelper"/> implementation targeting &lt;form&gt; element, for creating reCAPTCHA v3 component.
    /// </summary>
    [HtmlTargetElement("form", Attributes = AttributePrefix + "*")]
    public class RecaptchaFormTagHelper : RecaptchaTagHelperBase, IRecaptchaV3TagHelper
    {
        private const string AttributePrefix = "recaptcha-";

        /// <summary>
        /// An expression to be evaluated against the current model.
        /// </summary>
        [HtmlAttributeName(AttributePrefix + ExpressionAttributeName)]
        public ModelExpression For { get; set; }

        /// <summary>
        /// Unique Id for hidden &lt;input&gt; element that holds reCAPTCHA challenge response. Ignored if <see cref="For" /> property is provided.
        /// </summary>
        [HtmlAttributeName(AttributePrefix + IdAttributeName)]
        public string Id { get; set; }

        /// <summary>
        /// Your sites unique reCAPTCHA site key. Required if the site key is not provided in configuration.
        /// </summary>
        /// <remarks>Site key supplied through TagHelper attribute takes precedence.</remarks>
        [HtmlAttributeName(AttributePrefix + SiteKeyAttributeName)]
        public string SiteKey { get; set; }

        /// <summary>
        /// Term that reCAPTCHA service uses to provide a data break-down and adaptive risk analysis.
        /// </summary>
        [HtmlAttributeName(AttributePrefix + ActionAttributeName)]
        public string Action { get; set; }

        /// <summary>
        /// Javascript function to be executed after a successful reCAPTCHA challenge response is recieved. The response token is passed as an argument.
        /// </summary>
        [HtmlAttributeName(AttributePrefix + CallbackAttributeName)]
        public string Callback { get; set; }

        /// <summary>
        /// Controls position and visibility of the reCAPTCHA badge on the page.
        /// </summary>
        [HtmlAttributeName(AttributePrefix + BadgeAttributeName)]
        public BadgeType? Badge { get; set; }

        /// <summary>
        /// Synchronously executes the <see cref="RecaptchaTagHelper"/> with the given context and output.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <see cref="SiteKey" /> property / attribute is null or empty and a key cannot be found in configuration settings.
        /// </exception>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
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

            // recAPTCHA Html and script is appended to the end of the form. Rest of the form is unchanged.
            output.PostContent.AppendHtml(tagHelper.GenerateHtml());
        }
    }
}
