using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace Finoaker.Web.Recaptcha.TagHelpers
{
    /// <summary>
    /// <see cref="ITagHelper"/> implementation for creating a reCAPTCHA V3 component and binding the response to a model property.
    /// </summary>
    [Obsolete("This TagHelper is obsolete and will be removed in a future version. Use <recaptcha type=\"V3\" /> instead.")]
    [HtmlTargetElement(RecaptchaV3TagName, TagStructure = TagStructure.WithoutEndTag)]
    public class RecaptchaV3TagHelper : TagHelper
    {
        private const string RecaptchaV3TagName = "recaptcha-v3";

        /// <summary>
        /// Creates a new <see cref="RecaptchaV3TagHelper"/>.
        /// </summary>
        /// <param name="settings">The <see cref="RecaptchaSettings"/> contains settings from configuration.</param>
        /// <param name="generator">The <see cref="IHtmlGenerator"/>.</param>
        public RecaptchaV3TagHelper(
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
        /// Gets the <see cref="IHtmlGenerator"/> used to generate the <see cref="RecaptchaV3TagHelper"/>'s output.
        /// </summary>
        protected IHtmlGenerator Generator { get; }

        /// <inheritdoc />
        public override int Order => -1000;

        /// <summary>
        /// An expression to be evaluated against the current model.
        /// </summary>
        [HtmlAttributeName("asp-for")]
        public ModelExpression Expression { get; set; }

        /// <summary>
        /// Your site key. Required if the site key is not in your appSettings.
        /// </summary>
        [HtmlAttributeName("sitekey")]
        public string SiteKey { get; set; }

        /// <summary>
        /// The name of your callback function, executed when the user submits a successful response. The response token is passed to this callback.
        /// </summary>
        [HtmlAttributeName("callback")]
        public string Callback { get; set; }

        /// <summary>
        /// A name that the reCAPTCHA services uses to provide a data break-down and adaptive risk analysis. 
        /// </summary>
        [HtmlAttributeName("action")]
        public string Action { get; set; }

        /// <summary>
        /// Used to hide the reCAPTCHA badge and display simple text brand visibility instead.
        /// </summary>
        [HtmlAttributeName("badge-visible")]
        public bool IsBadgeVisible { get; set;  } = true;

        /// <summary>
        /// Gets the ViewContext of the executing view.
        /// </summary>
        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        /// <summary>
        /// Synchronously executes the <see cref="RecaptchaV3TagHelper"/> with the given context and output.
        /// </summary>
        /// <param name="context">Contains information associated with the current HTML tag.</param>
        /// <param name="output">A stateful HTML element used to generate an HTML tag.</param>
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
                Expression, 
                SiteKey, 
                Callback, 
                Action, 
                IsBadgeVisible);

            output.Reinitialize("input", TagMode.SelfClosing);
            output.MergeAttributes(result.HiddenInputTag);
            output.PostElement.AppendHtml(result.ScriptTag);
        }
    }
}