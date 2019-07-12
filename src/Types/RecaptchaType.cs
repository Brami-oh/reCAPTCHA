using System;

namespace Finoaker.Web.Recaptcha
{
    /// <summary>
    /// The type of reCAPTCHA component.
    /// </summary>
    public enum RecaptchaType
    {
        /// <summary>
        /// The "I'm not a robot" Checkbox requires the user to click a checkbox indicating the user is not a robot.
        /// This will either pass the user immediately (with No CAPTCHA) or challenge them to validate whether or not they are human. 
        /// This is the simplest option to integrate with and only requires two lines of HTML to render the checkbox.
        /// </summary>
        V2Checkbox,
        /// <summary>
        /// reCAPTCHA v3 allows you to verify if an interaction is legitimate without any user interaction. 
        /// It is a pure JavaScript API returning a score, giving you the ability to take action in the context of your site:
        /// for instance requiring additional factors of authentication, sending a post to moderation, or throttling bots that may be scraping content.
        /// </summary>
        V3
    }
}
