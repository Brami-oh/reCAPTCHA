using System;
using System.Reflection;
using System.IO;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Finoaker.Web.Recaptcha
{
    public static partial class HtmlHelperExtensions
    {
        internal const string RecaptchaV2ApiScript = "<script src=\"https://www.google.com/recaptcha/api.js\" type=\"text/javascript\" async defer></script>";
        internal const string EmbeddedV2ScriptFileName = "scripts/dist/RecaptchaV2Checkbox.min.js";
        internal const string HiddenInputV2CssClass = "recaptcha-v2-response";
        internal const string ContainerV2CssClass = "recaptcha-v2-container";

        internal const string RecaptchaCssClassName = "g-recaptcha";
        internal const string CallbackFunctionName = "recaptchaResponseCallback";
        

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