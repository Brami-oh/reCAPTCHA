namespace Finoaker.Web.Recaptcha
{
    /// <summary>
    /// Where to position reCAPTCHA badge on the page.
    /// </summary>
    public enum BadgeType
    {
        /// <summary>
        /// (Default) Positions badge in the bottom right corner of the page.
        /// </summary>
        BottomRight,

        /// <summary>
        /// Positions badge in the bottom left corner of the page.
        /// </summary>
        BottomLeft,

        /// <summary>
        /// Lets you position the badge with CSS.
        /// </summary>
        Inline,

        /// <summary>
        /// Badge will NOT be displayed (hidden).
        /// </summary>
        /// <remarks>Please consult the reCAPTCHA Terms and Conditions before using this option.</remarks>
        None
    }
}
