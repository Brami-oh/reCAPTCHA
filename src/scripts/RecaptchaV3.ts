// grecaptcha object is declared in reCAPTCHA api.
declare const grecaptcha: any;

// this section is called by v3 when the 
if (typeof grecaptcha !== `undefined` && typeof grecaptcha.ready === `function`) {
    grecaptcha.ready(() => {
        const tag = <HTMLInputElement>document.querySelector(`body input[type=hidden].recaptcha-v3-response`);
        if (tag && tag.dataset.sitekey && tag.dataset.action) {
            grecaptcha
                .execute(tag.dataset.sitekey, {
                    action: tag.dataset.action
                })
                .then((token: string) => {
                    tag.value = token;

                    if (tag.dataset.callback && window[tag.dataset.callback]) {
                        window[tag.dataset.callback](token);
                    }
                });
        }
    });
}