using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace SampleCode
{
    public class SampleModel
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
