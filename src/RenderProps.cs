using System;
using System.Runtime.Serialization;
using Finoaker.Web.Core;

namespace Finoaker.Web.Recaptcha
{
    /// <summary>
    /// Properties used to render a reCAPTCHA component using client side script. Instance is serlialized into Json.
    /// </summary>
    /// <exception cref="ArgumentException">Thrown if the <see cref="SiteKey"/> is invalid.</exception>
    [DataContract]
    internal class RenderProps
    {
        public RenderProps(
            RecaptchaType type,
            string siteKey,
            ThemeType? theme = null,
            int? tabIndex = null,
            SizeType? size = null,
            BadgeType? badge = null, 
            string action = null)
        {
            SiteKey = siteKey;
            Type = type;
            Theme = theme;
            TabIndex = tabIndex;
            Size = size;
            Badge = badge;
            Action = action;
        }

        /// <summary>
        /// Type of reCAPTCHA component. Only used to control output of other properties.
        /// </summary>
        [IgnoreDataMember]
        private RecaptchaType Type { get; }

        /// <summary>
        /// Your sites unique reCAPTCHA site key. This is the only required property and must be 40 characters long.
        /// </summary>
        [DataMember(Name = "sitekey", IsRequired = true)]
        public string SiteKey {

            get
            {
                return _siteKey;
            }

            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentNullException(nameof(SiteKey));
                }

                if (value.Trim().Length != 40)
                {
                    throw new ArgumentException("Site Key must be 40 characters long.", nameof(SiteKey));
                }
                _siteKey = value.Trim();
            }
        }
        private string _siteKey;

        [DataMember(Name = "callback")]
        public readonly string Callback = "f_recaptch__onexecute";

        [DataMember(Name = "expired-callback")]
        public string ExpiredCallback
        {
            get
            {
                return Type == RecaptchaType.V2Checkbox ? "f_recaptch__onexpired" : null;
            }
            private set { }
        }

        [DataMember(Name = "error-callback")]
        public string ErrorCallback
        {
            get
            {
                return Type == RecaptchaType.V2Checkbox ? "f_recaptch__onerror" : null;
            }
            private set { }
        }

        [DataMember(Name = "tabindex")]
        public int? TabIndex
        {
            get
            {
                return Type == RecaptchaType.V2Checkbox ? _tabIndex : null;
            }
            private set
            {
                _tabIndex = value;
            }
        }
        private int? _tabIndex;

        [DataMember(Name = "badge")]
        public object Badge
        {
            get
            {
                return (Type == RecaptchaType.V3 ? _badge : null)?.ToString().ToLower();
            }
            private set
            {
                _badge = value != null && value.GetType() == typeof(BadgeType) ? (BadgeType)value : _badge;
            }
        }
        private BadgeType? _badge;

        [DataMember(Name = "theme")]
        public object Theme
        {
            get
            {
                return (Type == RecaptchaType.V2Checkbox ? _theme : null)?.ToString().ToLower();
            }
            private set
            {
                _theme = value != null && value.GetType() == typeof(ThemeType) ? (ThemeType)value : _theme;
            }
        }
        private ThemeType? _theme;

        [DataMember(Name = "size")]
        public object Size
        {
            get
            {
                return (Type == RecaptchaType.V3 ? SizeType.Invisible : _size)?.ToString().ToLower();
            }
            private set
            {
                _size = value != null && value.GetType() == typeof(SizeType) ? (SizeType)value : _size;
            }
        }
        private SizeType? _size;

        [DataMember(Name = "action")]
        public string Action
        {
            get
            {
                return Type == RecaptchaType.V3 ? _action : null;
            }
            private set
            {
                _action = value.TrimToNull();
            }
        }
        private string _action;
    }
}
