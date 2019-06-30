using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace Finoaker.Web.Recaptcha
{
    [Serializable]
    public class RecaptchaSettings
    {
        [Required]
        public List<Key> Keys { get; set; }

        /// <summary>
        /// Returns the first <see cref="Key"/> of the sequence that matches <see cref="RecaptchaType"/> or null if no such element is found.
        /// </summary>
        public Key First(RecaptchaType type)
        {
            return Keys?.FirstOrDefault(key => key.KeyType == type);
        }
    }
    [Serializable]
    public class Key
    {
        /// <summary>
        /// reCAPTCHA Secret Key
        /// </summary>
        [Required]
        [StringLength(40, MinimumLength = 40)]
        public string SecretKey { get; set; }

        /// <summary>
        /// reCAPTCHA Site Key
        /// </summary>
        [StringLength(40, MinimumLength = 40)]
        public string SiteKey { get; set; }

        ///// <summary>
        ///// Type of reCAPTCHA service the keys can be used with (eg. v2 checkbox)
        ///// </summary>
        [Required]
        public RecaptchaType KeyType { get; set; }
    }
}
