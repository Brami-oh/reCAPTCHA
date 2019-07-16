using System;
using System.Linq.Expressions;
using System.Reflection;
using System.IO;
using System.Collections.Generic;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Finoaker.Web.Recaptcha
{
    public static partial class HtmlHelperExtensions
    {
        internal const string EmbeddedV3ScriptFileName = "Scripts/dist/RecaptchaV3.min.js";
        private const string DefaultAction = "Default";
        private const string HiddenInputCssClass = "recaptcha-response";

        /// <summary>
        /// <see cref="IHtmlHelper"/> implementation for creating a reCAPTCHA V3 component and binding the response to a model property.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TResult">The type of the <paramref name="expression"/> result</typeparam>
        /// <param name="htmlHelper">The <see cref="IHtmlHelper"/> instance this method extends.</param>
        /// <param name="expression">An expression to be evaluated against the current model.</param>
        /// <param name="siteKey">Your reCAPTCHA sitekey. Get from the admin console.</param>
        /// <param name="callback">The name of your Javascript callback function, executed when the user submits a successful response. Response token will be passed to this function.</param>
        /// <param name="action">A name used to provide a detailed break-down of data for your top ten actions in the admin console.</param>
        /// <param name="isBadgeVisible">Indicates if the reCAPTCHA badge is visible. NOTE: You are allowed to hide the badge as long as you include the reCAPTCHA branding visibly in the user flow of the page.</param>
        /// <returns>A new <see cref="IHtmlContent"/> containing all Html elements and scripts required to render and execute reCAPTCHA component.</returns>
        public static IHtmlContent RecaptchaV3For<TModel, TResult>(
            this IHtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TResult>> expression,
            string siteKey,
            string callback = null,
            string action = null,
            bool isBadgeVisible = true)
        {
            if (htmlHelper is null)
            {
                throw new ArgumentNullException(nameof(htmlHelper));
            }

            if (string.IsNullOrEmpty(siteKey))
            {
                throw new ArgumentNullException(nameof(siteKey));
            }

            if (string.IsNullOrEmpty(action))
            {
                // Try getting reCAPTCHA action from various sources or assign default value.
                action = HtmlHelperExtensions.GetAction(htmlHelper.ViewContext);
            }

            var hiddenInputAttributes = GenerateV3Attributes(
                sitekey: siteKey,
                action: action,
                callback: callback,
                isBadgeVisible: isBadgeVisible);

            var hiddenInputTag = htmlHelper.HiddenFor(expression, hiddenInputAttributes);

            return new HtmlContentBuilder()
                .AppendHtml(hiddenInputTag)
                .AppendHtml(GenerateScriptTag(expression.Name));
        }

        /// <summary>
        /// <see cref="IHtmlHelper"/> implementation for creating a reCAPTCHA V3 component and binding the response to a model property.
        /// </summary>
        /// <param name="htmlHelper">The <see cref="IHtmlHelper"/> instance this method extends.</param>
        /// <param name="expression">An expression to be evaluated against the current model.</param>
        /// <param name="siteKey">Your reCAPTCHA sitekey. Get from the admin console.</param>
        /// <param name="callback">The name of your Javascript callback function, executed when the user submits a successful response. Response token will be passed to this function.</param>
        /// <param name="action">A name used to provide a detailed break-down of data for your top ten actions in the admin console.</param>
        /// <param name="isBadgeVisible">Indicates if the reCAPTCHA badge is visible. NOTE: You are allowed to hide the badge as long as you include the reCAPTCHA branding visibly in the user flow of the page.</param>
        /// <returns>A new <see cref="IHtmlContent"/> containing all Html elements and scripts required to render and execute reCAPTCHA component.</returns>
        public static IHtmlContent RecaptchaV3(
            this IHtmlHelper htmlHelper,
            string siteKey,
            string expression = null,
            string callback = null,
            string action = null,
            bool isBadgeVisible = true)
        {
            if (htmlHelper is null)
            {
                throw new ArgumentNullException(nameof(htmlHelper));
            }

            if (string.IsNullOrEmpty(siteKey))
            {
                throw new ArgumentNullException(nameof(siteKey));
            }

            if (string.IsNullOrEmpty(action))
            {
                // Try getting reCAPTCHA action from various sources or assign default value.
                action =  GetAction(htmlHelper.ViewContext);
            }

            // set the default if not set
            var expressionName = expression ?? "recaptcha-v3--g-recaptcha";

            var hiddenInputAttributes = GenerateV3Attributes(
                sitekey: siteKey,
                action: action,
                callback: callback,
                isBadgeVisible: isBadgeVisible);

            var hiddenInputTag = htmlHelper.Hidden(
                expression: expressionName,
                value: null,
                htmlAttributes: hiddenInputAttributes);

            return new HtmlContentBuilder()
                .AppendHtml(hiddenInputTag)
                .AppendHtml(GenerateScriptTag(expressionName));
        }

        internal static Dictionary<string, object> GenerateV3Attributes(
            string sitekey,
            string callback = null,
            string action = null,
            bool? isBadgeVisible = null)
        {
            var htmlAttributes = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
            {
                { RecaptchaAttributeNames.SiteKey, sitekey },
                { "class", HiddenInputCssClass },
            };

            if (!string.IsNullOrEmpty(callback))
            {
                htmlAttributes.Add(RecaptchaAttributeNames.Callback, callback);
            }

            if (!string.IsNullOrEmpty(action))
            {
                htmlAttributes.Add(RecaptchaAttributeNames.Action, action);
            }

            if (isBadgeVisible.HasValue)
            {
                htmlAttributes.Add(RecaptchaAttributeNames.BadgeVisible, isBadgeVisible);
            }

            return htmlAttributes;
        }

        internal static string GetAction(ViewContext viewContext)
        {
            // If action attribute not assigned, try ViewData "Action" property 
            // or controller action name otherwise assign a default value.
            return viewContext.ViewData["Action"]?.ToString() ??
                viewContext.ViewData["action"]?.ToString() ??
                (string)viewContext.ViewBag?.Action ??
                (string)viewContext.ViewBag?.action ??
                viewContext.RouteData.Values["action"]?.ToString() ??
                DefaultAction;
        }

        internal static TagBuilder GenerateScriptTag(string id)
        {
            // get the script file content from embedded resources
            var script = ResourceHelper
                .GetEmbeddedResource(
                    EmbeddedV3ScriptFileName,
                    typeof(HtmlHelperExtensions).GetTypeInfo().Assembly);

            if (script is null)
            {
                throw new FileNotFoundException(
                    "Embedded Javascript file not found.",
                    EmbeddedV3ScriptFileName);
            }

            script = $"var recaptchaInputId=\"{id}\";{script}";

            var scriptTag = new TagBuilder("script");

            scriptTag.InnerHtml.AppendHtml(script);

            return scriptTag;
        }
    }
}