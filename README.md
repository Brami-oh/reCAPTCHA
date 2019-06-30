# reCAPTCHA for ASP.NET (.NET Standard 2.0) 
The Finoaker reCAPTCHA library makes it super easy to add reCAPTCHA functionalty to you ASP.NET sites! Simply add your keys to the settings, drop the reCAPTCHA TagHelper or HtmlHelper onto your page and your ready to go.

Finoaker reCAPTCHA also makes server side verification simpler by binding the client side reCAPTCHA response token to MVC model so it integrates seamlessly with the standard ASP.NET form workflow. 

## About reCAPTCHA
reCAPTCHA is a free service that protects your website from spam and abuse. reCAPTCHA uses an advanced risk analysis engine and adaptive challenges to keep automated software from engaging in abusive activities on your site. It does this while letting your valid users pass through with ease.

For more information visit the reCAPTCHA site: https://www.google.com/recaptcha

## Get Started
Check out the DEMO first to see reCAPTCHA in action and for detailed examples: https://finoaker-recaptcha.azurewebsites.net/

### Installation
```
PM> Install-Package Finoaker.Web.Recaptcha
```
### Settings
For AspNetCore apps add reCAPTCHA keys to the appSettings.json file (could also use UserSecrets).

```json
"RecaptchaSettings": {
  "Keys": [
    {
      "SecretKey": "<v3-secret-key>",
      "SiteKey": "<v3-site-key>",
      "KeyType": "V3"
    },
    {
      "SecretKey": "<v2-secret-key>",
      "SiteKey": "<v2-site-key>",
      "KeyType": "V2Checkbox"
    }
  ]
}
```

### Import Settings
In the apps Startup class, add the reCAPTCHA service to the service collection, in the ConfigureServices(IServiceCollection services) method. Pass in the RecaptchaSettings section.

```csharp
// This method gets called by the runtime. Use this method to add services to the container.
public void ConfigureServices(IServiceCollection services)
{
    services.Configure<CookiePolicyOptions>(options =>
    {
        // This lambda determines whether user consent for non-essential cookies is needed for a given request.
        options.CheckConsentNeeded = context => true;
        options.MinimumSameSitePolicy = SameSiteMode.None;
    });
 
    // Add reCAPTCHA settings.
    services.AddRecaptcha(Configuration.GetSection(nameof(RecaptchaSettings)));
 
    services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
}
```

### Model
Add a property to your model to hold the reCAPTCHA response, so it can be posted back for verification.

```csharp
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
```

### View
Add your model and TagHelper reference to the top of the view page and then drop the Finoaker reCAPTCHA TagHelper OR HtmlHelper onto your form. Add the property name from your model.

```html
@using Finoaker.Web.Recaptcha
@model ContactViewModel
@addTagHelper *, Finoaker.Web.Recaptcha
 
<!DOCTYPE html>
<html>
<head>
    <meta name="viewport" content="width=device-width" />
    <title>RecaptchaDemo</title>
</head>
<body>
    <h1>Contact Us</h1>
    <form asp-controller="Home" asp-action="RecaptchaV2CheckboxDemo" method="post">
        @Html.AntiForgeryToken()
 
        <div class="form-field-container">
            <label asp-for="Name"></label>
            <input asp-for="Name" type="text" />
        </div>
 
        <div class="form-field-container">
            <label asp-for="Email"></label>
            <input asp-for="Email" type="email" />
        </div>
 
        <div class="form-field-container">
            <label asp-for="Phone"></label>
            <input asp-for="Phone" type="tel" />
        </div>
 
        <div class="form-field-container">
            <label asp-for="Message"></label>
            <input asp-for="Message" type="tel" />
        </div>
 
        <div class="form-field-container">
 
            <!-- Use either the TagHelper reCAPTCHA version... -->
            <recaptcha-v2-checkbox asp-for="RecaptchaResponse" />
 
            <!-- ...OR the HtmlHelper reCAPTCHA version -->
            @Html.RecaptchaV2CheckboxFor(m => m.RecaptchaResponse)
 
        </div>
 
        <div class="form-field-container">
            <input type="submit" />
        </div>
 
    </form>
</body>
</html>
```

### Controller
In your controller action, add the Model and Recaptcha services as parameters. Check model state as normal, then call the verification service. Check the results for verifcation success.

```csharp
[HttpGet]
public IActionResult RecaptchaV2CheckboxDemo()
{
    return View();
}
 
[HttpPost, ValidateAntiForgeryToken]
public IActionResult RecaptchaV2CheckboxDemo(ContactViewModel model, [FromServices] IRecaptchaService recaptchaService)
{
    // check model state first to confirm all properties, including reCAPTCHA, are valid.
    if (ModelState.IsValid)
    {
        // verify the reCAPCTHA response against verification service
        var verifyResult = recaptchaService.VerifyAsync(model.RecaptchaResponse, RecaptchaType.V2Checkbox).Result;
 
        // check success status
        if (verifyResult.Success)
        {
            ...
        }
    }
    return View();
}
```
