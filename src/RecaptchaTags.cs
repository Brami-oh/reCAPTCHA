using Microsoft.AspNetCore.Mvc.Rendering;

namespace Finoaker.Web.Recaptcha
{
    public class RecaptchaTags
    {
        public TagBuilder HiddenInputTag { get; set; }
        public TagBuilder ScriptTag { get; set; }
    }
}
