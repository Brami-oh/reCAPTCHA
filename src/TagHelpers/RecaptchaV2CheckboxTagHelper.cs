using System;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Finoaker.Web.Recaptcha.TagHelpers
{
    /// <summary>
    /// <see cref="ITagHelper"/> implementation for creating a reCAPTCHA version 2 Checkbox component. 
    /// </summary>
    [HtmlTargetElement("div", Attributes = "[class=" + RecpatachaV2CheckboxTagClassName + "]", TagStructure = TagStructure.NormalOrSelfClosing)]
    [HtmlTargetElement(RecpatachaV2CheckboxTagClassName, TagStructure = TagStructure.NormalOrSelfClosing)]
    public class RecaptchaV2CheckboxTagHelper : TagHelper
    {
        private const string RecpatachaV2CheckboxTagClassName = "recaptcha-v2-checkbox";

        private IHtmlGenerator _generator;
        private RecaptchaSettings _settings;

        public RecaptchaV2CheckboxTagHelper(IOptions<RecaptchaSettings> settings, IHtmlGenerator generator)
        {
            _settings = settings.Value;
            _generator = generator;
        }

        /// <summary>
        /// An expression to be evaluated against the current model.
        /// </summary>
        [HtmlAttributeName("asp-for")]
        public ModelExpression Expression { get; set; }

        /// <summary>
        /// Your site key. Required if the site key is not in provided through appSettings.
        /// </summary>
        [HtmlAttributeName("sitekey")]
        public string SiteKey { get; set; }

        /// <summary>
        /// The color theme of the widget.
        /// </summary>
        [HtmlAttributeName("theme")]
        public Theme? Theme { get; set; }

        /// <summary>
        /// The size of the widget.
        /// </summary>
        [HtmlAttributeName("size")]
        public Size? Size { get; set; }

        /// <summary>
        /// The tabindex of the widget and challenge. If other elements in your page use tabindex, it should be set to make user navigation easier.
        /// </summary>
        [HtmlAttributeName("tabindex")]
        public int? TabIndex { get; set; }

        /// <summary>
        /// The name of your callback function, executed when the user submits a successful response. The response token is passed to this callback.
        /// </summary>
        [HtmlAttributeName("callback")]
        public string Callback { get; set; }

        /// <summary>
        /// The name of your callback function, executed when the reCAPTCHA response expires and the user needs to re-verify.
        /// </summary>
        [HtmlAttributeName("expired-callback")]
        public string ExpiredCallback { get; set; }

        /// <summary>
        /// The name of your callback function, executed when reCAPTCHA encounters an error (usually network connectivity) and cannot continue until connectivity is restored. If you specify a function here, you are responsible for informing the user that they should retry.
        /// </summary>
        [HtmlAttributeName("error-callback")]
        public string ErrorCallback { get; set; }

        /// <summary>
        /// Gets the <see cref="Microsoft.AspNetCore.Mvc.Rendering.ViewContext"/> of the executing view.
        /// </summary>
        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

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

            output.Reinitialize("div", TagMode.StartTagAndEndTag);
            output.Attributes.SetAttribute("class", HtmlHelperExtensions.ContainerV2CssClass);

            var hiddenInputTag = _generator.GenerateHidden(
                ViewContext, 
                Expression?.ModelExplorer, 
                Expression?.Name ?? "recaptcha-v2--g-recaptcha", 
                null, 
                true, 
                null);

            output.Content.AppendHtml(HtmlHelperExtensions.GenerateHtmlContent(
                Helpers.GetOptions(ViewContext),
                hiddenInputTag: hiddenInputTag,
                siteKey: SiteKey,
                theme: Theme,
                size: Size,
                tabIndex: TabIndex,
                callback: Callback,
                expiredCallback: ExpiredCallback,
                errorCallback: ErrorCallback)
                );
        }
    }
}