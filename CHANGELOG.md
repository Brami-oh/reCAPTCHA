## 1.1.0 (2019-07-29)

New Features:
	- Simplified TagHelpers by combining into one <recaptcha> tag and adding a "type" attribute
	- Made HtmlHelpers simpler by combining into @Html.Recaptcha() & @Html.RecaptchaFor() extension methods and adding "type" argument
	- Html output inside a single <div> container with class name "f-recaptcha-container"
	- Added basic TestApp for testing and basic demo.

Improvements:
	- Major code refactoring to simplify.
	- Deprecated <recaptcha-v2checkbox> & <recaptcha-v3> TagHelpers in favour of new <recaptcha> version.
	- Deprecated @Html.RecaptchaV2Checkbox & @Html.RecaptchaV3 extension methods in favour of new @Html.Recaptcha() & @Html.RecaptchaFor() versions
	- Moved Sample App into a separate solution.

## 1.0.1 (2019-07-12)

New Features:
    - Documented the library API so it's even easier to use.

Improvements:
    - Deprecated use of AspNetCore service for executing server side verificartion and replaced with static methods.
	- Removed dependency on TypeScript for library and sample projects.
	- Added a dialog the sample app that displays verification results after submission.

Bugfixes:
    - Removed redundant 'partial' from 'Helper' class.
	- Changed RecaptchaAttributeNames class the internal.

## 1.0.0 (2019-06-30)

    - Initial release.
