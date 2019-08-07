using System;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Finoaker.Web.Recaptcha
{
    /// <summary>
    /// <see cref="IHtmlHelper"/> implementations for creating reCAPTCHA components.
    /// </summary>
    public static class HtmlHelpers
    {
        //private const string DefaultAction = "DefaultAction";

        /// <summary>
        /// <see cref="IHtmlHelper"/> implementation for creating reCAPTCHA components.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="htmlHelper">The <see cref="IHtmlHelper"/> instance this method extends.</param>
        /// <param name="expression">An expression to be evaluated against the current model.</param>
        /// <param name="type">Type of reCAPTCHA component to be created.</param>
        /// <param name="settings">reCAPTCHA settings from configuration that contain a Site Key of the required type. Get keys from the admin console.</param>
        /// <param name="theme">The color theme of the widget.</param>
        /// <param name="size">The size of the widget.</param>
        /// <param name="tabIndex">The tabindex of the widget and challenge. If other elements in your page use tabindex, it should be set to make user navigation easier.</param>
        /// <param name="callback">The name of your Javascript callback function, executed when the user submits a successful response. Response token will be passed to this function.</param>
        /// <param name="expiredCallback">The name of your callback function, executed when the reCAPTCHA response expires and the user needs to re-verify.</param>
        /// <param name="errorCallback">The name of your callback function, executed when reCAPTCHA encounters an error (usually network connectivity) and cannot continue until connectivity is restored. If you specify a function here, you are responsible for informing the user that they should retry.</param>
        /// <param name="action">A name used to provide a detailed break-down of data for your top ten actions in the admin console.</param>
        /// <param name="badge">Controls position and visibility of the reCAPTCHA badge on the page.</param>
        /// <returns>Html and script content to render and execute the reCAPTCHA component.</returns>
        public static IHtmlContent Recaptcha<TModel>(
            this IHtmlHelper<TModel> htmlHelper,
            string expression,
            RecaptchaType type,
            RecaptchaSettings settings,
            ThemeType? theme = null,
            SizeType? size = null,
            int? tabIndex = null,
            string callback = null,
            string expiredCallback = null,
            string errorCallback = null,
            string action = null,
            BadgeType? badge = null)
        {
            if (settings is null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            return Recaptcha(
                htmlHelper,
                expression,
                type,
                settings.First(type)?.SiteKey,
                theme,
                size,
                tabIndex,
                callback,
                expiredCallback,
                errorCallback,
                action,
                badge);
        }

        /// <summary>
        /// <see cref="IHtmlHelper"/> implementation for creating reCAPTCHA components.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="htmlHelper">The <see cref="IHtmlHelper"/> instance this method extends.</param>
        /// <param name="expression">An expression to be evaluated against the current model.</param>
        /// <param name="type">Type of reCAPTCHA component to be created.</param>
        /// <param name="siteKey">Site Key of the required type. Get keys from the admin console.</param>
        /// <param name="theme">The color theme of the widget.</param>
        /// <param name="size">The size of the widget.</param>
        /// <param name="tabIndex">The tabindex of the widget and challenge. If other elements in your page use tabindex, it should be set to make user navigation easier.</param>
        /// <param name="callback">The name of your Javascript callback function, executed when the user submits a successful response. Response token will be passed to this function.</param>
        /// <param name="expiredCallback">The name of your callback function, executed when the reCAPTCHA response expires and the user needs to re-verify.</param>
        /// <param name="errorCallback">The name of your callback function, executed when reCAPTCHA encounters an error (usually network connectivity) and cannot continue until connectivity is restored. If you specify a function here, you are responsible for informing the user that they should retry.</param>
        /// <param name="action">A name used to provide a detailed break-down of data for your top ten actions in the admin console.</param>
        /// <param name="badge">Controls position and visibility of the reCAPTCHA badge on the page.</param>
        /// <returns>Html and script content to render and execute the reCAPTCHA component.</returns>
        public static IHtmlContent Recaptcha<TModel>(
            this IHtmlHelper<TModel> htmlHelper,
            string expression,
            RecaptchaType type,
            string siteKey,
            ThemeType? theme = null,
            SizeType? size = null,
            int? tabIndex = null,
            string callback = null,
            string expiredCallback = null,
            string errorCallback = null,
            string action = null,
            BadgeType? badge = null)
        {
            if (htmlHelper is null)
            {
                throw new ArgumentNullException(nameof(htmlHelper));
            }

            TagBuilder input = null;

            if (!string.IsNullOrWhiteSpace(expression))
            {
                input = (TagBuilder)htmlHelper.Hidden(expression);
            }

            var props = new RecaptchaProps(
                type,
                siteKey,
                callback: callback,
                expiredCallback: expiredCallback,
                errorCallback: errorCallback,
                theme: theme,
                tabIndex: tabIndex,
                size: size,
                badge: badge,
                action: action);

            return props.GenerateHtml(input);
        }

        /// <summary>
        /// <see cref="IHtmlHelper"/> implementation for creating reCAPTCHA components and binding to a model property.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TResult">The type of the <paramref name="expression"/> result</typeparam>
        /// <param name="htmlHelper">The <see cref="IHtmlHelper"/> instance this method extends.</param>
        /// <param name="expression">An expression to be evaluated against the current model.</param>
        /// <param name="type">Type of reCAPTCHA component to be created.</param>
        /// <param name="settings">reCAPTCHA settings from configuration that contain a site key of the required type. Get keys from the admin console.</param>
        /// <param name="theme">The color theme of the widget.</param>
        /// <param name="size">The size of the widget.</param>
        /// <param name="tabIndex">The tabindex of the widget and challenge. If other elements in your page use tabindex, it should be set to make user navigation easier.</param>
        /// <param name="callback">The name of your Javascript callback function, executed when the user submits a successful response. Response token will be passed to this function.</param>
        /// <param name="expiredCallback">The name of your callback function, executed when the reCAPTCHA response expires and the user needs to re-verify.</param>
        /// <param name="errorCallback">The name of your callback function, executed when reCAPTCHA encounters an error (usually network connectivity) and cannot continue until connectivity is restored. If you specify a function here, you are responsible for informing the user that they should retry.</param>
        /// <param name="action">A name used to provide a detailed break-down of data for your top ten actions in the admin console.</param>
        /// <param name="badge">Controls position and visibility of the reCAPTCHA badge on the page.</param>
        /// <returns>Html and script content to render and execute the reCAPTCHA component.</returns>
        public static IHtmlContent RecaptchaFor<TModel, TResult>(
            this IHtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TResult>> expression,
            RecaptchaType type,
            RecaptchaSettings settings,
            ThemeType? theme = null,
            SizeType? size = null,
            int? tabIndex = null,
            string callback = null,
            string expiredCallback = null,
            string errorCallback = null,
            string action = null,
            BadgeType? badge = null)
        {
            if (settings is null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            return RecaptchaFor(
                htmlHelper,
                expression,
                type,
                settings.First(type)?.SiteKey,
                theme,
                size,
                tabIndex,
                callback,
                expiredCallback,
                errorCallback,
                action,
                badge);
        }

        /// <summary>
        /// <see cref="IHtmlHelper"/> implementation for creating reCAPTCHA components and binding to a model property.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TResult">The type of the <paramref name="expression"/> result</typeparam>
        /// <param name="htmlHelper">The <see cref="IHtmlHelper"/> instance this method extends.</param>
        /// <param name="expression">An expression to be evaluated against the current model.</param>
        /// <param name="type">Type of reCAPTCHA component to be created.</param>
        /// <param name="siteKey">Site Key of the required type. Get keys from the admin console.</param>
        /// <param name="theme">The color theme of the widget.</param>
        /// <param name="size">The size of the widget.</param>
        /// <param name="tabIndex">The tabindex of the widget and challenge. If other elements in your page use tabindex, it should be set to make user navigation easier.</param>
        /// <param name="callback">The name of your Javascript callback function, executed when the user submits a successful response. Response token will be passed to this function.</param>
        /// <param name="expiredCallback">The name of your callback function, executed when the reCAPTCHA response expires and the user needs to re-verify.</param>
        /// <param name="errorCallback">The name of your callback function, executed when reCAPTCHA encounters an error (usually network connectivity) and cannot continue until connectivity is restored. If you specify a function here, you are responsible for informing the user that they should retry.</param>
        /// <param name="action">A name used to provide a detailed break-down of data for your top ten actions in the admin console.</param>
        /// <param name="badge">Controls position and visibility of the reCAPTCHA badge on the page.</param>
        /// <returns>Html and script content to render and execute the reCAPTCHA component.</returns>
        public static IHtmlContent RecaptchaFor<TModel, TResult>(
            this IHtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TResult>> expression,
            RecaptchaType type,
            string siteKey,
            ThemeType? theme = null,
            SizeType? size = null,
            int? tabIndex = null,
            string callback = null,
            string expiredCallback = null,
            string errorCallback = null,
            string action = null,
            BadgeType? badge = null)
        {
            if (htmlHelper is null)
            {
                throw new ArgumentNullException(nameof(htmlHelper));
            }

            TagBuilder input = null;

            if (expression != null)
            {
                input = (TagBuilder)htmlHelper.HiddenFor(expression);
            }

            var props = new RecaptchaProps(
                type,
                siteKey,
                callback: callback,
                expiredCallback: expiredCallback,
                errorCallback: errorCallback,
                theme: theme,
                tabIndex: tabIndex,
                size: size,
                badge: badge,
                action: action);

            return props.GenerateHtml(input);
        }
    }
}
