using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace Finoaker.Web.Recaptcha.TagHelpers
{
    /// <summary>
    /// <see cref="ITagHelper"/> implementation for creating a reCAPTCHA V3 component and binding the response to a model property.
    /// </summary>
    [HtmlTargetElement(RecpatachaV3TagClassName, TagStructure = TagStructure.NormalOrSelfClosing)]
    public class RecaptchaV3TagHelper : TagHelper
    {
        private const string RecpatachaV3TagClassName = "recaptcha-v3";
        private IHtmlGenerator _generator;
        private RecaptchaSettings _settings;

        /// <summary>
        /// Constructor for <see cref="RecaptchaV3TagHelper"/>.
        /// </summary>
        /// <param name="settings"><see cref="RecaptchaSettings"/> object that has reCAPTCHA keys from configuration.</param>
        /// <param name="generator"><see cref="IHtmlGenerator"/> used to assist in generating Html content.</param>
        public RecaptchaV3TagHelper(IOptions<RecaptchaSettings> settings, IHtmlGenerator generator)
        {
            _settings = settings?.Value;
            _generator = generator;
        }

        /// <summary>
        /// An expression to be evaluated against the current model.
        /// </summary>
        [HtmlAttributeName("asp-for")]
        public ModelExpression Expression { get; set; }

        /// <summary>
        /// Your site key. Required if the site key is not in your appSettings.
        /// </summary>
        [HtmlAttributeName("sitekey")]
        public string SiteKey
        {
            get
            {
                return _siteKey ?? _settings?.First(RecaptchaType.V3)?.SiteKey;
            }
            set
            {
                _siteKey = value;
            }
        }
        private string _siteKey;

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
        /// Gets the <see cref="Microsoft.AspNetCore.Mvc.Rendering.ViewContext"/> of the executing view.
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

            if (string.IsNullOrEmpty(SiteKey))
            {
                throw new ArgumentNullException(nameof(SiteKey));
            }

            var hiddenInputTag = _generator.GenerateHidden(
                ViewContext, 
                Expression?.ModelExplorer, 
                Expression?.Name ?? "recaptcha-v3--g-recaptcha", 
                null, 
                true, 
                null);

            output.Reinitialize("div", TagMode.StartTagAndEndTag);
            output.Attributes.Add("class", HtmlHelperExtensions.ContainerV3CssClass);

            output.Content.AppendHtml(
                HtmlHelperExtensions.GenerateHtmlContent(
                viewContext: ViewContext,
                hiddenInputTag: hiddenInputTag,
                siteKey: SiteKey,
                callback: Callback,
                action: Action,
                isBadgeVisible: IsBadgeVisible)
                );
        }
    }
}