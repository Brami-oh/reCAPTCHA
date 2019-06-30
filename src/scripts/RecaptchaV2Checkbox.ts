function recaptchaResponseCallback(token: string) {
    const tag = <HTMLInputElement>document.querySelector(`body input[type=hidden].recaptcha-v2-response`);
    if (tag) {
        tag.value = token;

        if (tag.dataset.callback && window[tag.dataset.callback]) {
            window[tag.dataset.callback](token);
        }
    }
}