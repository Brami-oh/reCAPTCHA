using System;
using System.Linq.Expressions;
using System.Reflection;
using System.IO;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Finoaker.Web.Recaptcha
{
    public static partial class HtmlHelperExtensions
    {
        internal const string RecaptchaV3ApiScript = "<script src=\"https://www.google.com/recaptcha/api.js?render={0}\" type=\"text/javascript\"></script>";
        internal const string EmbeddedV3ScriptFilename = "scripts/dist/RecaptchaV3.min.js";
        internal const string HiddenInputV3CssClass = "recaptcha-v3-response";
        internal const string ContainerV3CssClass = "recaptcha-v3-container";

        internal const string DefaultAction = "Default";
        

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
        /// <param name="hideBadge">Indicates if the reCAPTCHA badge is visible. NOTE: You are allowed to hide the badge as long as you include the reCAPTCHA branding visibly in the user flow of the page.</param>
        /// <returns>A new <see cref="IHtmlContent"/> containing all Html elements & scripts required to render and execute reCAPTCHA component.</returns>
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

            var hiddenInputTag = (TagBuilder)htmlHelper.HiddenFor(expression, htmlAttributes: null);

            var container = new TagBuilder("div");
            container.AddCssClass(ContainerV3CssClass);

            container.InnerHtml.AppendHtml(GenerateHtmlContent(
                viewContext: htmlHelper.ViewContext,
                hiddenInputTag: hiddenInputTag,
                siteKey: siteKey,
                callback: callback,
                action: action,
                isBadgeVisible: isBadgeVisible));

            return container;
        }

        /// <summary>
        /// <see cref="IHtmlHelper"/> implementation for creating a reCAPTCHA V3 component and binding the response to a model property.
        /// </summary>
        /// <param name="htmlHelper">The <see cref="IHtmlHelper"/> instance this method extends.</param>
        /// <param name="expression">An expression to be evaluated against the current model.</param>
        /// <param name="siteKey">Your reCAPTCHA sitekey. Get from the admin console.</param>
        /// <param name="callback">The name of your Javascript callback function, executed when the user submits a successful response. Response token will be passed to this function.</param>
        /// <param name="action">A name used to provide a detailed break-down of data for your top ten actions in the admin console.</param>
        /// <param name="hideBadge">Indicates if the reCAPTCHA badge is visible. NOTE: You are allowed to hide the badge as long as you include the reCAPTCHA branding visibly in the user flow of the page.</param>
        /// <returns>A new <see cref="IHtmlContent"/> containing all Html elements & scripts required to render and execute reCAPTCHA component.</returns>
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

            var hiddenInputTag = (TagBuilder)htmlHelper.Hidden(expression ?? "recaptcha-v3--g-recaptcha");

            var container = new TagBuilder("div");
            container.AddCssClass(ContainerV3CssClass);

            container.InnerHtml.AppendHtml(GenerateHtmlContent(
                viewContext: htmlHelper.ViewContext,
                hiddenInputTag: hiddenInputTag,
                siteKey: siteKey,
                callback: callback,
                action: action,
                isBadgeVisible: isBadgeVisible));

            return container;
        }

        internal static IHtmlContent GenerateHtmlContent(
            ViewContext viewContext,
            TagBuilder hiddenInputTag,
            string siteKey,
            string callback,
            string action,
            bool isBadgeVisible)
        {
            if (string.IsNullOrEmpty(action))
            {
                // If action attribute not assigned, use Action property of ViewBag otherwise use controller action name.
                action = viewContext.ViewBag?.Action ?? viewContext.RouteData.Values["action"].ToString() ?? DefaultAction;
            }

            hiddenInputTag.Attributes.Add(RecaptchaAttributeNames.Action, action);
            hiddenInputTag.Attributes.Add(RecaptchaAttributeNames.SiteKey, siteKey);
            hiddenInputTag.AddCssClass(HiddenInputV3CssClass);

            if (!string.IsNullOrEmpty(callback))
            {
                // add the data-callback attribute to the <input> element
                hiddenInputTag.Attributes.Add(RecaptchaAttributeNames.Callback, callback);
            }

            var script = ResourceHelper
                .GetEmbeddedResource(EmbeddedV3ScriptFilename, typeof(HtmlHelperExtensions).GetTypeInfo().Assembly);

            if (script is null)
            {
                throw new FileNotFoundException("Embedded Javascript file not found.", EmbeddedV3ScriptFilename);
            }

            var htmlContentBuilder = new HtmlContentBuilder();

            htmlContentBuilder
                .AppendHtml($"<style>.grecaptcha-badge{{visibility:{(isBadgeVisible ? "visible" : "hidden")};}}.{ContainerV3CssClass}{{display:none;}}</style>")
                .AppendFormat(RecaptchaV3ApiScript, siteKey)
                .AppendHtml(hiddenInputTag)
                .AppendHtml($"<script>{script}</script>");

            return htmlContentBuilder;
        }
    }
}