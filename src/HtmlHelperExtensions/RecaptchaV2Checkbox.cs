using System;
using System.Reflection;
using System.IO;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Finoaker.Web.Recaptcha
{
    /// <summary>
    /// <see cref="IHtmlHelper"/> implementations for creating reCAPTCHA components.
    /// </summary>
    public static partial class HtmlHelperExtensions
    {
        internal const string RecaptchaV2ApiScript = "<script src=\"https://www.google.com/recaptcha/api.js\" type=\"text/javascript\" async defer></script>";
        internal const string EmbeddedV2ScriptFileName = "Scripts/dist/RecaptchaV2Checkbox.min.js";
        internal const string HiddenInputV2CssClass = "recaptcha-v2-response";
        internal const string ContainerV2CssClass = "recaptcha-v2-container";

        internal const string RecaptchaCssClassName = "g-recaptcha";
        internal const string CallbackFunctionName = "recaptchaResponseCallback";

        /// <summary>
        /// <see cref="IHtmlHelper"/> implementation for creating a reCAPTCHA V2 Checkbox style component and binding the response to a model property.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TResult">The type of the <paramref name="expression"/> result</typeparam>
        /// <param name="htmlHelper">The <see cref="IHtmlHelper"/> instance this method extends.</param>
        /// <param name="expression">An expression to be evaluated against the current model.</param>
        /// <param name="siteKey">Your reCAPTCHA sitekey. Get from the admin console.</param>
        /// <param name="theme">The color theme of the widget.</param>
        /// <param name="size">The size of the widget.</param>
        /// <param name="tabIndex">The tabindex of the widget and challenge. If other elements in your page use tabindex, it should be set to make user navigation easier.</param>
        /// <param name="callback">The name of your Javascript callback function, executed when the user submits a successful response. Response token will be passed to this function.</param>
        /// <param name="expiredCallback">The name of your callback function, executed when the reCAPTCHA response expires and the user needs to re-verify.</param>
        /// <param name="errorCallback">The name of your callback function, executed when reCAPTCHA encounters an error (usually network connectivity) and cannot continue until connectivity is restored. If you specify a function here, you are responsible for informing the user that they should retry.</param>
        /// <returns>Html and script content to render and execute the reCAPTCHA V2 Checkbox.</returns>
        public static IHtmlContent RecaptchaV2CheckboxFor<TModel, TResult>(
            this IHtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TResult>> expression,
            string siteKey,
            Theme? theme = null,
            Size? size = null,
            int? tabIndex = null,
            string callback = null,
            string expiredCallback = null,
            string errorCallback = null)
        {
            if (htmlHelper == null)
            {
                throw new ArgumentNullException(nameof(htmlHelper));
            }

            if (string.IsNullOrEmpty(siteKey))
            {
                throw new ArgumentNullException(nameof(siteKey));
            }

            var hiddenInputTag = (TagBuilder)htmlHelper.HiddenFor(expression, htmlAttributes: null);

            var container = new TagBuilder("div");
            container.AddCssClass(ContainerV2CssClass);

            container.InnerHtml.AppendHtml(GenerateHtmlContent(
                hiddenInputTag: hiddenInputTag,
                siteKey: siteKey,
                theme: theme,
                size: size,
                tabIndex: tabIndex,
                callback: callback,
                expiredCallback: expiredCallback,
                errorCallback: errorCallback)
                );

            return container;
        }

        /// <summary>
        /// <see cref="IHtmlHelper"/> implementation for creating a reCAPTCHA V2 Checkbox style component and binding the response to a model property.
        /// </summary>
        /// <param name="htmlHelper">The <see cref="IHtmlHelper"/> instance this method extends.</param>
        /// <param name="expression">An expression to be evaluated against the current model.</param>
        /// <param name="siteKey">Your reCAPTCHA sitekey. Get from the admin console.</param>
        /// <param name="theme">The color theme of the widget.</param>
        /// <param name="size">The size of the widget.</param>
        /// <param name="tabIndex">The tabindex of the widget and challenge. If other elements in your page use tabindex, it should be set to make user navigation easier.</param>
        /// <param name="callback">The name of your Javascript callback function, executed when the user submits a successful response. Response token will be passed to this function.</param>
        /// <param name="expiredCallback">The name of your callback function, executed when the reCAPTCHA response expires and the user needs to re-verify.</param>
        /// <param name="errorCallback">The name of your callback function, executed when reCAPTCHA encounters an error (usually network connectivity) and cannot continue until connectivity is restored. If you specify a function here, you are responsible for informing the user that they should retry.</param>
        /// <returns>Html and script content to render and execute the reCAPTCHA V2 Checkbox.</returns>
        public static IHtmlContent RecaptchaV2Checkbox(
            this IHtmlHelper htmlHelper,
            string siteKey,
            string expression = null,
            Theme? theme = null,
            Size? size = null,
            int? tabIndex = null,
            string callback = null,
            string expiredCallback = null,
            string errorCallback = null)
        {
            if (htmlHelper is null)
            {
                throw new ArgumentNullException(nameof(htmlHelper));
            }

            if (string.IsNullOrEmpty(siteKey))
            {
                throw new ArgumentNullException(nameof(siteKey));
            }
            var hiddenInputTag = (TagBuilder)htmlHelper.Hidden(expression ?? "recaptcha-v2--g-recaptcha");

            var container = new TagBuilder("div");
            container.AddCssClass(ContainerV2CssClass);

            container.InnerHtml.AppendHtml(GenerateHtmlContent(
                hiddenInputTag: hiddenInputTag,
                siteKey: siteKey,
                theme: theme,
                size: size,
                tabIndex: tabIndex,
                callback: callback,
                expiredCallback: expiredCallback,
                errorCallback: errorCallback)
                );

            return container;
        }

        internal static IHtmlContent GenerateHtmlContent(
            TagBuilder hiddenInputTag,
            string siteKey,
            Theme? theme,
            Size? size,
            int? tabIndex,
            string callback,
            string expiredCallback,
            string errorCallback)
        {
            hiddenInputTag.AddCssClass(HiddenInputV2CssClass);

            var htmlContentBuilder = new HtmlContentBuilder();

            var script = ResourceHelper
                .GetEmbeddedResource(EmbeddedV2ScriptFileName, typeof(HtmlHelperExtensions).GetTypeInfo().Assembly);

            if (script is null)
            {
                throw new FileNotFoundException("Embedded Javascript file not found.", EmbeddedV2ScriptFileName);
            }

            var tag = new TagBuilder("div");

            tag.AddCssClass(RecaptchaCssClassName);
            tag.Attributes.Add(RecaptchaAttributeNames.SiteKey, siteKey);
            tag.Attributes.Add(RecaptchaAttributeNames.Callback, CallbackFunctionName);

            if (theme.HasValue)
            {
                tag.Attributes.Add(RecaptchaAttributeNames.Theme, theme.Value.ToString().ToLower());
            }

            if (size.HasValue)
            {
                tag.Attributes.Add(RecaptchaAttributeNames.Size, size.Value.ToString().ToLower());
            }

            if (tabIndex.HasValue)
            {
                tag.Attributes.Add(RecaptchaAttributeNames.TabIndex, tabIndex.ToString());
            }

            if (!string.IsNullOrEmpty(callback))
            {
                // if a callback function is specified, add to the <input> element.
                hiddenInputTag.Attributes.Add(RecaptchaAttributeNames.Callback, callback);
            }

            if (!string.IsNullOrEmpty(expiredCallback))
            {
                tag.Attributes.Add(RecaptchaAttributeNames.ExpiredCallback, expiredCallback);
            }

            if (!string.IsNullOrEmpty(errorCallback))
            {
                tag.Attributes.Add(RecaptchaAttributeNames.ErrorCallback, errorCallback);
            }

            htmlContentBuilder
                .AppendHtml(RecaptchaV2ApiScript)
                .AppendHtml(tag)
                .AppendHtml(hiddenInputTag)
                .AppendHtml($"<script>{script}</script>");

            return htmlContentBuilder;
        }
    }
}