using System;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Finoaker.Web.Recaptcha
{
    public static partial class HtmlHelperExtensions
    {
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
        /// <param name="isBadgeVisible">Controls visibility of the reCAPTCHA badge on the page.</param>
        /// <param name="badge">Controls position and visibility of the reCAPTCHA badge on the page.</param>
        /// <returns>A new <see cref="IHtmlContent"/> containing all Html elements and scripts required to render and execute reCAPTCHA component.</returns>
        [Obsolete("This HtmlHelper is obsolete and will be removed in a future version. Use @Html.RecaptchaFor instead.")]
        public static IHtmlContent RecaptchaV3For<TModel, TResult>(
            this IHtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TResult>> expression,
            string siteKey,
            string callback = null,
            string action = null,
            bool? isBadgeVisible = true,
            BadgeType? badge = null)
        {
            return RecaptchaV3(
                htmlHelper, 
                siteKey, 
                expression?.Name, 
                callback, 
                action, 
                isBadgeVisible, 
                badge);
        }

        /// <summary>
        /// <see cref="IHtmlHelper"/> implementation for creating a reCAPTCHA V3 component and binding the response to a model property.
        /// </summary>
        /// <param name="htmlHelper">The <see cref="IHtmlHelper"/> instance this method extends.</param>
        /// <param name="expression">An expression to be evaluated against the current model.</param>
        /// <param name="siteKey">Your reCAPTCHA sitekey. Get from the admin console.</param>
        /// <param name="callback">The name of your Javascript callback function, executed when the user submits a successful response. Response token will be passed to this function.</param>
        /// <param name="action">A name used to provide a detailed break-down of data for your top ten actions in the admin console.</param>
        /// <param name="isBadgeVisible">Controls visibility of the reCAPTCHA badge on the page.</param>
        /// <param name="badge">Controls position and visibility of the reCAPTCHA badge on the page.</param>
        /// <returns>A new <see cref="IHtmlContent"/> containing all Html elements and scripts required to render and execute reCAPTCHA component.</returns>
        [Obsolete("This HtmlHelper is obsolete and will be removed in a future version. Use @Html.Recaptcha instead.")]
        public static IHtmlContent RecaptchaV3<TModel>(
            this IHtmlHelper<TModel> htmlHelper,
            string siteKey,
            string expression = null,
            string callback = null,
            string action = null,
            bool? isBadgeVisible = true,
            BadgeType? badge = null)
        {
            if (htmlHelper is null)
            {
                throw new ArgumentNullException(nameof(htmlHelper));
            }

            // todo: 'IsBadgeVisible' property is obsolete and should be removed in a future version.
            if (isBadgeVisible.HasValue && !isBadgeVisible.Value)
            {
                // if IsBadgeVisible has been set to false then override Badge property
                badge = BadgeType.None;
            }

            TagBuilder input = null;

            if (!string.IsNullOrWhiteSpace(expression))
            {
                input = (TagBuilder)htmlHelper.Hidden(expression);
            }

            var props = new RecaptchaProps(
                RecaptchaType.V3,
                siteKey,
                callback: callback,
                badge: badge,
                action: action);

            return props.GenerateHtml(input);
        }
    }
}