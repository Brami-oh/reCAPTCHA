using System;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Finoaker.Web.Recaptcha.TagHelpers
{
    /// <summary>
    /// [deprecated] <see cref="ITagHelper"/> implementation for creating a reCAPTCHA version 2 Checkbox component. Use 
    /// </summary>
    [Obsolete("This TagHelper is obsolete and will be removed in a future version. Use <recaptcha type=\"V2Checkbox\" /> instead.")]
    [HtmlTargetElement("recaptcha-v2-checkbox", TagStructure = TagStructure.WithoutEndTag)]
    public class RecaptchaV2CheckboxTagHelper : RecaptchaTagHelperBase, IRecaptchaV2CheckboxTagHelper
    {
        /// <summary>
        /// [deprecated] An expression to be evaluated against the current model.
        /// </summary>
        [HtmlAttributeName(ObsoleteExpressionAttributeName)]
        [Obsolete("This property is obsolete and will be removed in a future version.")]
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
        /// The color theme of the widget.
        /// </summary>
        [HtmlAttributeName(ThemeAttributeName)]
        public ThemeType? Theme { get; set; }

        /// <summary>
        /// The size of the widget.
        /// </summary>
        [HtmlAttributeName(SizeAttributeName)]
        public SizeType? Size { get; set; }

        /// <summary>
        /// The tabindex of the widget and challenge.
        /// </summary>
        [HtmlAttributeName(TabIndexAttributeName)]
        public int? TabIndex { get; set; }

        /// <summary>
        /// Javascript function to be executed after a successful reCAPTCHA challenge response is recieved. The response token is passed as an argument.
        /// </summary>
        [HtmlAttributeName(CallbackAttributeName)]
        public string Callback { get; set; }

        /// <summary>
        /// Javascript function to be executed when the reCAPTCHA response expires.
        /// </summary>
        [HtmlAttributeName(ExpiredCallbackAttributeName)]
        public string ExpiredCallback { get; set; }

        /// <summary>
        /// Javascript function to be executed when the reCAPTCHA response encounters an error.
        /// </summary>
        [HtmlAttributeName(ErrorCallbackAttributeName)]
        public string ErrorCallback { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var tagHelper = new RecaptchaTagHelper()
            {
                Callback = Callback,
                ErrorCallback = ErrorCallback,
                ExpiredCallback = ExpiredCallback,
                For = For,
                SiteKey = SiteKey,
                Size = Size,
                TabIndex = TabIndex,
                Theme = Theme,
                Type = RecaptchaType.V2Checkbox,
                ViewContext = ViewContext
            };

            // The RecaptchaTagHelper produces the required Html output.
            tagHelper.Process(context, output);
        }
    }
}