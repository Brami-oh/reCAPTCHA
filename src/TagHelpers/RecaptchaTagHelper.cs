using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace Finoaker.Web.Recaptcha.TagHelpers
{
    /// <summary>
    /// <see cref="ITagHelper"/> implementation for creating reCAPTCHA components.
    /// </summary>
    [HtmlTargetElement(RecaptchaTagName, Attributes = TypeAttributeName, TagStructure = TagStructure.WithoutEndTag)]
    public class RecaptchaTagHelper : TagHelper
    {
        private const string RecaptchaTagName = "recaptcha";
        private const string TypeAttributeName = "type";
        private const string ExpressionAttributeName = "for";
        private const string SiteKeyAttributeName = "sitekey";
        private const string ActionAttributeName = "action";
        private const string SizeAttributeName = "size";
        private const string ThemeAttributeName = "theme";
        private const string TabIndexAttributeName = "tabindex";
        private const string CallbackAttributeName = "callback";
        private const string ExpiredCallbackAttributeName = "expired-callback";
        private const string ErrorCallbackAttributeName = "error-callback";
        private const string BadgeVisibleAttributeName = "badge-visible";
        private const string BadgePositionAttributeName = "badge-position";

        /// <summary>
        /// Creates a new <see cref="RecaptchaTagHelper"/>.
        /// </summary>
        /// <param name="settings">The <see cref="RecaptchaSettings"/> contains settings from configuration.</param>
        /// <param name="generator">The <see cref="IHtmlGenerator"/>.</param>
        public RecaptchaTagHelper(
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
        /// Gets the <see cref="IHtmlGenerator"/> used to generate the <see cref="RecaptchaTagHelper"/>'s output.
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
        /// Type of reCAPTCHA to be used eg. v2 Checkbox, v3
        /// </summary>
        [HtmlAttributeName(TypeAttributeName)]
        public RecaptchaType Type { get; set; }

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
        /// <remarks>V3 only.</remarks>
        [HtmlAttributeName(BadgeVisibleAttributeName)]
        public bool? IsBadgeVisible { get; set; }

        /// <summary>
        /// The color theme of the widget.
        /// </summary>
        /// <remarks>V2 checkbox only.</remarks>
        [HtmlAttributeName(ThemeAttributeName)]
        public Theme? Theme { get; set; }

        /// <summary>
        /// The size of the widget.
        /// </summary>
        [HtmlAttributeName(SizeAttributeName)]
        public Size? Size { get; set; }

        /// <summary>
        /// The tabindex of the widget and challenge. If other elements in your page use tabindex, it should be set to make user navigation easier.
        /// </summary>
        [HtmlAttributeName(TabIndexAttributeName)]
        public int? TabIndex { get; set; }

        /// <summary>
        /// The name of your callback function, executed when the reCAPTCHA response expires and the user needs to re-verify.
        /// </summary>
        [HtmlAttributeName(ExpiredCallbackAttributeName)]
        public string ExpiredCallback { get; set; }

        /// <summary>
        /// The name of your callback function, executed when reCAPTCHA encounters an error (usually network connectivity) and cannot continue until connectivity is restored. If you specify a function here, you are responsible for informing the user that they should retry.
        /// </summary>
        [HtmlAttributeName(ErrorCallbackAttributeName)]
        public string ErrorCallback { get; set; }

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

            RecaptchaTags result = null;

            switch (Type)
            {
                case RecaptchaType.V2Checkbox:
                    result = GenerateV2CheckboxTags(
                        output,
                        ViewContext,
                        Generator,
                        For,
                        SiteKey,
                        Callback,
                        ExpiredCallback,
                        ErrorCallback,
                        Size,
                        Theme,
                        TabIndex);
                    break;

                case RecaptchaType.V3:
                    result = GenerateV3Tags(
                        ViewContext,
                        Generator,
                        Settings,
                        For,
                        SiteKey,
                        Callback,
                        Action,
                        IsBadgeVisible);
                    break;

                default:
                    throw new NotImplementedException("Chosen reCAPTCHA type is not supported.");
            }

            output.Reinitialize("input", TagMode.SelfClosing);
            output.MergeAttributes(result.HiddenInputTag);
            output.PostElement.AppendHtml(result.ScriptTag);
        }

        internal static RecaptchaTags GenerateV2CheckboxTags(
            TagHelperOutput output,
            ViewContext viewContext,
            IHtmlGenerator generator,
            ModelExpression expression,
            string sitekey,
            string callback,
            string expiredCallback,
            string errorCallback,
            Size? size,
            Theme? theme,
            int? tabIndex)
        {
            if (string.IsNullOrEmpty(sitekey))
            {
                throw new ArgumentNullException(nameof(sitekey));
            }

            if (sitekey.Trim().Length != 40)
            {
                throw new ArgumentException("Site Key must be 40 characters long.", nameof(sitekey));
            }

            var htmlAttributes = HtmlHelperExtensions.GenerateV2CheckboxAttributes(
                sitekey,
                callback,
                expiredCallback: expiredCallback,
                errorCallback: errorCallback,
                theme: theme,
                size: size,
                tabIndex: tabIndex);

            throw new NotImplementedException();
        }

        internal static RecaptchaTags GenerateV3Tags(
            ViewContext viewContext,
            IHtmlGenerator generator,
            RecaptchaSettings settings,
            ModelExpression expression,
            string sitekey,
            string callback,
            string action,
            bool? isBadgeVisible)
        {
            sitekey = string.IsNullOrEmpty(sitekey) ? settings?.First(RecaptchaType.V3)?.SiteKey.Trim() : sitekey?.Trim();
            if (string.IsNullOrEmpty(sitekey))
            {
                throw new ArgumentNullException(nameof(sitekey));
            }

            if (sitekey.Trim().Length != 40)
            {
                throw new ArgumentException("Site Key must be 40 characters long.", nameof(sitekey));
            }

            if (string.IsNullOrEmpty(action))
            {
                // Try getting reCAPTCHA action from various sources or assign default value.
                action = HtmlHelperExtensions.GetAction(viewContext);
            }

            // default to true of not set
            isBadgeVisible = isBadgeVisible ?? true;

            // default tag name / id if model property not set
            var expressionName = expression?.Name ?? "recaptcha-v3--g-recaptcha";

            var hiddenInputAttributes = HtmlHelperExtensions.GenerateV3Attributes(
                sitekey: sitekey,
                action: action,
                callback: callback,
                isBadgeVisible: isBadgeVisible.Value);

            var hiddenInputTag = generator.GenerateHidden(
                viewContext: viewContext,
                modelExplorer: expression?.ModelExplorer,
                expression: expressionName,
                value: null,
                useViewData: true,
                htmlAttributes: hiddenInputAttributes);

            return new RecaptchaTags
            {
                HiddenInputTag = hiddenInputTag,
                ScriptTag = HtmlHelperExtensions.GenerateScriptTag(expressionName)
            };
        }
    }
}
