﻿// grecaptcha object is declared in reCAPTCHA api.
declare const grecaptcha: any;

// id of the tag that will hold the token response.
declare const recaptchaInputId: string;

(function() {
    // the reCAPTCHA <input> form element
    const hiddenInputTag = <HTMLInputElement>document.querySelector(`body input[type=hidden]#${recaptchaInputId}`);

    if (hiddenInputTag && hiddenInputTag.dataset.sitekey) {
        // insert the reCAPTCHA script
        let scriptTag = document.createElement(`script`);

        scriptTag.type = `text/javascript`;
        scriptTag.src = `https://www.google.com/recaptcha/api.js?onload=onloadRecaptchaResponse&render=${hiddenInputTag.dataset.sitekey}`;

        document.body.appendChild(scriptTag);
    }
})();

function onloadRecaptchaResponse() {
    // the reCAPTCHA form hidden <input> element
    const hiddenInputTag = <HTMLInputElement>document.querySelector(`body input[type=hidden]#${recaptchaInputId}`);

    if (grecaptcha &&
        typeof grecaptcha.ready == `function`) {

        grecaptcha.ready(() => {
            if (hiddenInputTag &&
                hiddenInputTag.dataset.sitekey &&
                hiddenInputTag.dataset.action) {
                grecaptcha
                    .execute(hiddenInputTag.dataset.sitekey, {
                        action: hiddenInputTag.dataset.action
                    })
                    .then((token: string) => {
                        // add the response token to <input> element
                        hiddenInputTag.value = token;

                        // execute users callback function if requested
                        if (hiddenInputTag.dataset.callback &&
                            window[hiddenInputTag.dataset.callback]) {
                            window[hiddenInputTag.dataset.callback](token);
                        }
                    });
            }
        });
    }

    // the reCAPTCHA badge <div> element
    const badgeTag = <HTMLDivElement>document.querySelector(`.grecaptcha-badge`);

    // Hide the reCAPTCHA badge if requested
    if (hiddenInputTag &&
        hiddenInputTag.dataset.badgeVisible &&
        hiddenInputTag.dataset.badgeVisible.trim().toLocaleLowerCase() == `false` &&
        badgeTag) {
        badgeTag.style.visibility = `hidden`;
    }
}