using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace Finoaker.Web.Recaptcha.TagHelpers
{
    /// <summary>
    /// <see cref="ITagHelper"/> implementation targeting &lt;form&gt; element, for creating reCAPTCHA V3 component.
    /// </summary>
    [HtmlTargetElement("form", Attributes = "recaptcha-*")]
    public class RecaptchaFormTagHelper : TagHelper
    {
        private const string ExpressionAttributeName = "recaptcha-for";
        private const string SiteKeyAttributeName = "recaptcha-sitekey";
        private const string ActionAttributeName = "recaptcha-action";
        private const string CallbackAttributeName = "recaptcha-callback";
        private const string BadgeVisibleAttributeName = "recaptcha-badge-visible";
        private const string BadgePositionAttributeName = "recaptcha-badge-position";

        /// <summary>
        /// Creates a new <see cref="RecaptchaFormTagHelper"/>.
        /// </summary>
        /// <param name="settings">The <see cref="RecaptchaSettings"/> contains settings from configuration.</param>
        /// <param name="generator">The <see cref="IHtmlGenerator"/>.</param>
        public RecaptchaFormTagHelper(
            IOptions<RecaptchaSettings> settings, 
            IHtmlGenerator generator)
        {
            Settings = settings?.Value;
            Generator = generator;
        }

        /// <summary>
        /// Gets the <see cref="RecaptchaSettings"/> that contains settings from configuration.
        /// </summary>
        protected RecaptchaSettings Settings { get; }

        /// <summary>
        /// Gets the <see cref="IHtmlGenerator"/> used to generate the <see cref="RecaptchaFormTagHelper"/>'s output.
        /// </summary>
        protected IHtmlGenerator Generator { get; }

        /// <inheritdoc />
        public override int Order => -1000;

        /// <summary>
        /// An expression to be evaluated against the current model.
        /// </summary>
        [HtmlAttributeName(ExpressionAttributeName)]
        public ModelExpression For { get; set; }

        /// <summary>
        /// Your site key. Required if site key is not stored in configuration.
        /// </summary>
        /// <remarks>
        /// Cannot be <c>null</c> if keys are not in configuration settings.
        /// </remarks>
        [HtmlAttributeName(SiteKeyAttributeName)]
        public string SiteKey { get; set; }

        /// <summary>
        /// Name that the reCAPTCHA service uses to provide a data break-down and adaptive risk analysis. 
        /// </summary>
        [HtmlAttributeName(ActionAttributeName)]
        public string Action { get; set; }

        /// <summary>
        /// Name of a Javascript function to be executed on successful response. The response token is passed to this function.
        /// </summary>
        [HtmlAttributeName(CallbackAttributeName)]
        public string Callback { get; set; }

        /// <summary>
        /// Controls visibility of the reCAPTCHA badge.
        /// </summary>
        [HtmlAttributeName(BadgeVisibleAttributeName)]
        public bool IsBadgeVisible { get; set; } = true;

        /// <summary>
        /// Gets the ViewContext of the executing view.
        /// </summary>
        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        /// <inheritdoc />
        /// <exception cref="ArgumentNullException">
        /// Thrown if <see cref="SiteKey"/> attribute is null or empty and a key cannot be found in configuration settings.
        /// </exception>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (output is null)
            {
                throw new ArgumentNullException(nameof(output));
            }

            var result = RecaptchaTagHelper.GenerateV3Tags(
                ViewContext,
                Generator,
                Settings,
                For,
                SiteKey,
                Callback,
                Action,
                IsBadgeVisible);

            // Append to the end of the form
            output.PostContent.AppendHtml(result.HiddenInputTag);
            output.PostContent.AppendHtml(result.ScriptTag);
        }
    }
}
