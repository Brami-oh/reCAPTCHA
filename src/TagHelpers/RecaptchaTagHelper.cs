using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;
using Finoaker.Web.Core;

namespace Finoaker.Web.Recaptcha.TagHelpers
{
    /// <summary>
    /// <see cref="ITagHelper"/> implementation for creating reCAPTCHA components.
    /// </summary>
    [HtmlTargetElement("recaptcha", Attributes = TypeAttributeName, TagStructure = TagStructure.WithoutEndTag)]
    public class RecaptchaTagHelper : RecaptchaTagHelperBase, IRecaptchaV3TagHelper, IRecaptchaV2CheckboxTagHelper
    {
        /// <summary>
        /// Gets the <see cref="RecaptchaSettings"/> that holds settings from confirguation.
        /// </summary>
        public RecaptchaSettings Settings
            => ((IOptions<RecaptchaSettings>)ViewContext.HttpContext.RequestServices.GetService(typeof(IOptions<RecaptchaSettings>)))?.Value;

        /// <summary>
        /// Gets the <see cref="IHtmlGenerator"/> used to generate the <see cref="RecaptchaTagHelper"/>'s output.
        /// </summary>
        public IHtmlGenerator Generator
            => (IHtmlGenerator)ViewContext.HttpContext.RequestServices.GetService(typeof(IHtmlGenerator));

        /// <summary>
        /// An expression to be evaluated against the current model.
        /// </summary>
        [HtmlAttributeName(ExpressionAttributeName)]
        public ModelExpression For { get; set; }

        /// <summary>
        /// Unique Id for hidden &lt;input&gt; element that holds reCAPTCHA challenge response. Ignored if <see cref="For" /> property is provided.
        /// </summary>
        [HtmlAttributeName(IdAttributeName)]
        public string Id { get; set; }

        /// <summary>
        /// Type of reCAPTCHA to be used eg. v2 Checkbox, v3
        /// </summary>
        [HtmlAttributeName(TypeAttributeName)]
        public RecaptchaType Type { get; set; }

        /// <summary>
        /// Your sites unique reCAPTCHA site key. Required if the site key is not provided in configuration.
        /// </summary>
        /// <remarks>Site key supplied through TagHelper attribute takes precedence.</remarks>
        [HtmlAttributeName(SiteKeyAttributeName)]
        public string SiteKey { get; set; }

        /// <summary>
        /// [v3 only] Term that reCAPTCHA service uses to provide a data break-down and adaptive risk analysis.
        /// </summary>
        /// <remarks>v3 only. Ignored for all other types.</remarks>
        [HtmlAttributeName(ActionAttributeName)]
        public string Action { get; set; }

        /// <summary>
        /// Javascript function to be executed after a successful reCAPTCHA challenge response is recieved. The response token is passed as an argument.
        /// </summary>
        [HtmlAttributeName(CallbackAttributeName)]
        public string Callback { get; set; }

        /// <summary>
        /// [v3 only] Controls position and visibility of the reCAPTCHA badge on the page.
        /// </summary>
        /// <remarks>v3 only. Ignored for all other types.</remarks>
        [HtmlAttributeName(BadgeAttributeName)]
        public BadgeType? Badge { get; set; }

        /// <summary>
        /// [v2 checkbox only] The color theme of the widget.
        /// </summary>
        /// <remarks>v2 checkbox only. Ignored for all other types.</remarks>
        [HtmlAttributeName(ThemeAttributeName)]
        public ThemeType? Theme { get; set; }

        /// <summary>
        /// [v2 checkbox only] The size of the widget.
        /// </summary>
        /// <remarks>v2 checkbox only. Ignored for all other types.</remarks>
        [HtmlAttributeName(SizeAttributeName)]
        public SizeType? Size { get; set; }

        /// <summary>
        /// [v2 checkbox only] The tabindex of the widget and challenge.
        /// </summary>
        /// <remarks>v2 checkbox only. Ignored for all other types.</remarks>
        [HtmlAttributeName(TabIndexAttributeName)]
        public int? TabIndex { get; set; }

        /// <summary>
        /// [v2 checkbox only] Javascript function to be executed when the reCAPTCHA response expires.
        /// </summary>
        /// <remarks>v2 checkbox only. Ignored for all other types.</remarks>
        [HtmlAttributeName(ExpiredCallbackAttributeName)]
        public string ExpiredCallback { get; set; }

        /// <summary>
        /// [v2 checkbox only] Javascript function to be executed when the reCAPTCHA response encounters an error.
        /// </summary>
        /// <remarks>
        /// v2 checkbox only. Ignored for all other types.
        /// Usually related to network connectivity. If a function is specified here, you are responsible for informing the user they should retry.
        /// </remarks>
        [HtmlAttributeName(ErrorCallbackAttributeName)]
        public string ErrorCallback { get; set; }

        /// <summary>
        /// Synchronously executes the <see cref="RecaptchaTagHelper"/> with the given context and output.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <see cref="SiteKey"/> property / attribute is null or empty and a key cannot be found in configuration settings.
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

            var tagBuilder = GenerateHtml();

            output.Reinitialize(tagBuilder.TagName, TagMode.StartTagAndEndTag);
            output.MergeAttributes(tagBuilder);
            output.Content.AppendHtml(tagBuilder.InnerHtml);
        }

        internal TagBuilder GenerateHtml()
        {
            if (Generator is null)
            {
                throw new ArgumentNullException(nameof(Generator));
            }

            TagBuilder input = null;

            if (For != null || !string.IsNullOrWhiteSpace(Id))
            {
                // Generate an <input> element using Model or Id property details. 
                input = Generator.GenerateHidden(
                    ViewContext,
                    For?.ModelExplorer,
                    For?.Name ?? Id,
                    null,
                    true,
                    null);
            }

            var props = new RecaptchaProps(
                Type,
                SiteKey.TrimToNull() ?? Settings?.First(Type)?.SiteKey,
                callback: Callback,
                expiredCallback: ExpiredCallback,
                errorCallback: ErrorCallback,
                theme: Theme,
                tabIndex: TabIndex,
                size: Size,
                badge: Badge,
                action: Action);

            return props.GenerateHtml(input);
        }
    }
}
