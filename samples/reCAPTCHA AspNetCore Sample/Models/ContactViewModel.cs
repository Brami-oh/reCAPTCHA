using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace reCAPTCHA_AspNetCore_Sample.Models
{
    public class ContactViewModel
    {
        [DisplayName("Name (required)"), Required]
        public string Name { get; set; }

        [DisplayName("Email (required)"), Required, EmailAddress]
        public string Email { get; set; }

        [DisplayName("Phone"), Phone]
        public string Phone { get; set; }

        [DisplayName("Message (required)"), Required]
        public string Message { get; set; }

        [Required]
        public string RecaptchaResponse { get; set; }
    }
}
