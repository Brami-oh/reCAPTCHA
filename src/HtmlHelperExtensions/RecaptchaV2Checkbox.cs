using System;
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
        [Obsolete("This HtmlHelper is obsolete and will be removed in a future version. Use @Html.RecaptchaFor instead.")]
        public static IHtmlContent RecaptchaV2CheckboxFor<TModel, TResult>(
            this IHtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TResult>> expression,
            string siteKey,
            ThemeType? theme = null,
            SizeType? size = null,
            int? tabIndex = null,
            string callback = null,
            string expiredCallback = null,
            string errorCallback = null)
        {
            return RecaptchaV2Checkbox(
                htmlHelper,
                siteKey,
                expression?.Name,
                theme, 
                size, 
                tabIndex, 
                callback, 
                expiredCallback, 
                errorCallback);

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
        [Obsolete("This HtmlHelper is obsolete and will be removed in a future version. Use @Html.Recaptcha instead.")]
        public static IHtmlContent RecaptchaV2Checkbox<TModel>(
            this IHtmlHelper<TModel> htmlHelper,
            string siteKey,
            string expression = null,
            ThemeType? theme = null,
            SizeType? size = null,
            int? tabIndex = null,
            string callback = null,
            string expiredCallback = null,
            string errorCallback = null)
        {
            if (htmlHelper is null)
            {
                throw new ArgumentNullException(nameof(htmlHelper));
            }

            TagBuilder input = null;

            if (!string.IsNullOrWhiteSpace(expression))
            {
                // Generate an <input> element using Model or Id property details. 
                input = (TagBuilder)htmlHelper.Hidden(expression);
            }

            var props = new RecaptchaProps(
                RecaptchaType.V2Checkbox,
                siteKey,
                callback: callback,
                expiredCallback: expiredCallback,
                errorCallback: errorCallback,
                theme: theme,
                tabIndex: tabIndex,
                size: size);

            return props.GenerateHtml(input);
        }
    }
}